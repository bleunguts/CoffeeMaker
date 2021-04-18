using CoffeeMaker.Api;
using Moq;
using NUnit.Framework;
using System;

namespace CoffeMaker.Api.Tests
{
    public class WarmerPlateTest
    {     
        [Test]
        public void onNextPotIsNotEmptyTurnsOnHardwareWarmer()
        {
            var hardware = new Mock<CoffeeMakerAPI>(); // need a mocking library           
            var warmerPlate = new WarmerPlate(hardware.Object);

            warmerPlate.OnNext(WarmerPlateStatus.POT_NOT_EMPTY);
                       
            hardware.Verify((api) =>            
                api.SetWarmerState(WarmerState.ON)            
            );
        }

        [Test]
        [TestCase(WarmerPlateStatus.WARMER_EMPTY)]
        [TestCase(WarmerPlateStatus.POT_EMPTY)]
        public void onNextPotIsEmptyTurnHardwareWarmerOff(WarmerPlateStatus status)
        {
            // Pot has nothing so turn the Warmer Off, don't waste electricity
            var hardware = new Mock<CoffeeMakerAPI>(); 
            var warmerPlate = new WarmerPlate(hardware.Object);

            warmerPlate.OnNext(status);

            hardware.Verify((api) =>
                api.SetWarmerState(WarmerState.OFF)
            );
        }

        [Test]
        public void whenHardwareIsNullThenShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new WarmerPlate(null));
        }

        [Test]
        public void onCompletedShouldNotThrowException()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var warmerPlate = new WarmerPlate(hardware.Object);

            Assert.DoesNotThrow(() => warmerPlate.OnCompleted());
        }


        [Test]
        public void onErrorShouldNotThrowException()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var warmerPlate = new WarmerPlate(hardware.Object);

            Assert.DoesNotThrow(() => warmerPlate.OnError(new Exception()));
        }
    }    
}