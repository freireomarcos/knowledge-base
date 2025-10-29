// ===== INHERITANCE INTERVIEW QUESTIONS =====

using System;

// Question 1: What is inheritance and why is it useful?
// Answer: Inheritance allows a class to inherit members from another class, promoting code reuse.
// The derived class (child) inherits from the base class (parent) and can extend or override its behavior.

public class Animal
{
    public string Name { get; set; }

    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }

    public virtual void MakeSound()
    {
        Console.WriteLine("Some generic sound");
    }
}

public class Dog : Animal // Dog inherits from Animal
{
    public string Breed { get; set; }

    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }

    public void Fetch()
    {
        Console.WriteLine($"{Name} is fetching");
    }
}

public class InheritanceExample
{
    public static void Demonstrate()
    {
        var dog = new Dog { Name = "Buddy", Breed = "Golden" };
        dog.Eat(); // Inherited from Animal
        dog.MakeSound(); // Overridden in Dog
        dog.Fetch(); // Defined in Dog
    }
}


// Question 2: What is the difference between 'is-a' and 'has-a' relationships?
// Answer: 'Is-a' represents inheritance (Dog is an Animal). 'Has-a' represents composition (Car has an Engine).
// Favor composition over inheritance when the relationship is not truly hierarchical.

// IS-A relationship (inheritance)
public class Vehicle
{
    public void Start() => Console.WriteLine("Vehicle started");
}

public class Car : Vehicle // Car IS-A Vehicle
{
    public void Drive() => Console.WriteLine("Car driving");
}

// HAS-A relationship (composition)
public class Engine
{
    public void Start() => Console.WriteLine("Engine started");
}

public class BetterCar // Car HAS-AN Engine
{
    private Engine _engine = new Engine();

    public void Start()
    {
        _engine.Start();
    }

    public void Drive()
    {
        Console.WriteLine("Car driving");
    }
}


// Question 3: What is method overriding and what are the rules?
// Answer: Method overriding allows a derived class to provide a specific implementation of a base class method.
// Base method must be virtual, abstract, or override. Derived method must use override keyword. Signatures must match.

public class Shape
{
    // Must be virtual to be overridden
    public virtual double CalculateArea()
    {
        return 0;
    }

    // Non-virtual method cannot be overridden
    public void Display()
    {
        Console.WriteLine("This is a shape");
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }

    // Override base method
    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }

    // This hides, not overrides (gives warning)
    public new void Display()
    {
        Console.WriteLine("This is a circle");
    }
}


// Question 4: What is the 'base' keyword used for?
// Answer: The 'base' keyword accesses members of the base class from a derived class.
// It's commonly used to call base constructors or invoke base method implementations.

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public virtual void Introduce()
    {
        Console.WriteLine($"Hi, I'm {Name}");
    }
}

public class Employee : Person
{
    public string JobTitle { get; set; }

    // Call base constructor using 'base'
    public Employee(string name, int age, string jobTitle)
        : base(name, age) // Calls Person constructor
    {
        JobTitle = jobTitle;
    }

    public override void Introduce()
    {
        base.Introduce(); // Call base implementation
        Console.WriteLine($"I work as a {JobTitle}");
    }
}


// Question 5: What is the difference between 'new' and 'override' keywords?
// Answer: 'override' replaces the base method implementation (polymorphic behavior). 'new' hides the base method
// (non-polymorphic). With override, the derived method is called even through base reference. With new, it depends on the reference type.

public class BaseClass
{
    public virtual void Method1()
    {
        Console.WriteLine("Base Method1");
    }

    public virtual void Method2()
    {
        Console.WriteLine("Base Method2");
    }
}

public class DerivedClass : BaseClass
{
    // Override - polymorphic behavior
    public override void Method1()
    {
        Console.WriteLine("Derived Method1");
    }

    // New - hides base method (non-polymorphic)
    public new void Method2()
    {
        Console.WriteLine("Derived Method2");
    }
}

public class NewVsOverrideExample
{
    public static void Demonstrate()
    {
        DerivedClass derived = new DerivedClass();
        BaseClass baseRef = derived; // Base reference to derived object

        // Override - calls derived version
        derived.Method1(); // Output: Derived Method1
        baseRef.Method1(); // Output: Derived Method1 (polymorphic)

        // New - depends on reference type
        derived.Method2(); // Output: Derived Method2
        baseRef.Method2(); // Output: Base Method2 (not polymorphic!)
    }
}


