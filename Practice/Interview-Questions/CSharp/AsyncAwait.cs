// ===== ASYNC/AWAIT INTERVIEW QUESTIONS =====

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

// Question 1: What is async/await and why do we use it?
// Answer: Async/await is a pattern for writing asynchronous code that looks synchronous.
// We use it to perform non-blocking operations (like I/O, network calls) without freezing the UI or wasting threads.

public class AsyncBasics
{
    // Synchronous method - blocks the thread
    public string GetDataSync()
    {
        Thread.Sleep(2000); // Blocks for 2 seconds
        return "Data";
    }

    // Asynchronous method - doesn't block the thread
    public async Task<string> GetDataAsync()
    {
        await Task.Delay(2000); // Doesn't block, frees the thread
        return "Data";
    }

    public async Task Example()
    {
        // The thread is free to do other work during the await
        var data = await GetDataAsync();
        Console.WriteLine(data);
    }
}


// Question 2: What is the difference between Task and Task<T>?
// Answer: Task represents an asynchronous operation that doesn't return a value (like void).
// Task<T> represents an asynchronous operation that returns a value of type T.

public class TaskExample
{
    // Returns Task (no return value)
    public async Task DoWorkAsync()
    {
        await Task.Delay(1000);
        Console.WriteLine("Work completed");
    }

    // Returns Task<int> (returns an integer)
    public async Task<int> CalculateAsync()
    {
        await Task.Delay(1000);
        return 42;
    }

    public async Task Usage()
    {
        await DoWorkAsync(); // No return value
        int result = await CalculateAsync(); // Returns 42
        Console.WriteLine(result);
    }
}


// Question 3: What happens if you don't await an async method?
// Answer: The method starts executing but the caller doesn't wait for it to complete.
// This can lead to unhandled exceptions, race conditions, and the method may not finish before the program ends.

public class NotAwaitingExample
{
    public async Task SaveDataAsync()
    {
        await Task.Delay(1000);
        throw new Exception("Error saving data");
    }

    public void BadExample()
    {
        // Warning: not awaited - exception won't be caught!
        SaveDataAsync(); // Fires and forgets
        Console.WriteLine("This runs immediately");
    }

    public async Task GoodExample()
    {
        try
        {
            await SaveDataAsync(); // Properly awaited
            Console.WriteLine("This runs after completion");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }
    }
}


// Question 4: What is ConfigureAwait(false) and when should you use it?
// Answer: ConfigureAwait(false) tells the await not to capture the synchronization context (doesn't return to the original thread).
// Use it in library code to improve performance. Don't use it in UI code where you need to update UI elements.

public class ConfigureAwaitExample
{
    // Library/API code - use ConfigureAwait(false)
    public async Task<string> FetchDataAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetStringAsync("https://api.example.com")
            .ConfigureAwait(false); // Don't need to return to original context
        return response;
    }

    // UI code - don't use ConfigureAwait(false)
    public async Task UpdateUIAsync()
    {
        var data = await FetchDataAsync(); // Returns to UI thread
        // UpdateLabel(data); // Safe - we're back on the UI thread
    }
}


// Question 5: What is the difference between Task.Run and async/await?
// Answer: Task.Run offloads synchronous CPU-bound work to a thread pool thread.
// Async/await is for I/O-bound operations that don't need a thread (like network calls, file I/O).

public class TaskRunExample
{
    // CPU-bound work - use Task.Run
    public Task<int> CalculatePrimeAsync(int limit)
    {
        return Task.Run(() =>
        {
            // Heavy computation on thread pool
            int count = 0;
            for (int i = 2; i < limit; i++)
            {
                if (IsPrime(i)) count++;
            }
            return count;
        });
    }

    private bool IsPrime(int n)
    {
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    // I/O-bound work - use async/await (no Task.Run needed)
    public async Task<string> ReadFileAsync(string path)
    {
        // Already async, no Task.Run needed
        return await File.ReadAllTextAsync(path);
    }
}


// Question 6: How do you handle multiple async operations in parallel?
// Answer: Use Task.WhenAll to run multiple tasks concurrently and wait for all to complete.
// Use Task.WhenAny to wait for the first one to complete.

public class ParallelAsyncExample
{
    public async Task<string> FetchUrl(string url)
    {
        await Task.Delay(1000); // Simulate network call
        return $"Data from {url}";
    }

    public async Task WhenAllExample()
    {
        // Start all tasks at once (runs in parallel)
        var task1 = FetchUrl("url1");
        var task2 = FetchUrl("url2");
        var task3 = FetchUrl("url3");

        // Wait for all to complete
        var results = await Task.WhenAll(task1, task2, task3);
        // Total time: ~1 second (not 3 seconds)

        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
    }

