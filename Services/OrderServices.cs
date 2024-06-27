using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.OrderDto;
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
}