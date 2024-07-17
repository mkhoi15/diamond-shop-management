using System.Linq.Expressions;
using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.DiamondDto;
using DTO.OrderDto;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services;

public class OrderServices : IOrderServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IMapper _mapper;
    private readonly IDiamondAccessoryRepository _diamondAccessoryRepository;
    private readonly IDiamondServices _diamondServices;
    private readonly IUserServices _userServices;

    public OrderServices(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper, IDiamondAccessoryRepository diamondAccessoryRepository, IDiamondServices diamondServices, IUserServices userServices)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _mapper = mapper;
        _diamondAccessoryRepository = diamondAccessoryRepository;
        _diamondServices = diamondServices;
        _userServices = userServices;
    }

    public async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest)
    {
        var order = _mapper.Map<Order>(orderRequest);
        _orderRepository.Add(order);
        _orderDetailRepository.AddRange(order.OrderDetails);
        await _unitOfWork.SaveChangeAsync();
        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<List<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.FindAll()
            .OrderByDescending(o => o.Date)
            .ToListAsync();
            
        return _mapper.Map<List<OrderResponse>>(orders);
        
    }

    public async Task<OrderResponse> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindById(
            orderId,
            cancellationToken,
            o => o.OrderDetails,
            o => o.Deliveries,
            o => o.Customer
            );
        if (order == null)
        {
            throw new ArgumentException("Not found order with this id");
        }
        
        order.Deliveries = order.Deliveries.OrderByDescending(d => d.CreatedAt).ToList();
        
        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse> UpdateOrderAsync(Order order)
    {
        var updateOrder = await _orderRepository.FindById(order.Id);

        if (updateOrder is null)
        {
            throw new ArgumentException("Not found order with this id");
        }

        //updateOrder.Address = order.Address;
        updateOrder.Status = order.Status;
        updateOrder.Description = order.Description;
        //updateOrder.IsDeleted = order.IsDeleted;
        
        _orderRepository.Update(updateOrder);
        await _unitOfWork.SaveChangeAsync();
        
        return _mapper.Map<OrderResponse>(updateOrder);
        
    }
    
    
    public async Task<List<DiamondResponse>> GetDiamondsByOrderId(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindById(
            orderId,
            cancellationToken,
            o => o.OrderDetails
        );
        
        if (order is null)
        {
            throw new ArgumentException("Not found order with this id");
        }

        //var diamondIdList = order.OrderDetails.Select(o => o.Product?.DiamondId).ToList();
        var orderDetails = order.OrderDetails;

        List<Guid> diamondIdList = new List<Guid>();
        foreach (var orderdetail in orderDetails)
        {
            var diamondAccessory = await _diamondAccessoryRepository.FindAll()
                .FirstOrDefaultAsync(x => x.Id == orderdetail.ProductId && x.IsDeleted == false, cancellationToken);

            if (diamondAccessory != null && diamondAccessory.DiamondId != null)
            {
                diamondIdList.Add(diamondAccessory.DiamondId.Value);
            }
            
        }

        List<DiamondResponse> diamonds = new List<DiamondResponse>();
        foreach (var diamondId in diamondIdList)
        {
            var diamond = await _diamondServices.GetByIdAsync(diamondId, cancellationToken);
            
            var warranty = diamond.PaperWorks?.FirstOrDefault(p => p.Type == "warranty" && p.IsDeleted == false);
            diamond.WarrantyId = warranty?.Id;
            
            diamonds.Add(diamond);
        }
        
        return diamonds;
    }

    public async Task<List<OrderResponse>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await _userServices.GetUserByIdAsync(customerId.ToString());

        if (customer is null)
        {
            throw new ArgumentException("Not found customer with this id");
        }
        
        var orders = await _orderRepository.FindAll()
            .Where(o => o.CustomerId == customerId)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<OrderResponse>>(orders);
        
    }

    public async Task<List<OrderResponse>> GetOrdersByDeliveryManId(Guid deliveryManId, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.FindAll()
            .Where(o => o.Deliveries.Any(d => d.DeliveryManId == deliveryManId))
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<OrderResponse>>(orders);
    }
}