// ===== INTERFACES INTERVIEW QUESTIONS =====

using System;

// Question 1: What is an interface and why do we use it?
// Answer: An interface is a contract that defines a set of methods and properties without implementation.
// We use it to achieve abstraction, polymorphism, and loose coupling. A class can implement multiple interfaces.

public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
    bool ValidatePayment(decimal amount);
}

public class CreditCardProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment: {amount}");
    }

    public bool ValidatePayment(decimal amount)
    {
        return amount > 0 && amount < 10000;
    }
}

public class PayPalProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment: {amount}");
    }

    public bool ValidatePayment(decimal amount)
    {
        return amount > 0;
    }
}

public class PaymentService
{
    // Depends on interface, not concrete implementation
    private readonly IPaymentProcessor _processor;

    public PaymentService(IPaymentProcessor processor)
    {
        _processor = processor;
    }

    public void MakePayment(decimal amount)
    {
        if (_processor.ValidatePayment(amount))
        {
            _processor.ProcessPayment(amount);
        }
    }
}


// Question 2: What is the difference between an interface and an abstract class?
// Answer: Interfaces define only contracts (no implementation before C# 8.0), support multiple inheritance, and can't have fields.
// Abstract classes can have implementation, fields, and constructors but only support single inheritance.

public interface IVehicle
{
    void Start(); // No implementation (pre C# 8.0)
    void Stop();
}

public abstract class Vehicle
{
    protected string _brand; // Can have fields

    public Vehicle(string brand) // Can have constructor
    {
        _brand = brand;
    }

    public abstract void Start(); // Abstract method

    public void Stop() // Concrete method
    {
        Console.WriteLine("Vehicle stopped");
    }
}

// Can implement multiple interfaces
public class Car : Vehicle, IVehicle
{
    public Car(string brand) : base(brand) { }

    public override void Start()
    {
        Console.WriteLine($"{_brand} car started");
    }
}


// Question 3: Can interfaces have default implementations in C#?
// Answer: Yes, starting with C# 8.0, interfaces can have default implementations for methods.
// This allows adding new methods to interfaces without breaking existing implementations.

public interface ILogger
{
    void Log(string message);

    // Default implementation (C# 8.0+)
    void LogError(string error)
    {
        Log($"ERROR: {error}");
    }

    void LogWarning(string warning)
    {
        Log($"WARNING: {warning}");
    }
}

public class ConsoleLogger : ILogger
{
    // Only need to implement Log
    public void Log(string message)
    {
        Console.WriteLine(message);
    }

    // Can override default implementation if needed
    public void LogError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {error}");
        Console.ResetColor();
    }
}


// Question 4: What is explicit interface implementation?
// Answer: Explicit interface implementation is when you implement interface members by fully qualifying them.
// It's used to avoid naming conflicts when implementing multiple interfaces or to hide implementation.

public interface IPrintable
{
    void Print();
}

public interface IDisplayable
{
    void Print();
}

public class Document : IPrintable, IDisplayable
{
    // Explicit implementation - only accessible through interface reference
    void IPrintable.Print()
    {
        Console.WriteLine("Printing document to printer");
    }

    void IDisplayable.Print()
    {
        Console.WriteLine("Displaying document on screen");
    }

    // Regular public method
    public void Save()
    {
        Console.WriteLine("Saving document");
    }
}

public class ExplicitInterfaceExample
{
    public static void Demonstrate()
    {
        var doc = new Document();
        // doc.Print(); // Error: Print is not accessible directly

        doc.Save(); // OK: public method

        IPrintable printable = doc;
        printable.Print(); // Output: Printing document to printer

        IDisplayable displayable = doc;
        displayable.Print(); // Output: Displaying document on screen
    }
}


// Question 5: What is the purpose of marker interfaces (empty interfaces)?
// Answer: Marker interfaces have no members and are used to mark or tag classes with specific behavior or capability.
// They're used for metadata, though attributes are now preferred for this purpose.

// Marker interface
public interface ISerializable
{
    // Empty - just marks the class as serializable
}

public class Customer : ISerializable
{
    public string Name { get; set; }
    public int Id { get; set; }
}

public class DataProcessor
{
    public void Process(object obj)
    {
        // Check if object is marked as serializable
        if (obj is ISerializable)
        {
            Console.WriteLine("Object can be serialized");
            // Serialize it
        }
        else
        {
            Console.WriteLine("Object cannot be serialized");
        }
    }
}


// Question 6: What are interface properties and how do they work?
// Answer: Interface properties define getters and setters without implementation. Implementing classes
// must provide the implementation. Properties can be read-only, write-only, or read-write.

