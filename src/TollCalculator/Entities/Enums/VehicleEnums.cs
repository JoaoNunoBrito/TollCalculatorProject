namespace TollFeeCalculator.Entities.Enums
{
    public class VehicleEnums
    {
        public enum VehicleTypeEnum
        {
            Car,
            Truck,
            SUV,
            Motorbike,
            Tractor,
            Emergency,
            Diplomat,
            Foreign,
            Military,
            Others
        }

        //Subset of VehicleTypeEnum. Ensure that the Vehicles' name below matches the name in VehicleTypeEnum
        public enum TollFreeVehicles
        {
            Motorbike,
            Tractor,
            Emergency,
            Diplomat,
            Foreign,
            Military
        }
    }
}
