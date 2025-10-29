# Four Pillars of Object-Oriented Programming

The four pillars of OOP are fundamental principles that guide how we design and structure object-oriented systems. They help create maintainable, scalable, and flexible software.

---

## 1. Polymorphism

**Definition:** Polymorphism allows objects of different classes to be treated as objects of a common base class. It enables methods to behave differently based on the actual object type at runtime.

**Key Concepts:**
- **Method Overriding:** Derived classes provide specific implementations of base class methods
- **Method Overloading:** Multiple methods with the same name but different parameters
- **Interface Implementation:** Different classes implementing the same interface contract

**Why It Matters:**
- Makes code more flexible and extensible
- Allows you to write code that works with base types but executes derived-type behavior
- Enables the Open/Closed Principle (open for extension, closed for modification)

**Real-World Example:**
```csharp
// Base class with virtual method
public abstract class Shape
{
    public abstract double CalculateArea();
}

// Different shapes override the method with specific implementations
public class Circle : Shape
{
    public double Radius { get; set; }
    public override double CalculateArea() => Math.PI * Radius * Radius;
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public override double CalculateArea() => Width * Height;
}

// Polymorphic behavior - same method call, different behavior
Shape shape1 = new Circle { Radius = 5 };
Shape shape2 = new Rectangle { Width = 4, Height = 6 };

Console.WriteLine(shape1.CalculateArea()); // Circle's implementation
Console.WriteLine(shape2.CalculateArea()); // Rectangle's implementation
```

---

## 2. Encapsulation

**Definition:** Encapsulation is the practice of hiding an object's internal state and exposing functionality only through well-defined public interfaces (methods and properties).

**Key Concepts:**
- **Information Hiding:** Internal implementation details are private
- **Controlled Access:** Public methods and properties control how data is accessed and modified
- **Data Protection:** Validation and business rules are enforced when data changes

**Why It Matters:**
- Protects data integrity by preventing invalid states
- Reduces coupling between components
- Makes code easier to maintain and change internally without affecting external code
- Provides clear boundaries and contracts

**Real-World Example:**
```csharp
public class BankAccount
{
    // Private field - encapsulated internal state
    private decimal _balance;

    // Public read-only property - controlled access
    public decimal Balance => _balance;

    public string AccountNumber { get; private set; }

    // Public methods with validation - controlled modification
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive");

        _balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive");

        if (amount > _balance)
            return false; // Insufficient funds

        _balance -= amount;
        return true;
    }
}

// Usage - clients can't bypass business rules
var account = new BankAccount();
account.Deposit(100);
account.Withdraw(50);
// account._balance = 1000000; // Compile error - can't access private field
```

---

## 3. Abstraction

**Definition:** Abstraction is the process of hiding complex implementation details and showing only the essential features of an object. It focuses on *what* an object does rather than *how* it does it.

**Key Concepts:**
- **Abstract Classes:** Define partial implementations with abstract methods
- **Interfaces:** Define pure contracts without implementation
- **Essential Features Only:** Hide complexity behind simple, meaningful operations

**Why It Matters:**
- Simplifies complex systems by breaking them into understandable components
- Allows you to work with high-level concepts without worrying about details
- Enables multiple implementations of the same concept
- Improves code maintainability and testability

**Real-World Example:**
```csharp
// Abstraction through interface - defines WHAT without HOW
public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
    string GetTransactionId();
}

// Different implementations hide their complexity
public class CreditCardProcessor : IPaymentProcessor
{
    private string _cardNumber;
    private string _cvv;

    public bool ProcessPayment(decimal amount)
    {
        // Complex credit card processing logic hidden
        ValidateCard();
        ContactPaymentGateway();
        EncryptData();
        return true;
    }

    public string GetTransactionId() => Guid.NewGuid().ToString();

    // Private implementation details
    private void ValidateCard() { /* ... */ }
    private void ContactPaymentGateway() { /* ... */ }
    private void EncryptData() { /* ... */ }
}

public class PayPalProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        // Different implementation, same interface
        AuthenticateWithPayPal();
        ProcessThroughPayPal();
        return true;
    }

    public string GetTransactionId() => $"PAYPAL-{Guid.NewGuid()}";

    private void AuthenticateWithPayPal() { /* ... */ }
    private void ProcessThroughPayPal() { /* ... */ }
}

// Client code works with abstraction, doesn't know implementation details
public class OrderService
{
    private readonly IPaymentProcessor _processor;

    public OrderService(IPaymentProcessor processor)
    {
        _processor = processor; // Works with any implementation
    }

    public void CompleteOrder(decimal amount)
    {
        if (_processor.ProcessPayment(amount))
        {
            Console.WriteLine($"Payment processed: {_processor.GetTransactionId()}");
        }
    }
}
```

---

## 4. Inheritance

