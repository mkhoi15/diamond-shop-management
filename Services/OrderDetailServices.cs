using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.OrderDto;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services;

public class OrderDetailServices : IOrderDetailServices
{
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IOrderServices _orderServices;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public OrderDetailServices(IOrderDetailRepository orderDetailRepository, IOrderServices orderServices, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _orderDetailRepository = orderDetailRepository;
        _orderServices = orderServices;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderServices.GetOrderByIdAsync(orderId, cancellationToken);

        if (order == null)
        {
            throw new ArgumentException("Order not found.");
        }
        
        var list = await _orderDetailRepository.FindAll().ToListAsync(cancellationToken);

        return list.Where(x => x.OrderId == orderId).ToList();

    }

    public async Task<bool> DeleteOrderDetailAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderdetailList = await GetOrderDetailsByOrderIdAsync(orderId, cancellationToken);
        
        if (orderdetailList.Count == 0)
        {
            return false;
        }
        
        foreach (var orderdetail in orderdetailList)
        {
            _orderDetailRepository.Delete(orderdetail);
        }

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return true;

    }

    public async Task<OrderDetail?> GetOrderDetailByProductId(Guid productId, CancellationToken cancellationToken)
    {
        var list =  await _orderDetailRepository.FindAll().ToListAsync(cancellationToken);

        var orderdetail = list.FirstOrDefault(od => od.ProductId == productId);
        
        return orderdetail;

    }
}