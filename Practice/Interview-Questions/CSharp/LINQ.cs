// ===== LINQ (Language Integrated Query) INTERVIEW QUESTIONS =====

using System;
using System.Collections.Generic;
using System.Linq;

// Question 1: What is LINQ and what are its main advantages?
// Answer: LINQ (Language Integrated Query) is a feature that allows querying data from different sources using a consistent syntax.
// Main advantages: type-safe queries, IntelliSense support, readability, and works with multiple data sources (objects, XML, databases).

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public class LINQBasics
{
    public static void Example()
    {
        var students = new List<Student>
        {
            new Student { Id = 1, Name = "John", Age = 20 },
            new Student { Id = 2, Name = "Alice", Age = 22 },
            new Student { Id = 3, Name = "Bob", Age = 19 }
        };

        // LINQ query
        var adults = students.Where(s => s.Age >= 20).OrderBy(s => s.Name);
    }
}


// Question 2: What's the difference between deferred execution and immediate execution in LINQ?
// Answer: Deferred execution means the query is not executed until you iterate over the results.
// Immediate execution happens when you call methods like ToList(), ToArray(), Count(), or First().

public class ExecutionExample
{
    public static void Demonstrate()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };

        // Deferred execution - query is not executed yet
        var deferredQuery = numbers.Where(n => n > 2);
        numbers.Add(6); // This will affect the query results

        // Query executes now during iteration
        foreach (var num in deferredQuery)
        {
            Console.WriteLine(num); // Outputs: 3, 4, 5, 6
        }

        // Immediate execution - query executes immediately
        var immediateQuery = numbers.Where(n => n > 2).ToList();
        numbers.Add(7); // This will NOT affect immediateQuery

        Console.WriteLine(immediateQuery.Count); // Outputs: 4 (not 5)
    }
}


// Question 3: What's the difference between First(), FirstOrDefault(), Single(), and SingleOrDefault()?
// Answer: First() returns the first element and throws if none exist. FirstOrDefault() returns default if none exist.
// Single() returns the only element and throws if there's zero or more than one. SingleOrDefault() returns default if none exist but throws if more than one.

public class QueryMethodsExample
{
    public static void Demonstrate()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var empty = new List<int>();
        var duplicates = new List<int> { 1, 1, 2 };

        // First() - throws InvalidOperationException if empty
        var first = numbers.First(); // Returns: 1
        // var firstEmpty = empty.First(); // Throws exception

        // FirstOrDefault() - returns default(T) if empty
        var firstOrDefault = empty.FirstOrDefault(); // Returns: 0

        // Single() - throws if zero or more than one element
        var single = new List<int> { 42 }.Single(); // Returns: 42
        // var singleDuplicate = duplicates.Single(); // Throws exception

        // SingleOrDefault() - returns default if empty, throws if more than one
        var singleOrDefault = empty.SingleOrDefault(); // Returns: 0
        // var singleOrDefaultDuplicate = duplicates.SingleOrDefault(); // Throws exception
    }
}


// Question 4: What is the difference between Select and SelectMany?
// Answer: Select projects each element to a new form (one-to-one transformation).
// SelectMany flattens nested collections into a single sequence (one-to-many transformation).

public class SelectExample
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<string> Items { get; set; }
    }

    public static void Demonstrate()
    {
        var orders = new List<Order>
        {
            new Order { OrderId = 1, Items = new List<string> { "Apple", "Banana" } },
            new Order { OrderId = 2, Items = new List<string> { "Orange", "Grape" } }
        };

        // Select - returns List<List<string>> (nested)
        var selectResult = orders.Select(o => o.Items);
        // Result: [ ["Apple", "Banana"], ["Orange", "Grape"] ]

        // SelectMany - returns List<string> (flattened)
        var selectManyResult = orders.SelectMany(o => o.Items);
        // Result: ["Apple", "Banana", "Orange", "Grape"]

        foreach (var item in selectManyResult)
        {
            Console.WriteLine(item);
        }
    }
}


// Question 5: What is the difference between Where and Select in LINQ?
// Answer: Where filters elements based on a condition (returns a subset of the original collection).
// Select transforms each element into a new form (returns a projection of the collection).

public class WhereVsSelectExample
{
    public static void Demonstrate()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };

        // Where - filters elements
        var evenNumbers = numbers.Where(n => n % 2 == 0);
        // Result: [2, 4]

        // Select - transforms elements
        var squaredNumbers = numbers.Select(n => n * n);
        // Result: [1, 4, 9, 16, 25]

        // You can combine them
        var squaredEvenNumbers = numbers.Where(n => n % 2 == 0).Select(n => n * n);
        // Result: [4, 16]
    }
}


// Question 6: How do you perform a join operation in LINQ?
// Answer: Use the Join method or join keyword in query syntax to combine two collections based on matching keys.
// It's similar to SQL INNER JOIN.

