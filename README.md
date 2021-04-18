# SOLID principles (Coffee Maker)

My implementation of the CoffeeMaker Mark IV following SOLID principles by Robert C Martin.

I think the Subscriber pattern fits nicely in modelling the HardwareApi events of the CoffeeMaker machinery.  Subscriber is now a first class interface in .NET FCL which is handy! 
The client simulation borrows Reactive Extensions to publish/subscribe to the hardware event model.  I'm very fond of Reactive Extensions as one of the best programming paradigms to capture pub/sub modelling.

The spec is described in Robert C Martins [blog](https://flylib.com/books/en/4.444.1.119/1/)
