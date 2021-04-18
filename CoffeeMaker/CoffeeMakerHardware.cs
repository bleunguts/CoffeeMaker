using CoffeeMaker.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker
{
    public interface ICoffeeMakerHardware
    {
        BoilerState BoilerState { get; }
        BoilerStatus BoilerStatus { get; set; }
        BrewButtonStatus BrewButtonStatus { get; set; }
        IndicatorState IndicatorState { get; }
        ReliefValveState ReliefValveState { get; }
        WarmerPlateStatus WarmerPlateStatus { get; set; }
        WarmerState WarmerState { get; }

        BoilerStatus GetBoilerStatus();
        BrewButtonStatus GetBrewButtonStatus();
        WarmerPlateStatus GetWarmerPlateStatus();
        void SetBoilerState(BoilerState s);
        void SetIndicatorState(IndicatorState s);
        void SetReliefValveState(ReliefValveState s);
        void SetWarmerState(WarmerState s);
    }

    public class CoffeeMakerHardware : CoffeeMakerAPI, ICoffeeMakerHardware
    {
        public BoilerStatus BoilerStatus { get; set; }
        public BrewButtonStatus BrewButtonStatus { get; set; }
        public WarmerPlateStatus WarmerPlateStatus { get; set; }

        public IndicatorState IndicatorState { get; internal set; }
        public BoilerState BoilerState { get; internal set; }
        public WarmerState WarmerState { get; internal set; }
        public ReliefValveState ReliefValveState { get; internal set; }

        #region Queries
        /*      
        *      * This function returns the status of the boiler switch.      
        *      * The boiler switch is a float switch that detects if      
        *      * there is more than 1/2 cup of water in the boiler.     
        *      
        */
        public BoilerStatus GetBoilerStatus()
        {
            return this.BoilerStatus;
        }

        /*      
        *      * This function returns the status of the brew button.      
        *      * The brew button is a momentary switch that remembers      
        *      * its state. Each call to this function returns the      
        *      * remembered state and then resets that state to      
        *      * NOT_PUSHED.      
        *      *      
        *      * Thus, even if this function is polled at a very slow      
        *      * rate, it will still detect when the brew button is      
        *      * pushed.      
        */
        public BrewButtonStatus GetBrewButtonStatus()
        {
            // yes you must be looking at this thinking this is wierd
            // the hardware api is designed wierd see above comment
            var currentStatus = BrewButtonStatus;
            BrewButtonStatus = BrewButtonStatus.NOT_PUSHED;
            return currentStatus;
        }

        /*      
        *      * This function returns the status of the warmer-plate     
        *      * sensor. This sensor detects the presence of the pot      
        *      * and whether it has coffee in it.      
        */
        public WarmerPlateStatus GetWarmerPlateStatus()
        {
            return this.WarmerPlateStatus;
        }
        #endregion

        #region Commands
        public void SetBoilerState(BoilerState s)
        {
            this.BoilerState = s;
        }

        public void SetIndicatorState(IndicatorState s)
        {
            this.IndicatorState = s;
        }

        public void SetReliefValveState(ReliefValveState s)
        {
            this.ReliefValveState = s;
        }

        public void SetWarmerState(WarmerState s)
        {
            this.WarmerState = s;
        }
        #endregion
    }
}
