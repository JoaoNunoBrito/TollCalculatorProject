using PublicHoliday;
using System;
using TollFeeCalculator.Entities.Interfaces;
using TollFeeCalculator.Vehicles;
using static TollFeeCalculator.Entities.Enums.VehicleEnums;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        public TollCalculator() { }

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(date, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);

                long diffInMillies = date.Millisecond - intervalStart.Millisecond;
                long minutes = diffInMillies / 1000 / 60;

                if (minutes <= 60)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }
            }

            //Check if fee does not exceed maximum daily fee
            if (totalFee > 60) totalFee = 60;

            return totalFee;
        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle.VehicleType)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 9;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 16;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 22;
            else if (hour == 8 && minute >= 0 && minute < 29) return 16;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 9;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 16;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 22;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 16;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        #region Helpers

        public static bool IsTollFreeVehicle(VehicleTypeEnum vehicleType)
        {
            //Check if vehicleType is in whitelist
            if (Enum.IsDefined(typeof(TollFreeVehicles), vehicleType.ToString())) return true;

            //Pay if not on whitelist
            return false;
        }

        public static bool IsTollFreeDate(DateTime date)
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
