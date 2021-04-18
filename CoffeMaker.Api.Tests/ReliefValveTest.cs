using CoffeeMaker.Api;
using Moq;
using NUnit.Framework;
using System;

namespace CoffeMaker.Api.Tests
{
    public class ReliefValveTest
    {
        [Test]
        public void onNextWarmerEmptyThenOpenReliefValve()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var reliefValve = new ReliefValve(hardware.Object);
            reliefValve.OnNext(WarmerPlateStatus.WARMER_EMPTY);

            hardware.Verify((hardware) => hardware.SetReliefValveState(ReliefValveState.OPEN));
        }

        [Test]
        [TestCase(WarmerPlateStatus.POT_NOT_EMPTY)]
        [TestCase(WarmerPlateStatus.POT_EMPTY)]
        public void onNextPotIsOnStoveThenCloseReliefValve(WarmerPlateStatus status)
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var reliefValve = new ReliefValve(hardware.Object);
            reliefValve.OnNext(status);

            hardware.Verify((hardware) => hardware.SetReliefValveState(ReliefValveState.CLOSED));
        }

        [Test]
        public void whenHardwareIsNullThenShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new ReliefValve(null));
        }

        [Test]
        public void onCompletedShouldNotThrowException()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var reliefValve = new ReliefValve(hardware.Object);

            Assert.DoesNotThrow(() => reliefValve.OnCompleted());
        }


        [Test]
        public void onErrorShouldNotThrowException()
        {
            var hardware = new Mock<CoffeeMakerAPI>();
            var reliefValve = new ReliefValve(hardware.Object);

            Assert.DoesNotThrow(() => reliefValve.OnError(new Exception()));
        }

    }
}