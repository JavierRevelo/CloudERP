using AutoMapper;
using backendfepon.DTOs.InventoryMovementDTOs;
using backendfepon.Models;
using System.Globalization;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class InventoryMovementProfile : Profile
    {
        public InventoryMovementProfile()
        {
            // Mapping from InventoryMovement to InventoryMovementDTO
            CreateMap<InventoryMovement, InventoryMovementDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Movement_Id))
                .ForMember(dest => dest.product, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.movementType, opt => opt.MapFrom(src => src.InventoryMovementType.Movement_Type_Name))
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

            // Mapping from InventoryMovementDTO to InventoryMovement
            CreateMap<InventoryMovementDTO, InventoryMovement>()
                .ForMember(dest => dest.Movement_Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Product_Id, opt => opt.Ignore())
                .ForMember(dest => dest.Inventory_Movement_Id, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.date)))
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryMovementType, opt => opt.Ignore());
        }
    }
}
