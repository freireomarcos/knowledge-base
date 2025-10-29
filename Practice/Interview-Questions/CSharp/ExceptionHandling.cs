// ===== EXCEPTION HANDLING INTERVIEW QUESTIONS =====

using System;
using System.IO;

// Question 1: What is the difference between Exception and Error?
// Answer: Exceptions are conditions that applications can catch and handle (like FileNotFoundException).
// Errors are serious problems that applications shouldn't try to catch (like OutOfMemoryError, StackOverflowException).

public class ExceptionVsErrorExample
{
    public static void Demonstrate()
    {
        try
        {
            // Exception - can and should be handled
            File.ReadAllText("nonexistent.txt");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("File not found - we can handle this");
        }

        // Error - shouldn't try to catch (let it crash)
        // StackOverflowException - indicates serious problem
        // OutOfMemoryException - system is out of resources
    }
}


// Question 2: What is the difference between throw and throw ex?
// Answer: 'throw' preserves the original stack trace. 'throw ex' resets the stack trace to the current location.
// Always use 'throw' to preserve debugging information.

public class ThrowExample
{
    public static void MethodA()
    {
        try
        {
            MethodB();
        }
        catch (Exception ex)
        {
            // BAD: loses original stack trace
            // throw ex;

            // GOOD: preserves stack trace
            throw;

            // ALSO GOOD: wrapping in new exception
            // throw new ApplicationException("Error in MethodA", ex);
        }
    }

    public static void MethodB()
    {
        throw new InvalidOperationException("Error in MethodB");
    }
}


// Question 3: What is the purpose of the finally block?
// Answer: The finally block executes whether an exception occurs or not. It's used for cleanup
// (closing files, database connections, releasing resources). It runs even if there's a return statement in try/catch.

public class FinallyExample
{
    public static void Demonstrate()
    {
        FileStream file = null;
        try
        {
            file = File.OpenRead("data.txt");
            // Process file
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("File not found");
        }
        finally
        {
            // Always executes - perfect for cleanup
            file?.Dispose();
            Console.WriteLine("Cleanup completed");
        }
    }

    public static int GetValue()
    {
        try
        {
            return 1;
        }
        finally
        {
            // Executes even though there's a return
            Console.WriteLine("Finally executed");
        }
        // Output: "Finally executed" then returns 1
    }
}


// Question 4: What is a custom exception and when should you create one?
// Answer: A custom exception is a class that inherits from Exception. Create one when you need
// to represent a specific error condition in your domain that built-in exceptions don't cover.

// Custom exception
public class InsufficientFundsException : Exception
{
    public decimal Balance { get; }
    public decimal RequestedAmount { get; }

    public InsufficientFundsException(decimal balance, decimal requested)
        : base($"Insufficient funds. Balance: {balance}, Requested: {requested}")
    {
        Balance = balance;
        RequestedAmount = requested;
    }

    // Required constructors for serialization
    public InsufficientFundsException() { }
    public InsufficientFundsException(string message) : base(message) { }
    public InsufficientFundsException(string message, Exception inner) : base(message, inner) { }
}

public class BankAccount
{
    private decimal _balance;

    public void Withdraw(decimal amount)
    {
        if (amount > _balance)
        {
            throw new InsufficientFundsException(_balance, amount);
        }
        _balance -= amount;
    }
}


// Question 5: What is the difference between using and try-finally for resource cleanup?
// Answer: The 'using' statement is syntactic sugar for try-finally with Dispose(). It's cleaner and ensures
// resources are disposed even if exceptions occur. It works with any IDisposable object.

public class UsingExample
{
    // Using statement (preferred)
    public static void WithUsing()
    {
        using (var file = File.OpenRead("data.txt"))
        {
            // Use file
        } // Automatically calls file.Dispose()
    }

    // Equivalent try-finally
    public static void WithTryFinally()
    {
        FileStream file = null;
        try
        {
            file = File.OpenRead("data.txt");
            // Use file
        }
        finally
        {
            file?.Dispose();
        }
    }

    // C# 8.0+ using declaration (even cleaner)
    public static void WithUsingDeclaration()
    {
        using var file = File.OpenRead("data.txt");
        // Use file
        // Automatically disposed at end of scope
    }
}


// Question 6: What are exception filters and when would you use them?
// Answer: Exception filters (when clause) let you catch an exception only if a condition is true.
// They're useful when you want to handle exceptions differently based on their properties.

public class ExceptionFiltersExample
{
    public static void Demonstrate()
    {
        try
        {
            ProcessData();
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("404"))
        {
            Console.WriteLine("Resource not found");
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("500"))
        {
            Console.WriteLine("Server error");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Other HTTP error");
        }
    }

    // Another example with retry logic
    public static void RetryExample()
    {
        int retryCount = 0;
        while (true)
        {
            try
            {
                ProcessData();
                break;
            }
            catch (Exception ex) when (retryCount < 3)
            {
                retryCount++;
                Console.WriteLine($"Retry {retryCount}");
            }
        }
    }