public interface IProduct
{
    string Name { get; set; } // Read-write property
    decimal Price { get; } // Read-only property
    int Stock { set; } // Write-only property (rare)
}

public class Product : IProduct
{
    private string _name;
    private decimal _price;
    private int _stock;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public decimal Price => _price; // Read-only

    public int Stock
    {
        set => _stock = value; // Write-only
    }

    public Product(decimal price)
    {
        _price = price;
    }
}


// Question 7: Can you have static members in interfaces?
// Answer: Yes, starting with C# 8.0, interfaces can have static members including methods, properties, and fields.
// They're accessed through the interface type, not through instances.

public interface IMathOperations
{
    // Static method (C# 8.0+)
    static int Add(int a, int b)
    {
        return a + b;
    }

    // Static property (C# 11+)
    static abstract int MaxValue { get; }

    // Instance method (normal)
    int Multiply(int a, int b);
}

public class MathOperations : IMathOperations
{
    public static int MaxValue => int.MaxValue;

    public int Multiply(int a, int b)
    {
        return a * b;
    }

    public static void Demonstrate()
    {
        // Call static method through interface
        int sum = IMathOperations.Add(5, 3);
        Console.WriteLine(sum); // 8

        // Instance method requires object
        var math = new MathOperations();
        int product = math.Multiply(5, 3);
        Console.WriteLine(product); // 15
    }
}


// Question 8: What is interface segregation principle (ISP)?
// Answer: ISP states that clients shouldn't be forced to depend on interfaces they don't use.
// It's better to have many small, specific interfaces than one large, general-purpose interface.

// BAD: Fat interface (violates ISP)
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

// Problem: Robot workers don't eat or sleep
public class Robot : IWorker
{
    public void Work() => Console.WriteLine("Robot working");
    public void Eat() => throw new NotImplementedException(); // Forced to implement
    public void Sleep() => throw new NotImplementedException(); // Forced to implement
}

// GOOD: Segregated interfaces (follows ISP)
public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

public class Human : IWorkable, IEatable, ISleepable
{
    public void Work() => Console.WriteLine("Human working");
    public void Eat() => Console.WriteLine("Human eating");
    public void Sleep() => Console.WriteLine("Human sleeping");
}

public class BetterRobot : IWorkable // Only implements what it needs
{
    public void Work() => Console.WriteLine("Robot working");
}


// Question 9: How do you implement multiple interfaces with conflicting members?
// Answer: Use explicit interface implementation to disambiguate between conflicting members.
// Each interface's member is implemented separately and accessed through that interface reference.

public interface IEnglishSpeaker
{
    void Greet();
}

public interface ISpanishSpeaker
{
    void Greet();
}

public class BilingualPerson : IEnglishSpeaker, ISpanishSpeaker
{
    // Explicit implementation for IEnglishSpeaker
    void IEnglishSpeaker.Greet()
    {
        Console.WriteLine("Hello!");
    }

    // Explicit implementation for ISpanishSpeaker
    void ISpanishSpeaker.Greet()
    {
        Console.WriteLine("¡Hola!");
    }

    public static void Demonstrate()
    {
        var person = new BilingualPerson();

        IEnglishSpeaker english = person;
        english.Greet(); // Output: Hello!

        ISpanishSpeaker spanish = person;
        spanish.Greet(); // Output: ¡Hola!
    }
}


// Question 10: What is the difference between programming to an interface vs programming to an implementation?
// Answer: Programming to an interface means depending on abstractions (interfaces) rather than concrete classes.
// This makes code more flexible, testable, and maintainable because implementations can be swapped without changing client code.

// BAD: Programming to implementation (tight coupling)
public class EmailService
{
    public void SendEmail(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

public class NotificationServiceBad
{
    private EmailService _emailService = new EmailService(); // Tightly coupled

    public void Notify(string message)
    {
        _emailService.SendEmail(message);
    }
}

// GOOD: Programming to interface (loose coupling)
public interface INotificationService
{
    void Send(string message);
}

public class EmailNotificationService : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

public class SmsNotificationService : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending SMS: {message}");
    }
}

public class NotificationServiceGood
{
    private readonly INotificationService _notificationService;

    // Depends on abstraction, not concrete class
    public NotificationServiceGood(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void Notify(string message)
    {
        _notificationService.Send(message);
    }
}

public class InterfaceVsImplementationExample
{
    public static void Demonstrate()
    {
        // Easy to swap implementations
        var emailNotifier = new NotificationServiceGood(new EmailNotificationService());
        emailNotifier.Notify("Hello via email");

        var smsNotifier = new NotificationServiceGood(new SmsNotificationService());
        smsNotifier.Notify("Hello via SMS");

        // Easy to test with mock implementations
    }
}
