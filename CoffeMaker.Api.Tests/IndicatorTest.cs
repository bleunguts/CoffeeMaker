using CoffeeMaker.Api;
using Moq;
using NUnit.Framework;
using System;

namespace CoffeMaker.Api.Tests
{

    class IndicatorTest
    {
        [Test] 
        public void whenBrewingButtonIsPushedShouldTurnOffIndicator()
        {
            var hardware = new Mock<CoffeeMakerAPI>();

            var indicator = new Indicator(hardware.Object);

            indicator.OnNext(BoilerStatus.NOT_EMPTY);
            indicator.OnNext(BrewButtonStatus.PUSHED);
            
            hardware.Verify(hw => hw.SetIndicatorState(IndicatorState.OFF));
        }

        [Test]
        public void shouldNotBrewIfNoWaterInBoiler()
        {
            var hardware = new Mock<CoffeeMakerAPI>();

            var indicator = new Indicator(hardware.Object);

            indicator.OnNext(BoilerStatus.EMPTY);
            indicator.OnNext(BrewButtonStatus.PUSHED);

            hardware.Verify(hw => hw.SetIndicatorState(IndicatorState.OFF), Times.Never);
        }

        [Test]
        public void whenBrewingCycleIsDoneTurnOnIndicator()
        {
            var hardware = new Mock<CoffeeMakerAPI>();

            var indicator = new Indicator(hardware.Object);

            // push the brew button first
            indicator.OnNext(BrewButtonStatus.PUSHED);
            // then notify the boiler has emptied everything out
            indicator.OnNext(BoilerStatus.EMPTY);

            // ensure no duplicate calls to turn on indicator
            indicator.OnNext(BoilerStatus.EMPTY);

            hardware.Verify(hw => hw.SetIndicatorState(IndicatorState.ON), Times.Once);            
        }
        
        [Test]
        public void whenHardwareIsNullThenShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new Indicator(null));
        }


        [Test]
        public void onCompletedShouldNotThrowException()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var indicator = new Indicator(hardware.Object);

            Assert.DoesNotThrow(() => indicator.OnCompleted());
        }


        [Test]
        public void onErrorShouldNotThrowException()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var indicator = new Indicator(hardware.Object);

            Assert.DoesNotThrow(() => indicator.OnError(new Exception()));
        }
    }
}
