using System;

namespace CoffeeMaker.Api
{
    public class ReliefValve : IObserver<WarmerPlateStatus>
    {
        private readonly CoffeeMakerAPI hardware;

        public ReliefValve(CoffeeMakerAPI hardware)
        {
            this.hardware = hardware ?? throw new ArgumentNullException(nameof(hardware));
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(WarmerPlateStatus value)
        {
            if (value == WarmerPlateStatus.WARMER_EMPTY)
            {
                hardware.SetReliefValveState(ReliefValveState.OPEN);
            }
            else
            {
                hardware.SetReliefValveState(ReliefValveState.CLOSED);
            }
        }
    }
}