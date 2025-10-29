// ===== DELEGATES AND EVENTS INTERVIEW QUESTIONS =====

using System;

// Question 1: What is a delegate?
// Answer: A delegate is a type-safe function pointer that holds references to methods.
// It allows methods to be passed as parameters and enables callback mechanisms.

public class DelegateBasics
{
    // Define a delegate type
    public delegate int MathOperation(int a, int b);

    public static int Add(int a, int b) => a + b;
    public static int Multiply(int a, int b) => a * b;

    public static void Demonstrate()
    {
        // Assign method to delegate
        MathOperation operation = Add;
        int result = operation(5, 3); // Calls Add: result = 8

        // Change the method
        operation = Multiply;
        result = operation(5, 3); // Calls Multiply: result = 15

        // Pass delegate as parameter
        int answer = Calculate(10, 5, Add);
        Console.WriteLine(answer); // 15
    }

    public static int Calculate(int a, int b, MathOperation operation)
    {
        return operation(a, b); // Callback
    }
}


// Question 2: What is the difference between Func, Action, and Predicate?
// Answer: Func<T> returns a value, Action performs an action without returning, and Predicate<T> returns bool.
// They're built-in generic delegates that eliminate the need to define custom delegate types.

public class BuiltInDelegatesExample
{
    public static void Demonstrate()
    {
        // Func<input, output> - returns a value
        Func<int, int, int> add = (a, b) => a + b;
        int sum = add(3, 5); // 8

        Func<string, int> getLength = str => str.Length;
        int length = getLength("hello"); // 5

        // Action<input> - doesn't return a value
        Action<string> print = message => Console.WriteLine(message);
        print("Hello"); // Outputs: Hello

        Action<int, int> printSum = (a, b) => Console.WriteLine(a + b);
        printSum(3, 5); // Outputs: 8

        // Predicate<T> - returns bool (used for testing conditions)
        Predicate<int> isEven = num => num % 2 == 0;
        bool result = isEven(4); // true

        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var evens = numbers.FindAll(isEven); // [2, 4]
    }
}


// Question 3: What is multicast delegate?
// Answer: A multicast delegate can hold references to multiple methods and invoke them sequentially.
// You add methods using += and remove them using -=.

public class MulticastDelegateExample
{
    public delegate void Notify(string message);

    public static void EmailNotification(string msg)
    {
        Console.WriteLine($"Email: {msg}");
    }

    public static void SmsNotification(string msg)
    {
        Console.WriteLine($"SMS: {msg}");
    }

    public static void PushNotification(string msg)
    {
        Console.WriteLine($"Push: {msg}");
    }

    public static void Demonstrate()
    {
        Notify notify = EmailNotification;
        notify += SmsNotification; // Add another method
        notify += PushNotification; // Add another

        // Invoke all methods in order
        notify("User registered");
        // Output:
        // Email: User registered
        // SMS: User registered
        // Push: User registered

        // Remove a method
        notify -= SmsNotification;
        notify("New message"); // Only Email and Push
    }
}


// Question 4: What is an event and how is it different from a delegate?
// Answer: An event is a special delegate that can only be invoked from within the class that declares it.
// It provides encapsulation and follows the publisher-subscriber pattern. Events prevent external code from invoking or clearing the delegate.

public class EventExample
{
    // Delegate (not encapsulated)
    public delegate void NotifyDelegate(string message);
    public NotifyDelegate PublicDelegate; // Anyone can invoke or clear

    // Event (encapsulated)
    public event NotifyDelegate NotificationEvent; // Only this class can invoke

    public void DoSomething()
    {
        // Only the class itself can raise the event
        NotificationEvent?.Invoke("Something happened");
    }
}

public class EventUsage
{
    public static void Demonstrate()
    {
        var example = new EventExample();

        // With delegate - bad, no encapsulation
        example.PublicDelegate = null; // Can clear all subscribers!
        example.PublicDelegate?.Invoke("Direct invoke"); // Can invoke from outside

        // With event - good, encapsulated
        example.NotificationEvent += OnNotification; // Can only subscribe
        // example.NotificationEvent.Invoke("test"); // Error: can't invoke from outside
        // example.NotificationEvent = null; // Error: can't clear from outside

        example.DoSomething(); // Only the class can raise it
    }

    private static void OnNotification(string message)
    {
        Console.WriteLine($"Received: {message}");
    }
}


// Question 5: What is the EventHandler pattern in C#?
// Answer: EventHandler is the standard pattern for events in .NET. It uses EventHandler<TEventArgs>
// where TEventArgs contains event data. This ensures consistency across the framework.

public class StockPriceChangedEventArgs : EventArgs
{
    public string Symbol { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
}

public class Stock
{
    private decimal _price;
    public string Symbol { get; set; }

    // Standard event pattern
    public event EventHandler<StockPriceChangedEventArgs> PriceChanged;

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                var oldPrice = _price;
                _price = value;
                OnPriceChanged(new StockPriceChangedEventArgs
                {
                    Symbol = Symbol,
                    OldPrice = oldPrice,
                    NewPrice = value
                });
            }
        }
    }

    // Protected virtual method to raise event
    protected virtual void OnPriceChanged(StockPriceChangedEventArgs e)
    {
        PriceChanged?.Invoke(this, e);
    }
}

public class StockMonitor
{
    public static void Demonstrate()
    {
        var stock = new Stock { Symbol = "MSFT", Price = 100 };

        // Subscribe to event
        stock.PriceChanged += (sender, e) =>
        {
            Console.WriteLine($"{e.Symbol} changed from {e.OldPrice} to {e.NewPrice}");
        };

        stock.Price = 105; // Triggers event
        // Output: MSFT changed from 100 to 105
    }
}