    private static void ProcessData()
    {
        throw new HttpRequestException("404 Not Found");
    }
}

public class HttpRequestException : Exception
{
    public HttpRequestException(string message) : base(message) { }
}


// Question 7: What is the difference between checked and unchecked contexts?
// Answer: In a checked context, arithmetic overflow throws OverflowException. In unchecked context, it wraps around.
// By default, C# uses unchecked context. Use checked when you need to detect overflow.

public class CheckedUncheckedExample
{
    public static void Demonstrate()
    {
        int maxValue = int.MaxValue;

        // Unchecked (default) - wraps around
        int uncheckedResult = maxValue + 1;
        Console.WriteLine(uncheckedResult); // -2147483648 (wrapped)

        try
        {
            // Checked - throws exception on overflow
            int checkedResult = checked(maxValue + 1);
        }
        catch (OverflowException)
        {
            Console.WriteLine("Overflow detected!");
        }

        // Checked block
        checked
        {
            int a = maxValue;
            // int b = a + 1; // Would throw OverflowException
        }

        // Unchecked block (explicit)
        unchecked
        {
            int a = maxValue;
            int b = a + 1; // Wraps around
        }
    }
}


// Question 8: How do you handle multiple exceptions in a catch block?
// Answer: Catch specific exceptions first (most specific to least specific), or use exception filters.
// You can also catch the base Exception, but be careful not to swallow important errors.

public class MultipleExceptionsExample
{
    public static void Demonstrate()
    {
        try
        {
            ProcessFile("data.txt");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("File not found");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Access denied");
        }
        catch (IOException ex) // More general IOException
        {
            Console.WriteLine("IO error");
        }
        catch (Exception ex) // Most general - always last
        {
            Console.WriteLine("Unknown error");
            throw; // Re-throw if you can't handle it
        }
    }

    // C# 7.0+ - Multiple exception types in one catch (not recommended, but possible)
    public static void MultiCatchExample()
    {
        try
        {
            ProcessFile("data.txt");
        }
        catch (Exception ex) when (ex is FileNotFoundException || ex is UnauthorizedAccessException)
        {
            Console.WriteLine("File access issue");
        }
    }

    private static void ProcessFile(string path)
    {
        File.ReadAllText(path);
    }
}


// Question 9: What is AggregateException and when does it occur?
// Answer: AggregateException wraps multiple exceptions, typically from parallel or async operations.
// It occurs with Task.WaitAll, Parallel.ForEach, or when multiple tasks fail.

public class AggregateExceptionExample
{
    public static void Demonstrate()
    {
        try
        {
            var task1 = Task.Run(() => throw new InvalidOperationException("Error 1"));
            var task2 = Task.Run(() => throw new ArgumentException("Error 2"));
            var task3 = Task.Run(() => throw new IOException("Error 3"));

            Task.WaitAll(task1, task2, task3);
        }
        catch (AggregateException ae)
        {
            Console.WriteLine($"Caught {ae.InnerExceptions.Count} exceptions:");
            foreach (var ex in ae.InnerExceptions)
            {
                Console.WriteLine($"- {ex.GetType().Name}: {ex.Message}");
            }

            // Handle specific exception types
            ae.Handle(ex =>
            {
                if (ex is InvalidOperationException)
                {
                    Console.WriteLine("Handled InvalidOperationException");
                    return true; // Exception handled
                }
                return false; // Exception not handled
            });
        }
    }
}


// Question 10: What are best practices for exception handling?
// Answer: Catch specific exceptions, don't catch and ignore (empty catch), clean up resources,
// don't use exceptions for flow control, log exceptions, and only catch what you can handle.

public class BestPracticesExample
{
    // BAD: Empty catch (swallows exceptions)
    public static void BadExample1()
    {
        try
        {
            File.ReadAllText("data.txt");
        }
        catch
        {
            // Silent failure - debugging nightmare
        }
    }

    // BAD: Using exceptions for flow control
    public static bool BadExample2(string input)
    {
        try
        {
            int.Parse(input);
            return true;
        }
        catch
        {
            return false; // Use TryParse instead!
        }
    }

    // GOOD: Specific exception, proper handling
    public static void GoodExample1()
    {
        try
        {
            File.ReadAllText("data.txt");
        }
        catch (FileNotFoundException ex)
        {
            // Log the exception
            Console.WriteLine($"File not found: {ex.Message}");
            // Provide fallback
            UseDefaultData();
        }
    }

    // GOOD: TryParse pattern instead of exceptions
    public static bool GoodExample2(string input, out int result)
    {
        return int.TryParse(input, out result);
    }

    // GOOD: Let exceptions bubble up if you can't handle them
    public static void GoodExample3()
    {
        try
        {
            ProcessCriticalData();
        }
        catch (Exception ex)
        {
            // Log it
            Console.WriteLine($"Error: {ex}");
            // Re-throw if you can't handle it
            throw;
        }
    }

    private static void UseDefaultData() { }
    private static void ProcessCriticalData() { }
}
