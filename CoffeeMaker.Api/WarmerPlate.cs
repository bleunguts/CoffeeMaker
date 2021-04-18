using CoffeeMaker.Api;
using System;

namespace CoffeMaker.Api
{    
    public class WarmerPlate : IObserver<WarmerPlateStatus>
    {
        private readonly CoffeeMakerAPI hardware;

        public WarmerPlate(CoffeeMakerAPI hardware)
        {
            this.hardware = hardware ?? throw new ArgumentNullException(nameof(hardware));
        }

        public void OnNext(WarmerPlateStatus value)
        {
            if (value == WarmerPlateStatus.POT_NOT_EMPTY)
            {
                hardware.SetWarmerState(WarmerState.ON);
            }
            else
            {
                hardware.SetWarmerState(WarmerState.OFF);
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}