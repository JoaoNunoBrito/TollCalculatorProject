using PublicHoliday;
using TollFeeCalculator.Entities;
using TollFeeCalculator.Vehicles;
using static TollFeeCalculator.Entities.Enums.VehicleEnums;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TollFeeCalculator
{
    public class TollCalculator : ITollCalculator
    {
        private readonly IConfiguration _configuration;
        // Define the time ranges and corresponding prices (startTimeSpan, endTimeSpan, Price)
        private readonly List<TimePriceRange> _priceRanges = new List<TimePriceRange>
        {
            new TimePriceRange(new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 59), 9),
            new TimePriceRange(new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 59), 16),
            new TimePriceRange(new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 59), 22),
            new TimePriceRange(new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 59), 16),
            new TimePriceRange(new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 59), 9),
            new TimePriceRange(new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59), 16),
            new TimePriceRange(new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 59), 22),
            new TimePriceRange(new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 59), 16),
            new TimePriceRange(new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 59), 9),
            new TimePriceRange(new TimeSpan(18, 30, 0), new TimeSpan(23, 59, 59), 0),
            new TimePriceRange(new TimeSpan(0, 0, 0), new TimeSpan(5, 59, 59), 0)
        };

        public TollCalculator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Calculate the total toll fee for one day
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="dates">Date and time of all passes on one day</param>
        /// <returns> 
        /// The total toll fee for that day OR -1 if the dates are in different days, -2 if MinTimeBetweenChargesConfig not found, -3 if MaxFeeAmountConfig not found 
        /// </returns>
        public int GetTollFeeNDates(Vehicle vehicle, DateTime[] dates)
        {
            //Check if all dates are the same day
            if (!dates.All(dt => dt.Date == dates[0].Date))
                return -1;

            //Get Configs
            string? minTimeBetweenChargesConfig = _configuration.GetSection("MinTimeBetweenCharges").Value;
            if (string.IsNullOrWhiteSpace(minTimeBetweenChargesConfig)) return -2;
            int minTimeBetweenCharges = Int32.Parse(minTimeBetweenChargesConfig);

            string? maxFeeConfig = _configuration.GetSection("MaxFeeAmount").Value;
            if (string.IsNullOrWhiteSpace(maxFeeConfig)) return -3;
            int maxFeeAmount = Int32.Parse(maxFeeConfig);

            //Ensure dates are ordered correctly
            dates = dates.OrderBy(d => d.TimeOfDay).ToArray();

            int totalFee = 0;
            DateTime intervalStart = dates[0];
            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(vehicle, date);
                int tempFee = GetTollFee(vehicle, intervalStart);

                TimeSpan ts = date - intervalStart;
                double minutes = ts.TotalMinutes;


                if (minutes <= minTimeBetweenCharges)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee; //Choose highest of the fees
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }
            }

            //Check if fee does not exceed maximum daily fee
            if (totalFee > maxFeeAmount) totalFee = maxFeeAmount;

            return totalFee;
        }

        /// <summary>
        /// Calculate the toll fee for one pass
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="date">Date and time of the pass</param>
        /// <returns> 
        /// the toll fee for that pass
        /// </returns>
        public int GetTollFee(Vehicle vehicle, DateTime date)
        {
            //Free days/vehicles
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle.VehicleType)) return 0;

            //Find the applicable price range
            var applicableRange = _priceRanges.FirstOrDefault(range => date.TimeOfDay >= range.Start && date.TimeOfDay <= range.End);

            return applicableRange != null ? applicableRange.Price : 0;
        }

        #region Helpers

        private static bool IsTollFreeVehicle(VehicleTypeEnum vehicleType)
        {
            //Check if vehicleType is in whitelist
            if (Enum.IsDefined(typeof(TollFreeVehicles), vehicleType.ToString()))
                return true;

            //Pay if not on whitelist
            return false;
        }

        private static bool IsTollFreeDate(DateTime date)
        {
            //Check if it's weekend
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            //Check if it's holiday
            SwedenPublicHoliday swedenPublicHoliday = new SwedenPublicHoliday();
            return swedenPublicHoliday.IsPublicHoliday(date);
        }

        #endregion
    }
}
