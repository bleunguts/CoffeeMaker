using CoffeeMaker.Api;
using CoffeMaker.Api;
using System;
using System.Reactive.Linq;

namespace CoffeeMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new TheCoffeeMaker(new CoffeeMakerHardware());

            // setup initial conditions of hardware
            api.SetReliefValveState(ReliefValveState.CLOSED);
            api.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            var brewEvents = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => api.GetBrewButtonStatus())
                .Publish();

            var boilerEvents = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => api.GetBoilerStatus())
                .Publish();

            var warmerEvents = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => api.GetWarmerPlateStatus())
                .Publish();
            var coffeeMakerEvents = Observable
                    .Interval(TimeSpan.FromMilliseconds(100))
                    .Publish();

            var boiler = new Boiler(api);
            var indicator = new Indicator(api);
            var reliefValve = new ReliefValve(api);
            var warmer = new WarmerPlate(api);

            using(brewEvents.Subscribe(boiler))
            using(brewEvents.Subscribe(indicator))
            using(boilerEvents.Subscribe(boiler))
            using(boilerEvents.Subscribe(indicator))
            using(warmerEvents.Subscribe(reliefValve))
            using(warmerEvents.Subscribe(warmer))
            using(coffeeMakerEvents.Subscribe(_ => api.Tick()))
            {
                brewEvents.Connect();
                boilerEvents.Connect();
                warmerEvents.Connect();
                coffeeMakerEvents.Connect();

                // Sequence to brew coffee
                // fill 
                // - boiler not empty
                // press 
                // - boiler on
                // drip
                // - warmer plate not empty
                // wait..
                // -pot on
                // drip
                // take
                // -warmer empty
                // - warmer plate off
                // - valve open
                // drip
                // drip
                // empty
                // dry

                // new brew cycle
                // fill 
                // press
                // drip               
                while (true)
                {
                    string command = Console.ReadLine();
                    if (command == "exit") break;
                    if (command == "brew") api.PressBrewButton();
                    if (command == "drink") api.DrinkCoffee();
                    if (command == "refill") api.RefillWater();
                    if (command == "insert") api.InsertPot();
                    if (command == "remove") api.RemovePot();

                    Console.Clear();
                    Print(api);
                }
            }            
        }

        private static void Print(TheCoffeeMaker coffeeMaker)
        {
            Console.WriteLine("Coffe maker status {0:T}:", DateTime.Now);

            Console.Write("Ready indicator:\t");
            Console.ForegroundColor =
                coffeeMaker.IndicatorState == IndicatorState.ON ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(coffeeMaker.IndicatorState);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Boiler:\t\t\t");
            Console.ForegroundColor =
                coffeeMaker.BoilerStatus == BoilerStatus.NOT_EMPTY ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(coffeeMaker.BoilerStatus);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Boiler:\t\t\t");
            Console.ForegroundColor =
                coffeeMaker.BoilerState == BoilerState.ON ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(coffeeMaker.BoilerState);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Warmer plate:\t\t");
            switch (coffeeMaker.WarmerPlateStatus)
            {
                case WarmerPlateStatus.WARMER_EMPTY:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case WarmerPlateStatus.POT_EMPTY:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case WarmerPlateStatus.POT_NOT_EMPTY:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    break;
            }
            Console.Write(coffeeMaker.WarmerPlateStatus);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Warmer plate:\t\t");
            Console.ForegroundColor =
                coffeeMaker.WarmerState == WarmerState.ON ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(coffeeMaker.WarmerState);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Relief valve:\t\t");
            Console.ForegroundColor =
                coffeeMaker.ReliefValveState == ReliefValveState.CLOSED ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(coffeeMaker.ReliefValveState);
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine($"Water level:         {coffeeMaker.WaterLevel} %");
            Console.WriteLine($"Coffee level:        {coffeeMaker.CoffeeLevel} %");
        }
    }
}
