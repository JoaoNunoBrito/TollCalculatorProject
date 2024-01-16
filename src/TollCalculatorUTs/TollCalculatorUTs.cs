using Moq;
using TollFeeCalculator;
using TollFeeCalculator.Entities.Interfaces;
using static TollFeeCalculator.Entities.Enums.VehicleEnums;

namespace TollCalculatorUTs
{
    [TestClass]
    public class TollCalculatorUTs
    {
        #region Basic interval test

        [TestMethod]
        public void GetTollFee_BasicInterval_Success()
        {
            //Arrange
            var vehicle = new Mock<IVehicle>();
            vehicle.Setup(x => x.GetVehicleType()).Returns(VehicleTypeEnum.Car);
            DateTime[] dates = [new DateTime(2024, 1, 16, 6, 0, 0)];

            //Act
            var tc = new TollCalculator();
            int result = tc.GetTollFee(vehicle.Object, dates);

            //Assert 
            Assert.IsTrue(result == 0);
        }

        #endregion
    }
}