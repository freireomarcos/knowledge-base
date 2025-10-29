// ===== OBJECT-ORIENTED PROGRAMMING (OOP) INTERVIEW QUESTIONS =====

// Question 1: What are the four pillars of OOP?
// Answer: The four pillars are Encapsulation, Inheritance, Polymorphism, and Abstraction.
// They are fundamental principles that help organize and structure code in an object-oriented way.


// Question 2: Can you give a real example of polymorphism in C#?
// Answer: Sure. Let's say we have a base class Shape with a method Draw().
// Different classes like Circle and Rectangle override the method to draw differently.
// When we call shape.Draw(), the correct implementation runs at runtime — that's polymorphism.

public abstract class Shape
{
    public abstract void Draw();
}

public class Circle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a circle");
    }
}

public class Rectangle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a rectangle");
    }
}

// Usage:
// Shape shape = new Circle();
// shape.Draw(); // Output: Drawing a circle


// Question 3: What's the difference between method overloading and method overriding?
// Answer: Overloading is having multiple methods with the same name but different parameters (compile-time polymorphism).
// Overriding is redefining a base class method in a derived class (runtime polymorphism).

public class Calculator
{
    // Method overloading
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
    public int Add(int a, int b, int c) => a + b + c;
}

public class Animal
{
    public virtual void MakeSound()
    {
        Console.WriteLine("Some generic sound");
    }
}

public class Dog : Animal
{
    // Method overriding
    public override void MakeSound()
    {
        Console.WriteLine("Bark!");
    }
}


// Question 4: What is the difference between abstract classes and interfaces?
// Answer: Abstract classes can have implementation, fields, and constructors. They support single inheritance.
// Interfaces only define contracts (methods, properties) with no implementation (before C# 8.0). A class can implement multiple interfaces.

public abstract class Vehicle
{
    public string Brand { get; set; }

    public Vehicle(string brand)
    {
        Brand = brand;
    }

    public abstract void Start();

    public void Stop()
    {
        Console.WriteLine("Vehicle stopped");
    }
}

public interface IFlyable
{
    void Fly();
}

public interface ISwimmable
{
    void Swim();
}

public class Duck : IFlyable, ISwimmable
{
    public void Fly() => Console.WriteLine("Duck is flying");
    public void Swim() => Console.WriteLine("Duck is swimming");
}


// Question 5: What is encapsulation and why is it important?
// Answer: Encapsulation is hiding internal state and requiring all interaction through methods/properties.
// It's important because it protects data integrity, reduces coupling, and makes code easier to maintain.

public class BankAccount
{
    private decimal _balance; // Private field - encapsulated

    public decimal Balance
    {
        get => _balance;
        private set => _balance = value; // Setter is private
    }

    public void Deposit(decimal amount)
    {
        if (amount > 0)
            _balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount > 0 && amount <= _balance)
        {
            _balance -= amount;
            return true;
        }
        return false;
    }
}


// Question 6: What is the difference between 'virtual' and 'abstract' methods?
// Answer: Virtual methods have a default implementation that can be overridden.
// Abstract methods have no implementation and must be overridden in derived classes.

public class BaseClass
{
    public virtual void VirtualMethod()
    {
        Console.WriteLine("Base implementation");
    }

    public abstract void AbstractMethod(); // Error: only abstract classes can have abstract methods
}

public abstract class CorrectBaseClass
{
    public virtual void VirtualMethod()
    {
        Console.WriteLine("Base implementation - can be overridden or used as-is");
    }

    public abstract void AbstractMethod(); // Must be implemented by derived class
}


// Question 7: Can you explain the 'sealed' keyword?
// Answer: The 'sealed' keyword prevents a class from being inherited or a method from being overridden.
// It's used when you want to prevent further modification in the inheritance chain.

public sealed class FinalClass
{
    // This class cannot be inherited
}

public class BaseWithSealed
{
    public virtual void DoSomething()
    {
        Console.WriteLine("Base implementation");
    }
}

public class Derived : BaseWithSealed
{
    public sealed override void DoSomething()
    {
        Console.WriteLine("Derived implementation - cannot be overridden further");
    }
}


// Question 8: What is composition over inheritance?
// Answer: It's a design principle that suggests using object composition (has-a relationship)
// instead of inheritance (is-a relationship) to achieve code reuse and flexibility.

// Inheritance approach (tightly coupled)
public class Car : Engine
{
    // Car IS-AN Engine - doesn't make sense
}

// Composition approach (loosely coupled)
public class Engine
{
    public void Start() => Console.WriteLine("Engine started");
}

public class BetterCar
{
    private Engine _engine; // Car HAS-AN Engine

    public BetterCar()
    {
        _engine = new Engine();
    }

    public void StartCar()
    {
        _engine.Start();
    }
}


// Question 9: What are access modifiers in C#?
// Answer: Access modifiers control the visibility of classes, methods, and properties.
// The main ones are: public (accessible everywhere), private (only within the class),
// protected (class and derived classes), internal (same assembly), and protected internal.

public class AccessModifierExample
{
    public string PublicField;           // Accessible everywhere
    private string PrivateField;         // Only within this class
    protected string ProtectedField;     // This class and derived classes
    internal string InternalField;       // Same assembly
    protected internal string ProtectedInternalField; // Same assembly or derived classes
}


// Question 10: What is a constructor and what types exist in C#?
// Answer: A constructor is a special method called when an object is created to initialize its state.
// Types include: default constructor, parameterized constructor, copy constructor, static constructor, and private constructor.

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    // Default constructor
    public Person()
    {
        Name = "Unknown";
        Age = 0;
    }

    // Parameterized constructor
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    // Copy constructor
    public Person(Person other)
    {
        Name = other.Name;
        Age = other.Age;
    }

    // Static constructor (called once before any instance is created)
    static Person()
    {
        Console.WriteLine("Static constructor called");
    }
}

// Singleton pattern using private constructor
public class Singleton
{
    private static Singleton _instance;

    private Singleton() { } // Private constructor prevents direct instantiation

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Singleton();
            return _instance;
        }
    }
}
