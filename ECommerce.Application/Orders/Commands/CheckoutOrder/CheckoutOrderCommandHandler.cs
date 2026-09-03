using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Orders.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Orders.Commands.CheckoutOrder;

public sealed class CheckoutOrderCommandHandler
    : IRequestHandler<CheckoutOrderCommand, CheckoutOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IPaymentGateway paymentGateway,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _paymentGateway = paymentGateway;
        _unitOfWork = unitOfWork;
    }

    public async Task<CheckoutOrderResponse> Handle(
        CheckoutOrderCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate order items
        if (request.Items == null || !request.Items.Any())
            throw new ValidationException(
                "Cannot checkout an empty order.");

        // 2. Get customer
        var customer = await _customerRepository.GetByIdAsync(
            request.CustomerId,
            cancellationToken);

        if (customer is null)
            throw new NotFoundException(
                nameof(Customer),
                request.CustomerId);

        // 3. Get products
        var productIds = request.Items
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        var products = (await _productRepository.GetByIdsAsync(
            productIds,
            cancellationToken))
            .ToDictionary(p => p.Id);

        // 4. Create order
        var order = new Order(customer.Id);

        // 5. Add order items
        foreach (var item in request.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new NotFoundException(
                    nameof(Product),
                    item.ProductId);
            }

            product.DeductStock(item.Quantity);

            order.AddItem(
                product.Id,
                item.Quantity,
                product.Price);
        }

        // 6. Get coupon if provided
        Coupon? coupon = null;

        if (!string.IsNullOrWhiteSpace(request.CouponCode))
        {
            coupon = await _orderRepository.GetCouponByCodeAsync(
                request.CouponCode,
                cancellationToken);

            if (coupon is null)
            {
                throw new ValidationException(
                    $"Coupon '{request.CouponCode}' is invalid or does not exist.");
            }

            if (!coupon.IsActive)
            {
                throw new ValidationException(
                    $"Coupon '{request.CouponCode}' is expired or inactive.");
            }
        }

        // 7. Calculate totals
        order.CalculateTotals(
            customer.IsVip,
            coupon);

        // 8. Process payment
        var paymentResult = await _paymentGateway.ChargeAsync(
            customer.Email,
            order.TotalAmount,
            cancellationToken);

        if (!paymentResult.IsSuccess)
        {
            throw new DomainException(
                $"Payment processing failed: " +
                $"{paymentResult.ErrorMessage ?? "Declined."}");
        }

        // 9. Mark order as paid
        order.MarkAsPaid(
            paymentResult.TransactionReference);

        // 10. Save order
        await _orderRepository.AddAsync(
            order,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 11. Return response
        return new CheckoutOrderResponse(
            order.Id,
            order.Status.ToString(),
            order.Subtotal,
            order.DiscountAmount,
            order.TaxAmount,
            order.ShippingFee,
            order.TotalAmount,
            paymentResult.TransactionReference);
    }
}