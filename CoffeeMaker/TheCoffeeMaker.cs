using CoffeeMaker.Api;
using System;

namespace CoffeeMaker
{
    public class TheCoffeeMaker : CoffeeMakerAPI, ICoffeeMakerHardware
    {
        private readonly ICoffeeMakerHardware hardware;
        private int _coffeeLevel;
        private int _waterLevel;
        private bool _isPotOnWarmerPlate;

        public int CoffeeLevel => this._coffeeLevel;
        public int WaterLevel => this._waterLevel;

        public TheCoffeeMaker(ICoffeeMakerHardware hardware)
        {
            this.hardware = hardware;
        }
        public void PressBrewButton()
        {
            BrewButtonStatus = BrewButtonStatus.PUSHED;
        }

        public void DrinkCoffee()
        {
            _coffeeLevel = 0;
        }

        public void RefillWater()
        {
            _waterLevel = 100;
        }

        public void InsertPot()
        {
            _isPotOnWarmerPlate = true;
        }

        public void RemovePot()
        {
            _isPotOnWarmerPlate = false;
        }

        public void Tick()
        {
            if (_waterLevel > 0 && BoilerState == BoilerState.ON)
            {
                _waterLevel--;
                if (ReliefValveState == ReliefValveState.CLOSED)
                    _coffeeLevel++;
            }
        }    

        #region ICoffeeMakerAPI interface methods
        public BoilerState BoilerState => hardware.BoilerState;

        public BoilerStatus BoilerStatus { get => hardware.BoilerStatus; set => hardware.BoilerStatus = value; }

        public BrewButtonStatus BrewButtonStatus { get => hardware.BrewButtonStatus; set => hardware.BrewButtonStatus = value; }

        public IndicatorState IndicatorState => hardware.IndicatorState;

        public ReliefValveState ReliefValveState => hardware.ReliefValveState;

        public WarmerState WarmerState => hardware.WarmerState;

        public WarmerPlateStatus WarmerPlateStatus { get => hardware.WarmerPlateStatus; set => hardware.WarmerPlateStatus = value; }

        public BoilerStatus GetBoilerStatus()
        {
            hardware.BoilerStatus = _waterLevel > 0 ? BoilerStatus.NOT_EMPTY : BoilerStatus.EMPTY;
            return hardware.GetBoilerStatus();
        }

        public BrewButtonStatus GetBrewButtonStatus()
        {
            return hardware.GetBrewButtonStatus();
        }

        public WarmerPlateStatus GetWarmerPlateStatus()
        {
            return _isPotOnWarmerPlate
                ? _coffeeLevel > 0
                    ? hardware.WarmerPlateStatus = WarmerPlateStatus.POT_NOT_EMPTY
                    : hardware.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY
                : hardware.WarmerPlateStatus = WarmerPlateStatus.WARMER_EMPTY;           
        }

        public void SetBoilerState(BoilerState s)
        {
            hardware.SetBoilerState(s);
        }

        public void SetIndicatorState(IndicatorState s)
        {
            hardware.SetIndicatorState(s);
        }

        public void SetReliefValveState(ReliefValveState s)
        {
            hardware.SetReliefValveState(s);
        }

        public void SetWarmerState(WarmerState s)
        {
            hardware.SetWarmerState(s);
        }
        #endregion    
    }
}