public class JoinExample
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
    }

    public static void Demonstrate()
    {
        var employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "John", DepartmentId = 1 },
            new Employee { Id = 2, Name = "Alice", DepartmentId = 2 },
            new Employee { Id = 3, Name = "Bob", DepartmentId = 1 }
        };

        var departments = new List<Department>
        {
            new Department { Id = 1, DepartmentName = "IT" },
            new Department { Id = 2, DepartmentName = "HR" }
        };

        // Method syntax
        var result = employees.Join(
            departments,
            emp => emp.DepartmentId,
            dept => dept.Id,
            (emp, dept) => new { emp.Name, dept.DepartmentName }
        );

        // Query syntax
        var queryResult = from emp in employees
                          join dept in departments on emp.DepartmentId equals dept.Id
                          select new { emp.Name, dept.DepartmentName };

        foreach (var item in result)
        {
            Console.WriteLine($"{item.Name} works in {item.DepartmentName}");
        }
    }
}


// Question 7: What is the difference between IEnumerable and IQueryable?
// Answer: IEnumerable is for in-memory collections and uses LINQ-to-Objects (client-side execution).
// IQueryable is for remote data sources like databases and uses LINQ-to-SQL/Entity Framework (server-side execution with expression trees).

public class EnumerableVsQueryableExample
{
    public static void Demonstrate()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };

        // IEnumerable - filtering happens in memory
        IEnumerable<int> enumerable = numbers.Where(n => n > 2);
        // Query executes in C# code

        // IQueryable - filtering can be translated to SQL
        IQueryable<int> queryable = numbers.AsQueryable().Where(n => n > 2);
        // Query can be translated to SQL if used with a database context

        // Key difference: IQueryable allows query composition that gets translated to the data source
        // Example with Entity Framework:
        // IQueryable<User> query = dbContext.Users.Where(u => u.Age > 18); // SQL: WHERE Age > 18
    }
}


// Question 8: What are GroupBy and Aggregate in LINQ?
// Answer: GroupBy groups elements by a key into collections.
// Aggregate performs a custom accumulation operation over a sequence (like Sum, but more flexible).

public class GroupByAggregateExample
{
    public static void Demonstrate()
    {
        var students = new List<Student>
        {
            new Student { Id = 1, Name = "John", Age = 20 },
            new Student { Id = 2, Name = "Alice", Age = 22 },
            new Student { Id = 3, Name = "Bob", Age = 20 }
        };

        // GroupBy
        var groupedByAge = students.GroupBy(s => s.Age);
        foreach (var group in groupedByAge)
        {
            Console.WriteLine($"Age {group.Key}: {group.Count()} students");
        }

        // Aggregate - custom accumulation
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var product = numbers.Aggregate((acc, n) => acc * n);
        // Result: 1 * 2 * 3 * 4 * 5 = 120

        // Aggregate with seed
        var sentence = new List<string> { "Hello", "World", "LINQ" };
        var combined = sentence.Aggregate("Result:", (acc, word) => acc + " " + word);
        // Result: "Result: Hello World LINQ"
    }
}


// Question 9: What is the difference between Any() and All() in LINQ?
// Answer: Any() returns true if at least one element matches the condition.
// All() returns true only if all elements match the condition.

public class AnyAllExample
{
    public static void Demonstrate()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };

        // Any() - checks if at least one element satisfies the condition
        var hasEven = numbers.Any(n => n % 2 == 0); // true (2, 4 are even)
        var hasNegative = numbers.Any(n => n < 0); // false

        // All() - checks if all elements satisfy the condition
        var allPositive = numbers.All(n => n > 0); // true
        var allEven = numbers.All(n => n % 2 == 0); // false

        // Any() without predicate - checks if collection has any elements
        var isEmpty = !numbers.Any(); // false
    }
}


// Question 10: How do you handle null values in LINQ queries?
// Answer: Use null-conditional operators, Where clauses to filter nulls, or DefaultIfEmpty() for left joins.
// You can also use null-coalescing operator (??) to provide default values.

public class NullHandlingExample
{
    public class Person
    {
        public string Name { get; set; }
        public int? Age { get; set; } // Nullable int
    }

    public static void Demonstrate()
    {
        var people = new List<Person>
        {
            new Person { Name = "John", Age = 30 },
            new Person { Name = "Alice", Age = null },
            new Person { Name = null, Age = 25 }
        };

        // Filter out nulls
        var validNames = people.Where(p => p.Name != null).Select(p => p.Name);

        // Use null-conditional operator
        var names = people.Select(p => p.Name?.ToUpper() ?? "Unknown");

        // Handle nullable values
        var averageAge = people.Where(p => p.Age.HasValue).Average(p => p.Age.Value);

        // Or simpler
        var averageAge2 = people.Where(p => p.Age != null).Average(p => p.Age.Value);

        // DefaultIfEmpty for left join scenario
        var result = people.Select(p => p.Name).DefaultIfEmpty("No names found");
    }
}
