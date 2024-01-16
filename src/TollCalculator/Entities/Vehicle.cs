using static TollFeeCalculator.Entities.Enums.VehicleEnums;

namespace TollFeeCalculator.Vehicles
{
    public class Vehicle
    {
        public VehicleTypeEnum VehicleType { get; set; }
        public VehicleTypeEnum GetVehicleType()
        {
            return VehicleType;
        }
    }
}
