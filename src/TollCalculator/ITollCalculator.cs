using PublicHoliday;
using TollFeeCalculator.Entities;
using TollFeeCalculator.Vehicles;
using static TollFeeCalculator.Entities.Enums.VehicleEnums;

namespace TollFeeCalculator
{
    public interface ITollCalculator
    {
        public int GetTollFeeNDates(Vehicle vehicle, DateTime[] dates);
        public int GetTollFee(Vehicle vehicle, DateTime date);
    }
}
