// ===== DEPENDENCY INJECTION INTERVIEW QUESTIONS =====

// Question 1: What is Dependency Injection?
// Answer: Dependency Injection is a design pattern where objects receive their dependencies from external sources
// rather than creating them internally. It promotes loose coupling and makes code more testable.

public interface IEmailService
{
    void SendEmail(string message);
}

public class EmailService : IEmailService
{
    public void SendEmail(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

// Without DI (tightly coupled)
public class UserServiceWithoutDI
{
    private EmailService _emailService = new EmailService(); // Bad: creates its own dependency
}

// With DI (loosely coupled)
public class UserService
{
    private readonly IEmailService _emailService;

    public UserService(IEmailService emailService) // Good: receives dependency
    {
        _emailService = emailService;
    }

    public void RegisterUser(string email)
    {
        _emailService.SendEmail($"Welcome {email}");
    }
}


// Question 2: What are the three types of Dependency Injection?
// Answer: Constructor Injection (recommended), Property Injection, and Method Injection.
// Constructor injection is preferred because it ensures dependencies are always provided and promotes immutability.

public class ServiceExamples
{
    private readonly IEmailService _emailService;
    private ILogger _logger;

    // 1. Constructor Injection (most common and recommended)
    public ServiceExamples(IEmailService emailService)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    // 2. Property Injection (used for optional dependencies)
    public ILogger Logger
    {
        get => _logger;
        set => _logger = value;
    }

    // 3. Method Injection (when dependency is only needed for specific method)
    public void ProcessData(IDataProcessor processor)
    {
        processor.Process();
    }
}


// Question 3: What is the difference between service lifetimes (Transient, Scoped, Singleton)?
// Answer: Transient creates a new instance every time it's requested.
// Scoped creates one instance per scope (e.g., per HTTP request).
// Singleton creates one instance for the entire application lifetime.

// In Startup.cs or Program.cs:
// services.AddTransient<IEmailService, EmailService>(); // New instance every time
// services.AddScoped<IOrderService, OrderService>();     // One per request
// services.AddSingleton<IConfiguration, Configuration>(); // One for entire app

public interface IOrderService { }
public class OrderService : IOrderService { }

// Example demonstrating the concept:
public class ServiceLifetimeExample
{
    // Transient: Good for lightweight, stateless services
    // Scoped: Good for services that maintain state during a request (e.g., DbContext)
    // Singleton: Good for services that are expensive to create or need shared state
}


// Question 4: What is an IoC Container?
// Answer: An Inversion of Control (IoC) Container is a framework that manages object creation and dependency injection automatically.
// In .NET, the built-in container is IServiceProvider. It resolves dependencies based on registered services.

using Microsoft.Extensions.DependencyInjection;

public class IoCContainerExample
{
    public static void DemonstrateIoC()
    {
        // Setup the container
        var services = new ServiceCollection();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<UserService>();

        var serviceProvider = services.BuildServiceProvider();

        // Container resolves dependencies automatically
        var userService = serviceProvider.GetService<UserService>();
        userService.RegisterUser("user@example.com");
    }
}


// Question 5: What is the Service Locator pattern and why should you avoid it?
// Answer: Service Locator is an anti-pattern where classes request dependencies from a central registry.
// It should be avoided because it hides dependencies, makes testing harder, and couples code to the service locator.

// Anti-pattern: Service Locator (avoid this)
public class ServiceLocator
{
    private static IServiceProvider _provider;

    public static void Initialize(IServiceProvider provider)
    {
        _provider = provider;
    }

    public static T GetService<T>()
    {
        return _provider.GetService<T>();
    }
}

public class BadUserService
{
    public void DoSomething()
    {
        // Bad: hidden dependency, hard to test
        var emailService = ServiceLocator.GetService<IEmailService>();
        emailService.SendEmail("test");
    }
}

// Better approach: Constructor Injection
public class GoodUserService
{
    private readonly IEmailService _emailService;

    public GoodUserService(IEmailService emailService) // Explicit dependency
    {
        _emailService = emailService;
    }

    public void DoSomething()
    {
        _emailService.SendEmail("test");
    }
}


// Question 6: How do you inject multiple implementations of the same interface?
// Answer: Register all implementations and inject IEnumerable<TInterface> to receive all of them,
// or use named registrations with a factory pattern.

public interface INotificationService
{
    void Notify(string message);
}

public class EmailNotification : INotificationService
{
    public void Notify(string message) => Console.WriteLine($"Email: {message}");
}

public class SmsNotification : INotificationService
{
    public void Notify(string message) => Console.WriteLine($"SMS: {message}");
}

public class NotificationHandler
{
    private readonly IEnumerable<INotificationService> _notificationServices;

    // Inject all implementations
    public NotificationHandler(IEnumerable<INotificationService> notificationServices)
    {
        _notificationServices = notificationServices;
    }

    public void SendAll(string message)
    {
        foreach (var service in _notificationServices)
        {
            service.Notify(message);
        }
    }
}

// Registration:
// services.AddTransient<INotificationService, EmailNotification>();
// services.AddTransient<INotificationService, SmsNotification>();


// Question 7: What is the difference between AddTransient and AddScoped in ASP.NET Core?
// Answer: AddTransient creates a new instance every time it's requested, even within the same HTTP request.
// AddScoped creates one instance per HTTP request and reuses it throughout that request.

public interface IRequestIdService
{
    Guid GetRequestId();
}

public class RequestIdService : IRequestIdService
{
    private readonly Guid _id = Guid.NewGuid();

    public Guid GetRequestId() => _id;
}

// If registered as Scoped:
// services.AddScoped<IRequestIdService, RequestIdService>();
// Multiple calls within the same request will return the same GUID

// If registered as Transient:
// services.AddTransient<IRequestIdService, RequestIdService>();
// Each call will return a different GUID


// Question 8: How do you handle optional dependencies?
// Answer: Use property injection, method parameters with default values, or make the constructor parameter nullable.
// The service should work even if the optional dependency is not provided.

public class ServiceWithOptionalDependency
{
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;

    // Required dependency
    public ServiceWithOptionalDependency(IEmailService emailService, ILogger logger = null)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger; // Optional - can be null
    }

    public void DoWork()
    {
        _logger?.Log("Starting work"); // Safe navigation operator
        _emailService.SendEmail("Work completed");
    }
}


// Question 9: What is the purpose of IServiceScope and when would you use it?
// Answer: IServiceScope creates a new scope for resolving scoped services outside of the default scope.
// It's useful when you need to resolve scoped services in background tasks or singleton services.

public class BackgroundWorker
{
    private readonly IServiceProvider _serviceProvider;

    public BackgroundWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void DoWork()
    {
        // Create a new scope
        using (var scope = _serviceProvider.CreateScope())
        {
            var scopedService = scope.ServiceProvider.GetRequiredService<IOrderService>();
            // Use the scoped service
        } // Scope is disposed here, along with scoped services
    }
}


// Question 10: How does DI improve testability?
// Answer: DI allows you to inject mock or stub implementations during testing instead of real dependencies.
// This isolates the class under test and makes unit testing easier and faster.

// Production code
public interface IDataRepository
{
    string GetData();
}

public class DataService
{
    private readonly IDataRepository _repository;

    public DataService(IDataRepository repository)
    {
        _repository = repository;
    }

    public string ProcessData()
    {
        var data = _repository.GetData();
        return data.ToUpper();
    }
}

// In tests (using a mocking framework like Moq):
// var mockRepository = new Mock<IDataRepository>();
// mockRepository.Setup(r => r.GetData()).Returns("test data");
// var service = new DataService(mockRepository.Object);
// var result = service.ProcessData();
// Assert.Equal("TEST DATA", result);

// Without DI, you couldn't easily replace the repository for testing
