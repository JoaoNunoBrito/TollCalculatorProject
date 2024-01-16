using Castle.Core.Configuration;
using Moq;
using System.Reflection;
using TollFeeCalculator;
using TollFeeCalculator.Entities.Interfaces;
using TollFeeCalculator.Vehicles;
using static TollFeeCalculator.Entities.Enums.VehicleEnums;

namespace TollCalculatorUTs
{
    [TestClass]
    public class TollCalculatorUTs
    {
        #region IsTollFreeVehicle

        [TestMethod]
        public void IsTollFreeVehicle_InvalidEnum_ReturnsFalse()
        {
            //Arrange
            Type type = typeof(TollCalculator);
            MethodInfo method = type.GetMethod("IsTollFreeVehicle", BindingFlags.NonPublic | BindingFlags.Static);
            var vehicle = (VehicleTypeEnum)999;

            //Act
            bool result = (bool)method.Invoke(null, new object[] { vehicle });

            //Assert 
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsTollFreeVehicle_Car_ReturnsFalse()
        {
            //Arrange
            Type type = typeof(TollCalculator);
            MethodInfo method = type.GetMethod("IsTollFreeVehicle", BindingFlags.NonPublic | BindingFlags.Static);
            var vehicle = VehicleTypeEnum.Car;

            //Act
            bool result = (bool)method.Invoke(null, new object[] { vehicle });

            //Assert 
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsTollFreeVehicle_Motorbike_ReturnsTrue()
        {
            //Arrange
            Type type = typeof(TollCalculator);
            MethodInfo method = type.GetMethod("IsTollFreeVehicle", BindingFlags.NonPublic | BindingFlags.Static);
            var vehicle = VehicleTypeEnum.Motorbike;

            //Act
            bool result = (bool)method.Invoke(null, new object[] { vehicle });

            //Assert 
            Assert.IsTrue(result == true);
        }

        #endregion

        #region IsTollFreeDate

        [TestMethod]
        public void IsTollFreeDate_WeekDay_ReturnsFalse()
        {
            //Arrange
            Type type = typeof(TollCalculator);
            MethodInfo method = type.GetMethod("IsTollFreeDate", BindingFlags.NonPublic | BindingFlags.Static);
            var date = new DateTime(2024, 1, 16); //Tuesday

            //Act
            bool result = (bool)method.Invoke(null, new object[] { date });

            //Assert 
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsTollFreeDate_WeekendDay_ReturnsTrue()
        {
            //Arrange
            Type type = typeof(TollCalculator);
            MethodInfo method = type.GetMethod("IsTollFreeDate", BindingFlags.NonPublic | BindingFlags.Static);
            var date = new DateTime(2024, 1, 20); //Saturday

            //Act
            bool result = (bool)method.Invoke(null, new object[] { date });

            //Assert 
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void IsTollFreeDate_Holiday_ReturnsTrue()
        {
            //Arrange
            Type type = typeof(TollCalculator);
            MethodInfo method = type.GetMethod("IsTollFreeDate", BindingFlags.NonPublic | BindingFlags.Static);
            var date = new DateTime(2024, 12, 25); //Holiday

            //Act
            bool result = (bool)method.Invoke(null, new object[] { date });

            //Assert 
            Assert.IsTrue(result == true);
        }

        #endregion

        #region GetTollFee

        [TestMethod]
        public void GetTollFee_TollFreeDate_Returns0()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 12, 25, 6, 0, 0); //holiday

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTollFee_TollFreeDate2_Returns0()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 20, 6, 0, 0); //weekend

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTollFee_TollFreeVehicle_Returns0()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Emergency };
            DateTime date = new DateTime(2024, 1, 16, 6, 0, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRangeNotFound_Returns0()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 5, 59, 59, 100); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange1_Returns9()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 6, 0, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange2_Returns16()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 6, 30, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(16, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange3_Returns22()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 7, 00, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(22, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange4_Returns16()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 8, 00, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(16, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange5_Returns9()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 8, 30, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange6_Returns16()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 15, 00, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(16, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange7_Returns22()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 15, 30, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(22, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange8_Returns16()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 17, 00, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(16, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange9_Returns9()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 18, 00, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void GetTollFee_TimePriceRange10_Returns0()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime date = new DateTime(2024, 1, 16, 18, 30, 0); //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(0, result);
        }

        #endregion

        #region GetTollFee (Multiple dates)

        [TestMethod]
        public void GetTollFee2_DifferentDays_ReturnsNegative1()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime[] date = [new DateTime(2024, 1, 16, 6, 00, 00), new DateTime(2024, 1, 17, 6, 00, 00)]; //different days

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void GetTollFee2_OneDate_Returns9()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime[] date = [new DateTime(2024, 1, 16, 6, 00, 00)]; //week day

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void GetTollFee2_TwoDates_Returns31()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime[] date = [new DateTime(2024, 1, 16, 7, 01, 00), new DateTime(2024, 1, 16, 6, 00, 00)]; //week days

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(9 + 22, result);
        }

        [TestMethod]
        public void GetTollFee2_TwoDatesSameHour_Returns22()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime[] date = [new DateTime(2024, 1, 16, 6, 00, 00), new DateTime(2024, 1, 16, 7, 00, 00)]; //week days

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(22, result);
        }

        [TestMethod]
        public void GetTollFee2_Exceedes60_Returns60()
        {
            //Arrange
            var vehicle = new Vehicle() { VehicleType = VehicleTypeEnum.Car };
            DateTime[] date = [
                new DateTime(2024, 1, 16, 6, 00, 00),
                new DateTime(2024, 1, 16, 7, 01, 00),
                new DateTime(2024, 1, 16, 8, 02, 00),
                new DateTime(2024, 1, 16, 15, 01, 00),
                new DateTime(2024, 1, 16, 18, 01, 00)
            ]; //week days

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle, date);

            //Assert 
            Assert.AreEqual(60, result);
        }

        #endregion

    }
}