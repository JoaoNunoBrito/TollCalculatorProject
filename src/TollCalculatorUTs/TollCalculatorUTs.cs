using Castle.Core.Configuration;
using Moq;
using TollFeeCalculator;
using TollFeeCalculator.Entities.Interfaces;
using TollFeeCalculator.Vehicles;
using static TollFeeCalculator.Entities.Enums.VehicleEnums;

namespace TollCalculatorUTs
{
    [TestClass]
    public class TollCalculatorUTs
    {
        //private Mock<IVehicle> _vehicle;

        //[TestInitialize()]
        //public void TestInitialize()
        //{
        //    _vehicle = new Mock<IVehicle>();
        //}

        #region IsTollFreeVehicle

        [TestMethod]
        public void IsTollFreeVehicle_InvalidEnum_ReturnsFalse()
        {
            //Arrange
            var vehicle = (VehicleTypeEnum)999;

            //Act
            bool result = TollCalculator.IsTollFreeVehicle(vehicle);

            //Assert 
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsTollFreeVehicle_Car_ReturnsFalse()
        {
            //Arrange
            var vehicle = VehicleTypeEnum.Car;

            //Act
            bool result = TollCalculator.IsTollFreeVehicle(vehicle);

            //Assert 
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsTollFreeVehicle_Motorbike_ReturnsTrue()
        {
            //Arrange
            var vehicle = VehicleTypeEnum.Motorbike;

            //Act
            bool result = TollCalculator.IsTollFreeVehicle(vehicle);

            //Assert 
            Assert.IsTrue(result == true);
        }

        #endregion

        #region IsTollFreeDate

        [TestMethod]
        public void IsTollFreeDate_WeekDay_ReturnsFalse()
        {
            //Arrange
            var date = new DateTime(2024, 1, 16); //Tuesday

            //Act
            bool result = TollCalculator.IsTollFreeDate(date);

            //Assert 
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsTollFreeDate_WeekendDay_ReturnsTrue()
        {
            //Arrange
            var date = new DateTime(2024, 1, 20); //Saturday

            //Act
            bool result = TollCalculator.IsTollFreeDate(date);

            //Assert 
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void IsTollFreeDate_Holiday_ReturnsTrue()
        {
            //Arrange
            var date = new DateTime(2024, 12, 25); //Holiday

            //Act
            bool result = TollCalculator.IsTollFreeDate(date);

            //Assert 
            Assert.IsTrue(result == true);
        }

        #endregion

        #region Basic interval test

        //[TestMethod]
        //public void GetTollFee_BasicInterval_Success()
        //{
        //    //Arrange
        //    var vehicle = new Mock<IVehicle>();
        //    vehicle.Setup(x => x.GetVehicleType()).Returns(VehicleTypeEnum.Car);
        //    DateTime[] dates = [new DateTime(2024, 1, 16, 6, 0, 0)];

        //    //Act
        //    var tc = new TollCalculator();
        //    int result = tc.GetTollFee(vehicle.Object, dates);

        //    //Assert 
        //    Assert.IsTrue(result == 0);
        //}

        #endregion
    }
}