using static TollFeeCalculator.Entities.Enums.VehicleEnums;

namespace TollFeeCalculator.Entities.Interfaces
{
    public interface IVehicle
    {
        VehicleTypeEnum GetVehicleType();
    }
}