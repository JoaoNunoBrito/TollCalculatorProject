﻿using System;
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

        public Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
