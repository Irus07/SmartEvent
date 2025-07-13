# SmartEvent

The installation is performed via the CLI with the command :
```PowerShell
dotnet add package SmartEvent --version 1.0.0
```
The SmartEvent library provides advanced functionality compared to the basic C# events.


```C# 
public partial class SmartEvent<TDelegate> : ISmartEvent<TDelegate>
		where TDelegate : Delegate
```

## Methods

1. ### ```Subscribe(TDelegate @delegate)```			
	 Subscribes the handler _(@delegate)_ to the event.

2.	### ```Subscribe(TDelegate @delegate, ushort priority)```		
	Subscribes the handler _(@delegate)_ to an event indicating the priority _(priority)_ of the call.
	The priority of a call determines the order in which the handlers are executed when an event is triggered (using the Invoke method),The higher the priority, the earlier the handler will be called.
3.  ### ```Unsubscribe(TDelegate @delegate)```
	 Deletes the event handler.

4. ### ```UnsubscribeAll()```	
	Deletes all handlers.Does not affect pinned handlers.

5. ### ```Invoke(params object[]? TDelegateParameters)```
	Calls signed handlers. The parameter **TDelegateParameters** must match the signature **TDelegate**

## How to use it

To create an instance of a class SmartEvent, you need to pass a delegate of any signature to the generalization.
```C# 
delegate void Delegate(string s);
SmartEvent<Delegate> smartEvent = new SmartEvent<Delegate>();
// Or
SmartEvent<Action<string>> smartEvent = new SmartEvent<Action<string>>();
```
To add a handler, use the Subscribe(TDelegate @delegate) method and pass the handler as a parameter or use the += operator.

```C# 
SmartEvent<Delegate> smartEvent = new SmartEvent<Delegate>();

smartEvent.Subscribe(F1)
//equal
smartEvent += F1;
//equal
smartEvent.Subscribe((string s)=> Console.WriteLine(s));

private void F1(string s) => Console.WriteLine(s);
```
To delete a handler, use the Unsubscribe(TDelegate @delegate) method and pass the handler as a parameter or use the -= operator.
```C# 
smartEvent.Unsubscribe(F1)
//equal
smartEvent -= F1;

```