// Question 6: Can constructors be inherited?
// Answer: No, constructors are not inherited. However, derived class constructors must call a base class constructor
// (implicitly calls parameterless constructor or explicitly using 'base').

public class BaseWithConstructor
{
    public string Name { get; set; }

    public BaseWithConstructor(string name)
    {
        Name = name;
        Console.WriteLine("Base constructor called");
    }
}

public class DerivedWithConstructor : BaseWithConstructor
{
    public int Id { get; set; }

    // Must explicitly call base constructor if it has parameters
    public DerivedWithConstructor(string name, int id)
        : base(name) // Explicitly call base constructor
    {
        Id = id;
        Console.WriteLine("Derived constructor called");
    }
}

// Constructor execution order: Base -> Derived


// Question 7: What is multilevel inheritance?
// Answer: Multilevel inheritance is when a class derives from a derived class, creating a chain.
// C# supports multilevel inheritance but not multiple inheritance (inheriting from multiple classes).

public class LivingThing
{
    public void Breathe()
    {
        Console.WriteLine("Breathing");
    }
}

public class Mammal : LivingThing // Mammal inherits from LivingThing
{
    public void GiveBirth()
    {
        Console.WriteLine("Giving birth");
    }
}

public class Human : Mammal // Human inherits from Mammal (multilevel)
{
    public void Think()
    {
        Console.WriteLine("Thinking");
    }
}

public class MultilevelExample
{
    public static void Demonstrate()
    {
        var human = new Human();
        human.Breathe(); // From LivingThing
        human.GiveBirth(); // From Mammal
        human.Think(); // From Human
    }
}


// Question 8: Why doesn't C# support multiple inheritance of classes?
// Answer: C# doesn't support multiple inheritance to avoid the "diamond problem" (ambiguity when two base classes
// have the same member). Instead, C# allows implementing multiple interfaces.

// This is NOT allowed in C#:
// public class Child : Parent1, Parent2 { } // Compile error

// This IS allowed:
public interface IFlyable
{
    void Fly();
}

public interface ISwimmable
{
    void Swim();
}

public class Duck : Animal, IFlyable, ISwimmable // One class, multiple interfaces
{
    public void Fly() => Console.WriteLine("Duck flying");
    public void Swim() => Console.WriteLine("Duck swimming");
}


// Question 9: What is the 'protected' access modifier?
// Answer: 'protected' makes members accessible within the class and its derived classes, but not from outside.
// It's useful when you want derived classes to access members but keep them hidden from other code.

public class BankAccount
{
    private decimal _balance; // Only this class
    protected decimal _fees; // This class and derived classes
    public string AccountNumber { get; set; } // Everyone

    protected void CalculateFees()
    {
        // Protected method - accessible to derived classes
        _fees = _balance * 0.01m;
    }
}

public class SavingsAccount : BankAccount
{
    public void DisplayInfo()
    {
        // Can access protected members
        CalculateFees();
        Console.WriteLine($"Fees: {_fees}");

        // Cannot access private members
        // Console.WriteLine(_balance); // Error
    }
}


// Question 10: What is the 'sealed' keyword and when would you use it with inheritance?
// Answer: The 'sealed' keyword prevents a class from being inherited or a method from being overridden further.
// Use it when you want to prevent modification in the inheritance chain for security or design reasons.

// Sealed class - cannot be inherited
public sealed class FinalClass
{
    public void DoSomething()
    {
        Console.WriteLine("Cannot inherit from this class");
    }
}

// This would cause a compile error:
// public class CannotDeriveFromSealed : FinalClass { }

public class Level1
{
    public virtual void Method()
    {
        Console.WriteLine("Level 1");
    }
}

public class Level2 : Level1
{
    // Sealed override - can override here but prevent further overriding
    public sealed override void Method()
    {
        Console.WriteLine("Level 2 - final implementation");
    }
}

public class Level3 : Level2
{
    // This would cause a compile error:
    // public override void Method() { } // Cannot override sealed method
}

// Use cases for sealed:
// 1. Performance optimization (allows some compiler optimizations)
// 2. Security (prevent malicious derived classes)
// 3. Design (indicate that class is complete and shouldn't be extended)
public sealed class SecurityHelper
{
    public static void ValidateUser() { }
    // Sealed to prevent tampering
}
