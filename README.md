# SOLID principles (Coffee Maker)

This is my implementation of the CoffeeMaker Mark IV as proposed by Robert C Martin the originator of the SOLID principles.

I use the Subscriber pattern to model the HardwareApi events, it is now a first class interface in .NET FCL.  
Consequently the client implemntation uses Reactive Extensions to publish/subscribe to the hardware event model.

Robert C Martins spec is described in his [blog](https://flylib.com/books/en/4.444.1.119/1/)
