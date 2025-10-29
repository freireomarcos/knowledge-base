// ===== ENCAPSULATION AND ABSTRACTION INTERVIEW QUESTIONS =====

using System;

// Question 1: What is encapsulation and why is it important?
// Answer: Encapsulation is hiding internal implementation details and exposing only what's necessary through public interfaces.
// It's important because it protects data integrity, reduces coupling, and makes code easier to maintain and change.

public class BankAccount
{
    // Private fields - hidden from outside
    private decimal _balance;
    private string _accountNumber;

    // Public property with controlled access
    public decimal Balance
    {
        get => _balance;
        private set // Only this class can set
        {
            if (value < 0)
                throw new ArgumentException("Balance cannot be negative");
            _balance = value;
        }
    }

    // Public methods provide controlled access
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive");
        Balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance)
            return false;
        Balance -= amount;
        return true;
    }
}


// Question 2: What is abstraction and how is it different from encapsulation?
// Answer: Abstraction hides complexity by showing only essential features (what an object does).
// Encapsulation hides implementation details (how an object does it). Abstraction is about design, encapsulation is about implementation.

// Abstraction - shows WHAT you can do
public abstract class DatabaseConnection
{
    // Abstract - defines WHAT without HOW
    public abstract void Connect();
    public abstract void ExecuteQuery(string query);
    public abstract void Disconnect();
}

// Encapsulation - hides HOW it's done
public class SqlServerConnection : DatabaseConnection
{
    private string _connectionString; // Private - encapsulated
    private bool _isConnected; // Private - encapsulated

    public SqlServerConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override void Connect()
    {
        // Internal implementation hidden
        _isConnected = true;
        Console.WriteLine("Connected to SQL Server");
    }

    public override void ExecuteQuery(string query)
    {
        if (!_isConnected)
            throw new InvalidOperationException("Not connected");
        // Implementation details hidden
        Console.WriteLine($"Executing: {query}");
    }

    public override void Disconnect()
    {
        _isConnected = false;
        Console.WriteLine("Disconnected");
    }
}


// Question 3: What are access modifiers and how do they support encapsulation?
// Answer: Access modifiers control visibility of class members. They support encapsulation by restricting access to internal details.
// Main modifiers: public, private, protected, internal, protected internal, private protected.

public class AccessModifiersExample
{
    // private - only this class
    private int _privateField;

    // protected - this class and derived classes
    protected int _protectedField;

    // internal - same assembly
    internal int _internalField;

    // protected internal - same assembly OR derived classes
    protected internal int _protectedInternalField;

    // private protected - same assembly AND derived classes only (C# 7.2+)
    private protected int _privateProtectedField;

    // public - accessible everywhere
    public int PublicField;

    private void PrivateMethod() { }
    protected void ProtectedMethod() { }
    internal void InternalMethod() { }
    public void PublicMethod() { }
}


// Question 4: What is the difference between fields and properties?
// Answer: Fields are variables that store data directly. Properties are methods that provide controlled access to fields
// with get/set accessors. Properties enable validation, lazy loading, and change notification. Always prefer properties.

public class FieldVsPropertyExample
{
    // Field - direct access (not recommended for public members)
    public string PublicField;

    // Private field with public property (recommended)
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        }
    }

    // Auto-property (compiler creates backing field)
    public int Age { get; set; }

    // Read-only property
    public string FullName => $"{_name} - Age {Age}";

    // Property with private setter
    public DateTime CreatedDate { get; private set; }

    public FieldVsPropertyExample()
    {
        CreatedDate = DateTime.Now;
    }
}


// Question 5: What are automatic properties and when should you use them?
// Answer: Automatic properties (auto-properties) let the compiler generate the backing field automatically.
// Use them when you don't need custom logic in getters/setters. You can add logic later without breaking code.

public class AutoPropertyExample
{
    // Auto-property - simple
    public string Name { get; set; }

    // Auto-property with initialization (C# 6.0+)
    public int Age { get; set; } = 0;

    // Read-only auto-property (C# 6.0+)
    public DateTime CreatedDate { get; } = DateTime.Now;

    // Auto-property with different access levels
    public decimal Balance { get; private set; }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    // Later, you can add logic without breaking callers:
    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            if (!value.Contains("@"))
                throw new ArgumentException("Invalid email");
            _email = value;
        }
    }
}


// Question 6: What is information hiding and why is it important?
// Answer: Information hiding is concealing implementation details that clients don't need to know.
// It's important because it reduces dependencies, allows implementation changes without affecting clients, and improves maintainability.

// BAD: Implementation details exposed
public class BadStack
{
    public int[] Items = new int[100]; // Exposed - clients can modify directly
    public int Count = 0; // Exposed - can be corrupted

    public void Push(int item)
    {
        Items[Count++] = item;
    }
}

// GOOD: Implementation details hidden
public class GoodStack
{
    private int[] _items = new int[100]; // Hidden
    private int _count = 0; // Hidden

    public int Count => _count; // Read-only

    public void Push(int item)
    {
        if (_count >= _items.Length)
            throw new InvalidOperationException("Stack full");
        _items[_count++] = item;
    }

    public int Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Stack empty");
        return _items[--_count];
    }

    // Can change from array to List internally without affecting clients
}


// Question 7: How do you achieve abstraction using abstract classes?
// Answer: Abstract classes define a template with abstract methods (no implementation) and concrete methods (with implementation).
// Derived classes must implement abstract methods. Use abstract classes when you have common implementation to share.

