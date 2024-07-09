using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.DeliveryDto;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services;

public class DeliveryServices : IDeliveryServices
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserServices _userServices;

    public DeliveryServices(IDeliveryRepository deliveryRepository, IMapper mapper, IUnitOfWork unitOfWork, IUserServices userServices)
    {
        _deliveryRepository = deliveryRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userServices = userServices;
    }

    public async Task<DeliveryResponse> CreateDeliveryAsync(DeliveryRequest deliveryRequest)
    {
        var delivery = _mapper.Map<Delivery>(deliveryRequest);
        _deliveryRepository.Add(delivery);
        await _unitOfWork.SaveChangeAsync();
        return _mapper.Map<DeliveryResponse>(delivery);
    }

    public async Task<List<DeliveryResponse>> GetAllDeliveriesAsync()
    {
        var deliveries = await _deliveryRepository.FindAll().ToListAsync();
        
        return _mapper.Map<List<DeliveryResponse>>(deliveries);
    }

    public async Task<DeliveryResponse> GetDeliveryByIdAsync(Guid deliveryId, CancellationToken cancellationToken)
    {
        var delivery = await _deliveryRepository.FindById(deliveryId, cancellationToken);
        
        return _mapper.Map<DeliveryResponse>(delivery);
    }

    public async Task<DeliveryResponse> UpdateDeliveryAsync(DeliveryRequest deliveryRequest)
    {
        var updatedDelivery = _mapper.Map<Delivery>(deliveryRequest);

        if (updatedDelivery is null)
        {
            throw new ArgumentException("Not found delivery to update");
        }
        
        updatedDelivery.Location = deliveryRequest.Location;
        updatedDelivery.Status = deliveryRequest.Status;
        
        _deliveryRepository.Update(updatedDelivery);
        await _unitOfWork.SaveChangeAsync();
        
        return _mapper.Map<DeliveryResponse>(updatedDelivery);
    }

    public async Task<List<DeliveryResponse>> GetDeliveriesByDeliveryManId(Guid deliveryManId, CancellationToken cancellationToken)
    {
        var deliveries = await _deliveryRepository.FindAll()
            .Where(d => d.DeliveryManId == deliveryManId)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<DeliveryResponse>>(deliveries);
        
    }

    public async Task<List<DeliveryResponse>> GetDeliveriesByOrderId(Guid orderId, CancellationToken cancellationToken)
    {
        var deliveries = await _deliveryRepository.FindAll()
            .Where(d => d.OrderId == orderId)
            .ToListAsync(cancellationToken);
        
        var deliveryMen = await _userServices.GetDeliveryMenAsync(cancellationToken);
        
        var deliveryResponses = deliveries.Select(delivery =>
        {
            var deliveryResponse = _mapper.Map<DeliveryResponse>(delivery);

            // Find the delivery man whose ID matches DeliveryManId
            var deliveryMan = deliveryMen.FirstOrDefault(dm => dm.Id == delivery.DeliveryManId.ToString());
        
            if (deliveryMan != null)
            {
                deliveryResponse.DeliveryManFullName = deliveryMan.FullName;
            }
        
            return deliveryResponse;
        }).OrderBy(d => d.CreatedAt).ToList();

        return deliveryResponses;
        
    }
}