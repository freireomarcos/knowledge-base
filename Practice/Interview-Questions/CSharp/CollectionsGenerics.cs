// ===== COLLECTIONS AND GENERICS INTERVIEW QUESTIONS =====

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Question 1: What is the difference between Array, ArrayList, and List<T>?
// Answer: Array has fixed size and is type-safe. ArrayList is dynamic but not type-safe (stores objects, requires casting).
// List<T> is dynamic, type-safe, and generally preferred for most scenarios.

public class ArrayVsListExample
{
    public static void Demonstrate()
    {
        // Array - fixed size, type-safe
        int[] array = new int[3] { 1, 2, 3 };
        // array[3] = 4; // Error: index out of bounds

        // ArrayList - dynamic size, NOT type-safe
        ArrayList arrayList = new ArrayList();
        arrayList.Add(1);
        arrayList.Add("string"); // Can mix types - not ideal
        int value = (int)arrayList[0]; // Requires casting

        // List<T> - dynamic size, type-safe (preferred)
        List<int> list = new List<int>();
        list.Add(1);
        list.Add(2);
        // list.Add("string"); // Compile error - type-safe
        int val = list[0]; // No casting needed
    }
}


// Question 2: What is the difference between List and LinkedList?
// Answer: List uses an array internally (fast random access, slow insertions/deletions in middle).
// LinkedList uses nodes with pointers (slow random access, fast insertions/deletions anywhere).

public class ListVsLinkedListExample
{
    public static void Demonstrate()
    {
        // List<T> - backed by array
        List<int> list = new List<int> { 1, 2, 3, 4, 5 };
        var item = list[2]; // O(1) - fast random access
        list.Insert(2, 99); // O(n) - slow, needs to shift elements

        // LinkedList<T> - backed by nodes
        LinkedList<int> linkedList = new LinkedList<int>();
        linkedList.AddLast(1);
        linkedList.AddLast(2);
        linkedList.AddLast(3);
        // No index access - need to traverse
        var node = linkedList.First; // Start from first
        linkedList.AddAfter(node, 99); // O(1) - fast insertion if you have the node
    }
}


// Question 3: What is the difference between Dictionary, Hashtable, and ConcurrentDictionary?
// Answer: Dictionary<K,V> is generic, type-safe, and not thread-safe. Hashtable is non-generic, not type-safe, and thread-safe for reads.
// ConcurrentDictionary is generic, type-safe, and designed for concurrent access.

public class DictionaryExample
{
    public static void Demonstrate()
    {
        // Dictionary<K,V> - preferred, not thread-safe
        Dictionary<string, int> dict = new Dictionary<string, int>();
        dict["one"] = 1;
        dict["two"] = 2;
        int value = dict["one"]; // No casting

        // Hashtable - old, not recommended
        Hashtable hashtable = new Hashtable();
        hashtable["one"] = 1;
        hashtable["two"] = "2"; // Can mix types
        int val = (int)hashtable["one"]; // Requires casting

        // ConcurrentDictionary - for multi-threaded scenarios
        var concurrentDict = new System.Collections.Concurrent.ConcurrentDictionary<string, int>();
        concurrentDict.TryAdd("one", 1);
        concurrentDict.TryUpdate("one", 2, 1); // Thread-safe update
    }
}


// Question 4: What are the differences between IEnumerable, ICollection, and IList?
// Answer: IEnumerable only allows iteration (foreach). ICollection adds Count and Add/Remove operations.
// IList adds index-based access and insertion. Each interface builds on the previous one.

public class InterfaceHierarchyExample
{
    public static void Demonstrate()
    {
        // IEnumerable - only foreach
        IEnumerable<int> enumerable = new List<int> { 1, 2, 3 };
        foreach (var item in enumerable)
        {
            Console.WriteLine(item);
        }
        // No Count, no Add, no indexer

        // ICollection - adds Count, Add, Remove
        ICollection<int> collection = new List<int> { 1, 2, 3 };
        collection.Add(4);
        collection.Remove(1);
        int count = collection.Count;
        // No indexer access

        // IList - adds indexer and Insert
        IList<int> list = new List<int> { 1, 2, 3 };
        int value = list[0]; // Index access
        list.Insert(1, 99);
    }
}


// Question 5: What are Generics and what problems do they solve?
// Answer: Generics allow you to define type-safe classes and methods without specifying the exact type until usage.
// They eliminate boxing/unboxing, provide compile-time type safety, and enable code reuse.

public class GenericsExample
{
    // Without generics (bad)
    public class OldStack
    {
        private object[] items = new object[10];
        private int count = 0;

        public void Push(object item) => items[count++] = item;
        public object Pop() => items[--count]; // Requires casting
    }

    // With generics (good)
    public class Stack<T>
    {
        private T[] items = new T[10];
        private int count = 0;

        public void Push(T item) => items[count++] = item;
        public T Pop() => items[--count]; // No casting needed
    }

    public static void Demonstrate()
    {
        // Old way - requires casting, not type-safe
        OldStack oldStack = new OldStack();
        oldStack.Push(5);
        oldStack.Push("string"); // Mixing types - bad!
        int value = (int)oldStack.Pop(); // Casting required

        // Generic way - type-safe, no casting
        Stack<int> stack = new Stack<int>();
        stack.Push(5);
        // stack.Push("string"); // Compile error - good!
        int val = stack.Pop(); // No casting
    }
}


// Question 6: What are generic constraints and when would you use them?
// Answer: Generic constraints limit the types that can be used with a generic class or method.
// They're used when you need to call specific methods or ensure certain capabilities (like new(), interface implementation).