public abstract class PaymentProcessor
{
    // Common implementation for all processors
    public void ProcessPayment(decimal amount)
    {
        ValidateAmount(amount);
        Console.WriteLine("Logging payment...");
        PerformPayment(amount);
        SendConfirmation();
    }

    // Abstract method - each processor implements differently
    protected abstract void PerformPayment(decimal amount);

    // Concrete method - shared by all
    private void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
    }

    // Virtual method - can be overridden if needed
    protected virtual void SendConfirmation()
    {
        Console.WriteLine("Payment confirmation sent");
    }
}

public class CreditCardProcessor : PaymentProcessor
{
    protected override void PerformPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment: {amount}");
    }
}

public class PayPalProcessor : PaymentProcessor
{
    protected override void PerformPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment: {amount}");
    }

    protected override void SendConfirmation()
    {
        Console.WriteLine("PayPal confirmation with receipt");
    }
}


// Question 8: How do you achieve abstraction using interfaces?
// Answer: Interfaces define pure abstraction - only the contract (what), no implementation (how).
// Use interfaces when you need multiple implementations with no shared code, or when you need multiple inheritance.

public interface IRepository<T>
{
    void Add(T item);
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Update(T item);
    void Delete(int id);
}

// SQL implementation
public class SqlRepository<T> : IRepository<T>
{
    public void Add(T item)
    {
        Console.WriteLine("Adding to SQL database");
    }

    public T GetById(int id)
    {
        Console.WriteLine("Getting from SQL database");
        return default(T);
    }

    public IEnumerable<T> GetAll()
    {
        Console.WriteLine("Getting all from SQL database");
        return new List<T>();
    }

    public void Update(T item)
    {
        Console.WriteLine("Updating in SQL database");
    }

    public void Delete(int id)
    {
        Console.WriteLine("Deleting from SQL database");
    }
}

// MongoDB implementation
public class MongoRepository<T> : IRepository<T>
{
    public void Add(T item)
    {
        Console.WriteLine("Adding to MongoDB");
    }

    public T GetById(int id)
    {
        Console.WriteLine("Getting from MongoDB");
        return default(T);
    }

    public IEnumerable<T> GetAll()
    {
        Console.WriteLine("Getting all from MongoDB");
        return new List<T>();
    }

    public void Update(T item)
    {
        Console.WriteLine("Updating in MongoDB");
    }

    public void Delete(int id)
    {
        Console.WriteLine("Deleting from MongoDB");
    }
}


// Question 9: What is the Law of Demeter (Principle of Least Knowledge)?
// Answer: The Law of Demeter states that an object should only talk to its immediate friends, not strangers.
// In practice: don't chain method calls through multiple objects. It reduces coupling and improves encapsulation.

// BAD: Violates Law of Demeter (too much knowledge)
public class Address
{
    public string City { get; set; }
}

public class Customer
{
    public Address Address { get; set; }
}

public class Order
{
    public Customer Customer { get; set; }
}

public class BadOrderProcessor
{
    public void Process(Order order)
    {
        // Chaining through multiple objects - tight coupling
        string city = order.Customer.Address.City;
        Console.WriteLine($"Shipping to {city}");
    }
}

// GOOD: Follows Law of Demeter
public class BetterOrder
{
    private Customer _customer;

    public string GetShippingCity()
    {
        // Order knows how to get shipping city - encapsulated
        return _customer.GetCity();
    }
}

public class BetterCustomer
{
    private Address _address;

    public string GetCity()
    {
        // Customer knows how to get city - encapsulated
        return _address.City;
    }
}

public class GoodOrderProcessor
{
    public void Process(BetterOrder order)
    {
        // Only talks to immediate friend (order)
        string city = order.GetShippingCity();
        Console.WriteLine($"Shipping to {city}");
    }
}


// Question 10: How do encapsulation and abstraction work together?
// Answer: Abstraction defines the "what" (interface/contract), and encapsulation implements the "how" (hiding details).
// Together, they create loosely coupled, maintainable code. Abstraction shows what's possible, encapsulation hides how it works.

// Abstraction - defines WHAT a cache should do
public interface ICache
{
    void Set(string key, object value);
    object Get(string key);
    void Remove(string key);
}

// Encapsulation - hides HOW the cache works
public class InMemoryCache : ICache
{
    // Private implementation details - encapsulated
    private Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>();
    private readonly int _expirationMinutes;

    private class CacheEntry
    {
        public object Value { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public InMemoryCache(int expirationMinutes = 10)
    {
        _expirationMinutes = expirationMinutes;
    }

    public void Set(string key, object value)
    {
        // Implementation hidden
        _cache[key] = new CacheEntry
        {
            Value = value,
            ExpiresAt = DateTime.Now.AddMinutes(_expirationMinutes)
        };
    }

    public object Get(string key)
    {
        // Implementation hidden
        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt > DateTime.Now)
                return entry.Value;
            _cache.Remove(key); // Auto-cleanup
        }
        return null;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}

// Client code depends on abstraction, doesn't know implementation details
public class DataService
{
    private readonly ICache _cache;

    public DataService(ICache cache)
    {
        _cache = cache; // Depends on abstraction
    }

    public string GetData(string key)
    {
        var cached = _cache.Get(key); // Uses interface - doesn't know HOW
        if (cached != null)
            return cached.ToString();

        var data = FetchFromDatabase(key);
        _cache.Set(key, data);
        return data;
    }

    private string FetchFromDatabase(string key)
    {
        return "Data from database";
    }
}
