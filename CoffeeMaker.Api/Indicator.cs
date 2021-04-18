using System;

namespace CoffeeMaker.Api
{
    public class Indicator : IObserver<BoilerStatus>, IObserver<BrewButtonStatus>
    {
        private CoffeeMakerAPI hardware;
        private bool isBrewing;
        private bool isBoilerFull;

        public Indicator(CoffeeMakerAPI hardware)
        {
            this.hardware = hardware ?? throw new ArgumentNullException(nameof(hardware));
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(BoilerStatus value)
        {
            isBoilerFull = value == BoilerStatus.NOT_EMPTY;
            if (isBrewing && value == BoilerStatus.EMPTY)
            {
                hardware.SetIndicatorState(IndicatorState.ON);
                isBrewing = false;
            }
        }

        public void OnNext(BrewButtonStatus value)
        {
            if (isBoilerFull && value == BrewButtonStatus.PUSHED)
            {
                // start brewing
                hardware.SetIndicatorState(IndicatorState.OFF);
                isBrewing = true;
            }
        }
    }
}