    public async Task WhenAnyExample()
    {
        var task1 = FetchUrl("url1");
        var task2 = FetchUrl("url2");

        // Wait for the first one to complete
        var completedTask = await Task.WhenAny(task1, task2);
        var result = await completedTask;
        Console.WriteLine($"First result: {result}");
    }
}


// Question 7: What are the common mistakes with async/await?
// Answer: Async void (except for event handlers), not awaiting tasks, blocking on async code with .Result or .Wait(),
// and using async for CPU-bound work without Task.Run.

public class AsyncMistakesExample
{
    // MISTAKE 1: async void (avoid except for event handlers)
    public async void BadMethod() // Can't be awaited, exceptions are lost
    {
        await Task.Delay(1000);
        throw new Exception("Lost exception!"); // Won't be caught!
    }

    public async Task GoodMethod() // Can be awaited
    {
        await Task.Delay(1000);
    }

    // MISTAKE 2: Blocking on async code
    public void DeadlockExample()
    {
        // DON'T DO THIS - can cause deadlock
        // var result = GetDataAsync().Result; // Blocks
        // var result = GetDataAsync().Wait();  // Blocks

        // DO THIS instead
        // var result = await GetDataAsync();
    }

    private async Task<string> GetDataAsync()
    {
        await Task.Delay(1000);
        return "Data";
    }

    // MISTAKE 3: Not awaiting in sequence when needed
    public async Task SequentialMistake()
    {
        // BAD: Running sequentially when could be parallel
        var data1 = await FetchData("url1"); // Waits
        var data2 = await FetchData("url2"); // Then waits again
        // Total: 2 seconds

        // GOOD: Running in parallel when possible
        var task1 = FetchData("url1");
        var task2 = FetchData("url2");
        await Task.WhenAll(task1, task2); // Total: 1 second
    }

    private async Task<string> FetchData(string url)
    {
        await Task.Delay(1000);
        return url;
    }
}


// Question 8: What is a CancellationToken and how do you use it?
// Answer: CancellationToken is a mechanism to cancel async operations gracefully.
// Pass it to async methods and check it periodically to stop work when cancellation is requested.

public class CancellationExample
{
    public async Task ProcessDataAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 100; i++)
        {
            // Check if cancellation is requested
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(100, cancellationToken);
            Console.WriteLine($"Processing item {i}");
        }
    }

    public async Task Usage()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(2)); // Auto-cancel after 2 seconds

        try
        {
            await ProcessDataAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was cancelled");
        }
    }
}


// Question 9: What is the difference between Task.Delay and Thread.Sleep?
// Answer: Task.Delay is asynchronous and doesn't block the thread (returns control to caller).
// Thread.Sleep is synchronous and blocks the current thread (wastes resources).

public class DelayVsSleepExample
{
    // BAD: Blocks the thread
    public void BlockingDelay()
    {
        Thread.Sleep(5000); // Thread is blocked for 5 seconds
        Console.WriteLine("Done");
    }

    // GOOD: Doesn't block the thread
    public async Task NonBlockingDelay()
    {
        await Task.Delay(5000); // Thread is freed during delay
        Console.WriteLine("Done");
    }

    public async Task Comparison()
    {
        var start = DateTime.Now;

        // Thread.Sleep blocks - can't do anything else
        Thread.Sleep(1000);
        Console.WriteLine($"Sleep took: {(DateTime.Now - start).TotalSeconds}s");

        start = DateTime.Now;
        // Task.Delay doesn't block - thread can do other work
        await Task.Delay(1000);
        Console.WriteLine($"Delay took: {(DateTime.Now - start).TotalSeconds}s");
    }
}


// Question 10: How do you handle exceptions in async methods?
// Answer: Use try-catch blocks around await statements. For Task.WhenAll, catch AggregateException to handle multiple failures.
// Unhandled exceptions in unawaited tasks are lost unless you observe them.

public class AsyncExceptionHandlingExample
{
    public async Task<string> FetchDataAsync()
    {
        await Task.Delay(1000);
        throw new InvalidOperationException("Data fetch failed");
    }

    // Basic exception handling
    public async Task BasicHandling()
    {
        try
        {
            var data = await FetchDataAsync();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }
    }

    // Handling exceptions with Task.WhenAll
    public async Task WhenAllExceptionHandling()
    {
        var task1 = FetchDataAsync();
        var task2 = FetchDataAsync();

        try
        {
            await Task.WhenAll(task1, task2);
        }
        catch (Exception ex)
        {
            // Only the first exception is thrown
            Console.WriteLine($"One exception: {ex.Message}");

            // To get all exceptions:
            var allExceptions = Task.WhenAll(task1, task2).Exception?.InnerExceptions;
        }
    }

    // Observing task exceptions
    public async Task ObserveTaskExceptions()
    {
        var task = FetchDataAsync();
        // Not awaiting, but observing the exception
        task.ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Console.WriteLine($"Task failed: {t.Exception?.InnerException?.Message}");
            }
        });

        await Task.Delay(2000); // Give task time to complete
    }
}