// Question 6: What is the difference between += and = when subscribing to events?
// Answer: += adds a subscriber without removing existing ones (standard). = replaces all subscribers
// with just the new one (dangerous, usually not what you want).

public class SubscriptionExample
{
    public event EventHandler MyEvent;

    public static void Demonstrate()
    {
        var example = new SubscriptionExample();

        // Subscribe using += (correct)
        example.MyEvent += Handler1;
        example.MyEvent += Handler2;
        // Both handlers will be called

        // Subscribe using = (wrong - overwrites)
        // example.MyEvent = Handler1; // Removes all previous subscribers!
        // Only Handler1 will be called
    }

    private static void Handler1(object sender, EventArgs e)
    {
        Console.WriteLine("Handler 1");
    }

    private static void Handler2(object sender, EventArgs e)
    {
        Console.WriteLine("Handler 2");
    }
}


// Question 7: How do you prevent memory leaks with events?
// Answer: Always unsubscribe from events (using -=) when you're done, especially if the publisher lives
// longer than the subscriber. Events hold strong references and prevent garbage collection.

public class MemoryLeakExample
{
    public class Publisher
    {
        public event EventHandler DataChanged;

        public void RaiseEvent()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Subscriber : IDisposable
    {
        private Publisher _publisher;

        public Subscriber(Publisher publisher)
        {
            _publisher = publisher;
            _publisher.DataChanged += OnDataChanged; // Subscribe
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Data changed");
        }

        // Must unsubscribe to prevent memory leak
        public void Dispose()
        {
            _publisher.DataChanged -= OnDataChanged; // Unsubscribe
        }
    }

    public static void Demonstrate()
    {
        var publisher = new Publisher();

        // Bad: subscriber goes out of scope but publisher keeps reference
        var subscriber1 = new Subscriber(publisher);
        // subscriber1 can't be garbage collected!

        // Good: properly dispose and unsubscribe
        using (var subscriber2 = new Subscriber(publisher))
        {
            // Use subscriber
        } // Automatically unsubscribes
    }
}


// Question 8: What are anonymous methods and lambda expressions with delegates?
// Answer: Anonymous methods and lambdas are inline methods without a name. Lambdas are the modern,
// concise syntax. They're commonly used with delegates and events.

public class AnonymousMethodsExample
{
    public delegate int Calculate(int a, int b);

    public static void Demonstrate()
    {
        // Traditional named method
        Calculate calc1 = Add;

        // Anonymous method (older syntax)
        Calculate calc2 = delegate (int a, int b)
        {
            return a + b;
        };

        // Lambda expression (modern, preferred)
        Calculate calc3 = (a, b) => a + b;

        // With events
        var button = new Button();

        // Lambda (most common)
        button.Click += (sender, e) => Console.WriteLine("Clicked!");

        // Anonymous method
        button.Click += delegate (object sender, EventArgs e)
        {
            Console.WriteLine("Clicked with anonymous method");
        };
    }

    private static int Add(int a, int b) => a + b;
}

public class Button
{
    public event EventHandler Click;
}


// Question 9: What is a closure in C# and how does it relate to delegates?
// Answer: A closure is when a lambda or anonymous method captures variables from its outer scope.
// The captured variables remain accessible even after the outer method returns, which can lead to unexpected behavior.

public class ClosureExample
{
    public static void Demonstrate()
    {
        var actions = new List<Action>();

        // Classic closure bug
        for (int i = 0; i < 3; i++)
        {
            // Lambda captures the variable 'i', not its value
            actions.Add(() => Console.WriteLine(i));
        }

        // All print 3 (not 0, 1, 2) because they share the same 'i'
        foreach (var action in actions)
        {
            action(); // Output: 3, 3, 3
        }

        // Fix: capture the value, not the variable
        var fixedActions = new List<Action>();
        for (int i = 0; i < 3; i++)
        {
            int captured = i; // Create a new variable for each iteration
            fixedActions.Add(() => Console.WriteLine(captured));
        }

        foreach (var action in fixedActions)
        {
            action(); // Output: 0, 1, 2
        }
    }
}


// Question 10: What are covariance and contravariance with delegates?
// Answer: Covariance allows a method to return a more derived type than the delegate specifies (out).
// Contravariance allows a method to accept a less derived type than the delegate specifies (in).

public class CovarianceContravarianceExample
{
    public class Animal { public string Name { get; set; } }
    public class Dog : Animal { public string Breed { get; set; } }

    // Covariance (out) - return more derived type
    public delegate Animal AnimalFactory();

    public static Dog CreateDog()
    {
        return new Dog { Name = "Buddy", Breed = "Golden" };
    }

    // Contravariance (in) - accept less derived type
    public delegate void AnimalHandler(Dog dog);

    public static void HandleAnimal(Animal animal)
    {
        Console.WriteLine($"Handling: {animal.Name}");
    }

    public static void Demonstrate()
    {
        // Covariance: can assign method that returns Dog to delegate expecting Animal
        AnimalFactory factory = CreateDog; // Dog is more specific than Animal
        Animal animal = factory(); // Returns Dog, assigned to Animal

        // Contravariance: can assign method that takes Animal to delegate expecting Dog
        AnimalHandler handler = HandleAnimal; // Animal is less specific than Dog
        handler(new Dog { Name = "Max" }); // Pass Dog, handled as Animal

        // Built-in examples
        Func<Animal> func = () => new Dog(); // Covariance with Func<out T>
        Action<Dog> action = (Animal a) => { }; // Contravariance with Action<in T>
    }
}