**Definition:** Inheritance allows a class (child/derived class) to inherit properties, methods, and behavior from another class (parent/base class), promoting code reuse and establishing hierarchical relationships.

**Key Concepts:**
- **Base Class:** The parent class that provides common functionality
- **Derived Class:** The child class that inherits and extends base functionality
- **Code Reuse:** Avoid duplicating code by inheriting common behavior
- **"Is-A" Relationship:** The derived class "is a" specialized version of the base class

**Why It Matters:**
- Promotes code reuse and reduces duplication
- Establishes clear hierarchical relationships between classes
- Enables polymorphism through method overriding
- Supports the DRY principle (Don't Repeat Yourself)

**Important Note:** Favor composition over inheritance when the relationship is not a true "is-a" relationship.

**Real-World Example:**
```csharp
// Base class with common functionality
public class Employee
{
    public string Name { get; set; }
    public int EmployeeId { get; set; }
    public decimal BaseSalary { get; set; }

    public virtual decimal CalculateSalary()
    {
        return BaseSalary;
    }

    public void ClockIn()
    {
        Console.WriteLine($"{Name} clocked in at {DateTime.Now}");
    }
}

// Derived class inherits and extends base functionality
public class Manager : Employee
{
    public decimal Bonus { get; set; }
    public List<Employee> TeamMembers { get; set; } = new List<Employee>();

    // Override to provide specialized behavior
    public override decimal CalculateSalary()
    {
        return BaseSalary + Bonus;
    }

    // Add new functionality specific to Manager
    public void ConductMeeting()
    {
        Console.WriteLine($"{Name} is conducting a meeting with {TeamMembers.Count} team members");
    }
}

public class Developer : Employee
{
    public string ProgrammingLanguage { get; set; }
    public int LinesOfCodeWritten { get; set; }

    public override decimal CalculateSalary()
    {
        // Specialized salary calculation
        decimal bonus = LinesOfCodeWritten * 0.01m;
        return BaseSalary + bonus;
    }

    public void WriteCode()
    {
        Console.WriteLine($"{Name} is writing {ProgrammingLanguage} code");
    }
}

// Usage - inherited and specific functionality
var manager = new Manager
{
    Name = "Alice",
    BaseSalary = 80000,
    Bonus = 10000
};
manager.ClockIn(); // Inherited method
manager.ConductMeeting(); // Manager-specific method

var dev = new Developer
{
    Name = "Bob",
    BaseSalary = 70000,
    ProgrammingLanguage = "C#"
};
dev.ClockIn(); // Inherited method
dev.WriteCode(); // Developer-specific method
```

---

## How the Four Pillars Work Together

The four pillars are not isolated concepts—they work together to create robust object-oriented systems:

1. **Inheritance** establishes relationships and enables code reuse
2. **Encapsulation** protects the internal state of each class in the hierarchy
3. **Abstraction** defines contracts and hides implementation complexity
4. **Polymorphism** allows objects to be treated uniformly while behaving differently

**Example showing all four pillars:**
```csharp
// Abstraction - defines what a notification service does
public abstract class NotificationService
{
    // Encapsulation - private internal state
    private List<string> _recipients = new List<string>();

    // Encapsulation - controlled access
    protected List<string> Recipients => _recipients;

    public void AddRecipient(string recipient)
    {
        if (!string.IsNullOrWhiteSpace(recipient))
            _recipients.Add(recipient);
    }

    // Abstraction - abstract method to be implemented by derived classes
    public abstract bool Send(string message);
}

// Inheritance - EmailService inherits from NotificationService
public class EmailService : NotificationService
{
    // Override provides specific implementation (Polymorphism)
    public override bool Send(string message)
    {
        foreach (var recipient in Recipients)
        {
            Console.WriteLine($"Sending email to {recipient}: {message}");
        }
        return true;
    }
}

// Inheritance - SmsService inherits from NotificationService
public class SmsService : NotificationService
{
    // Different implementation of the same method (Polymorphism)
    public override bool Send(string message)
    {
        foreach (var recipient in Recipients)
        {
            Console.WriteLine($"Sending SMS to {recipient}: {message}");
        }
        return true;
    }
}

// Polymorphism in action
NotificationService service = GetNotificationService();
service.AddRecipient("user@example.com");
service.Send("Hello!"); // Behavior depends on actual type at runtime
```

---

## Summary

| Pillar | Core Idea | Main Benefit |
|--------|-----------|-------------|
| **Polymorphism** | One interface, multiple implementations | Flexibility and extensibility |
| **Encapsulation** | Hide internal state, expose through methods | Data protection and maintainability |
| **Abstraction** | Hide complexity, show only essentials | Simplicity and reduced coupling |
| **Inheritance** | Reuse code through hierarchical relationships | Code reuse and clear structure |

Understanding and applying these four pillars is essential for writing clean, maintainable, and scalable object-oriented code.
