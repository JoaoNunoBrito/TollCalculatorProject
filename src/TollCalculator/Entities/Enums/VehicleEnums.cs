namespace TollFeeCalculator.Entities.Enums
{
    public class VehicleEnums
    {
        public enum VehicleTypeEnum
        {
            Car = 0,
            Truck = 1,
            SUV = 2,
            Motorbike = 3,
            Tractor = 4,
            Emergency = 5,
            Diplomat = 6,
            Foreign = 7,
            Military = 8,
            Others = 9
        }

        //Subset of VehicleTypeEnum. Ensure that the Vehicles' int below matches the int in VehicleTypeEnum
        public enum TollFreeVehicles
        {
            Motorbike = 3,
            Tractor = 4,
            Emergency = 5,
            Diplomat = 6,
            Foreign = 7,
            Military = 8
        }
    }
}
