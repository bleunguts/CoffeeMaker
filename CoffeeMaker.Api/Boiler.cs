using System;

namespace CoffeeMaker.Api
{
    public class Boiler : IObserver<BoilerStatus>, IObserver<BrewButtonStatus>
    {
        private readonly CoffeeMakerAPI hardware;
        private bool boilerIsFull;

        public Boiler(CoffeeMakerAPI hardware)
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
            boilerIsFull = value == BoilerStatus.NOT_EMPTY;
            if (!boilerIsFull)
                hardware.SetBoilerState(BoilerState.OFF);
        }

        public void OnNext(BrewButtonStatus value)
        {
            if (boilerIsFull && value == BrewButtonStatus.PUSHED)
            {
                hardware.SetBoilerState(BoilerState.ON);
            }
        }
    }
}