public class ConstraintsExample
{
    // where T : class - T must be a reference type
    public class ReferenceTypeOnly<T> where T : class
    {
        public T CreateDefault() => default(T); // Returns null
    }

    // where T : struct - T must be a value type
    public class ValueTypeOnly<T> where T : struct
    {
        public T CreateDefault() => default(T); // Returns 0, false, etc.
    }

    // where T : new() - T must have a parameterless constructor
    public class Factory<T> where T : new()
    {
        public T Create()
        {
            return new T(); // Can call new()
        }
    }

    // where T : SomeClass - T must inherit from SomeClass
    public class AnimalProcessor<T> where T : Animal
    {
        public void Process(T animal)
        {
            animal.MakeSound(); // Can call Animal methods
        }
    }

    // where T : IComparable - T must implement IComparable
    public class Sorter<T> where T : IComparable<T>
    {
        public void Sort(List<T> items)
        {
            items.Sort(); // Can use CompareTo
        }
    }

    // Multiple constraints
    public class Repository<T> where T : class, IEntity, new()
    {
        // T must be a class, implement IEntity, and have parameterless constructor
    }
}

public abstract class Animal
{
    public abstract void MakeSound();
}

public interface IEntity
{
    int Id { get; set; }
}


// Question 7: What is the difference between Stack and Queue?
// Answer: Stack is Last-In-First-Out (LIFO) - like a stack of plates. You Push and Pop from the top.
// Queue is First-In-First-Out (FIFO) - like a line at a store. You Enqueue at the back and Dequeue from the front.

public class StackVsQueueExample
{
    public static void Demonstrate()
    {
        // Stack - LIFO
        Stack<int> stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        Console.WriteLine(stack.Pop()); // 3 (last in, first out)
        Console.WriteLine(stack.Pop()); // 2
        Console.WriteLine(stack.Peek()); // 1 (peek doesn't remove)

        // Queue - FIFO
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        Console.WriteLine(queue.Dequeue()); // 1 (first in, first out)
        Console.WriteLine(queue.Dequeue()); // 2
        Console.WriteLine(queue.Peek()); // 3 (peek doesn't remove)
    }
}


// Question 8: What is HashSet and when would you use it?
// Answer: HashSet is an unordered collection of unique elements with O(1) add/remove/lookup.
// Use it when you need fast lookups and don't care about order or duplicates.

public class HashSetExample
{
    public static void Demonstrate()
    {
        // HashSet - no duplicates, fast lookup
        HashSet<int> set = new HashSet<int>();
        set.Add(1);
        set.Add(2);
        set.Add(2); // Ignored - duplicates not allowed
        Console.WriteLine(set.Count); // 2

        // Fast lookup - O(1)
        bool contains = set.Contains(1); // true

        // Set operations
        var set1 = new HashSet<int> { 1, 2, 3 };
        var set2 = new HashSet<int> { 3, 4, 5 };

        set1.UnionWith(set2); // { 1, 2, 3, 4, 5 }
        set1.IntersectWith(set2); // { 3 }
        set1.ExceptWith(set2); // { 1, 2 }

        // Use case: removing duplicates from a list
        var listWithDupes = new List<int> { 1, 2, 2, 3, 3, 3 };
        var unique = new HashSet<int>(listWithDupes); // { 1, 2, 3 }
    }
}


// Question 9: What is the difference between IEnumerable<T> and IQueryable<T>?
// Answer: IEnumerable<T> executes queries in-memory (LINQ to Objects). IQueryable<T> builds expression trees
// that can be translated to other query languages like SQL (LINQ to SQL/EF).

public class EnumerableVsQueryable
{
    public static void Demonstrate()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };

        // IEnumerable - filtering happens in memory
        IEnumerable<int> enumerable = numbers.Where(n => n > 2);
        // Query: load all numbers, then filter in C#

        // IQueryable - can be translated to SQL
        IQueryable<int> queryable = numbers.AsQueryable().Where(n => n > 2);
        // If this was a database context, the Where would become SQL: WHERE n > 2

        // Key difference with Entity Framework:
        // IEnumerable: SELECT * FROM Users; then filter in memory (slow)
        // IQueryable: SELECT * FROM Users WHERE Age > 18; (fast)
    }
}


// Question 10: What is the yield keyword and how does it work?
// Answer: The yield keyword creates an iterator that returns elements one at a time on demand (lazy evaluation).
// It's memory-efficient for large sequences because it doesn't create the entire collection upfront.

public class YieldExample
{
    // Without yield - creates entire list in memory
    public static List<int> GetNumbersEager()
    {
        var list = new List<int>();
        for (int i = 0; i < 1000000; i++)
        {
            list.Add(i); // All stored in memory
        }
        return list;
    }

    // With yield - generates numbers on demand
    public static IEnumerable<int> GetNumbersLazy()
    {
        for (int i = 0; i < 1000000; i++)
        {
            yield return i; // One at a time
        }
    }

    public static void Demonstrate()
    {
        // Eager evaluation - all 1M numbers created immediately
        var eager = GetNumbersEager();
        Console.WriteLine(eager.First()); // Had to create all 1M first

        // Lazy evaluation - only creates what's needed
        var lazy = GetNumbersLazy();
        Console.WriteLine(lazy.First()); // Only generates first number
        Console.WriteLine(lazy.Take(10).Sum()); // Only generates first 10
    }

    // yield break - stops iteration
    public static IEnumerable<int> GetNumbersUntilNegative(List<int> numbers)
    {
        foreach (var num in numbers)
        {
            if (num < 0)
                yield break; // Stop iteration
            yield return num;
        }
    }
}
