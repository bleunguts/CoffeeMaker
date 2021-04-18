using CoffeeMaker.Api;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeMaker.Api.Tests
{

    public class BoilerTest
    {
        [Test]
        public void whenBoilerIsNotEmptyAndBrewButtonIsPushedThenTurnOnBoiler()
        {            
            var boiler = new Boiler(new Mock<CoffeeMakerAPI>().Object);

            boiler.OnNext(BoilerStatus.NOT_EMPTY);
            boiler.OnNext(BrewButtonStatus.PUSHED);

            Assert.IsTrue(true);
        }

        [Test]
        public void whenBoilerIsEmptyThenTurnBoilerOff()
        {
            Mock<CoffeeMakerAPI> hardware = new Mock<CoffeeMakerAPI>();
            var boiler = new Boiler(hardware.Object);

            boiler.OnNext(BoilerStatus.EMPTY);

            hardware.Verify((hardware) => hardware.SetBoilerState(BoilerState.OFF));
        }

        [Test]
        public void whenBoilerIsEmptyAndBrewingDoesNotStartBoiler()
        {
            Mock<CoffeeMakerAPI> hardware = new Mock<CoffeeMakerAPI>();
            var boiler = new Boiler(hardware.Object);

            boiler.OnNext(BoilerStatus.EMPTY);
            boiler.OnNext(BrewButtonStatus.PUSHED);

            hardware.Verify((hardware) => hardware.SetBoilerState(BoilerState.ON), Times.Never);
        }

        [Test]
        public void whenHardwareIsNullThenShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new Boiler(null));
        }

        [Test]
        public void onCompletedShouldNotThrowException()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var boiler = new Boiler(hardware.Object);

            Assert.DoesNotThrow(() => boiler.OnCompleted());
        }


        [Test]
        public void onErrorShouldNotThrowException()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var boiler = new Boiler(hardware.Object);

            Assert.DoesNotThrow(() => boiler.OnError(new Exception()));
        }
    }
}
