using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
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

    public OrderServices(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper, IDiamondAccessoryRepository diamondAccessoryRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _mapper = mapper;
        _diamondAccessoryRepository = diamondAccessoryRepository;
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
        var orders = await _orderRepository.FindAll().ToListAsync();

        return _mapper.Map<List<OrderResponse>>(orders);
        
    }

    public async Task<OrderResponse> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindById(
            orderId,
            cancellationToken,
            o => o.OrderDetails
            );
        
        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse> UpdateOrderAsync(Order order)
    {
        var updateOrder = await _orderRepository.FindById(order.Id);

        if (updateOrder is null)
        {
            throw new ArgumentException("Not found order with this id");
        }

        updateOrder.Address = order.Address;
        updateOrder.Status = order.Status;
        //updateOrder.IsDeleted = order.IsDeleted;
        
        _orderRepository.Update(updateOrder);
        await _unitOfWork.SaveChangeAsync();
        
        return _mapper.Map<OrderResponse>(updateOrder);
        
    }
}