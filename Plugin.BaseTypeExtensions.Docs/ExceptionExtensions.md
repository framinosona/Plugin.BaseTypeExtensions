# ExceptionExtensions

Provides exception handling and manipulation utilities.

## Overview

The `ExceptionExtensions` class offers utilities for exception operations including stack trace analysis, exception chaining, and detailed error information extraction.

## API Reference

<xref:Plugin.BaseTypeExtensions.ExceptionExtensions>

## Core Methods

### Information Extraction

#### `GetFullMessage(Exception exception)`
Gets the complete error message including inner exceptions.

```csharp
try
{
    // Some operation that throws nested exceptions
}
catch (Exception ex)
{
    var fullMessage = ex.GetFullMessage();
    // Includes all inner exception messages
    logger.LogError(fullMessage);
}
```

#### `GetRootCause(Exception exception)`
Gets the innermost exception (root cause).

```csharp
try
{
    // Operation with multiple wrapped exceptions
}
catch (Exception ex)
{
    var rootCause = ex.GetRootCause();
    Console.WriteLine($"Root cause: {rootCause.GetType().Name}: {rootCause.Message}");
}
```

### Stack Trace Operations

#### `GetDetailedStackTrace(Exception exception)`
Gets an enhanced stack trace with additional context.

```csharp
try
{
    ProcessData();
}
catch (Exception ex)
{
    var detailedTrace = ex.GetDetailedStackTrace();
    // Includes enhanced debugging information
    File.WriteAllText("error.log", detailedTrace);
}
```

### Exception Analysis

#### `IsFatal(Exception exception)`
Determines if an exception represents a fatal error.

```csharp
try
{
    RiskyOperation();
}
catch (Exception ex)
{
    if (ex.IsFatal())
    {
        // Fatal errors like OutOfMemoryException, StackOverflowException
        Environment.FailFast("Fatal error occurred", ex);
    }
    else
    {
        // Handle recoverable exceptions
        logger.LogWarning(ex, "Recoverable error occurred");
    }
}
```

## Practical Examples

### Comprehensive Error Logging

```csharp
public class ErrorLogger
{
    private readonly ILogger _logger;

    public ErrorLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void LogException(Exception exception, string context = null)
    {
        var errorInfo = new
        {
            Context = context,
            ExceptionType = exception.GetType().Name,
            Message = exception.Message,
            FullMessage = exception.GetFullMessage(),
            RootCause = exception.GetRootCause().Message,
            StackTrace = exception.GetDetailedStackTrace(),
            IsFatal = exception.IsFatal(),
            Timestamp = DateTime.UtcNow
        };

        if (exception.IsFatal())
        {
            _logger.LogCritical("Fatal exception: {@ErrorInfo}", errorInfo);
        }
        else
        {
            _logger.LogError("Exception occurred: {@ErrorInfo}", errorInfo);
        }
    }
}
```

### Exception Handling Middleware

```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {FullMessage}", ex.GetFullMessage());

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = new
            {
                message = "An error occurred while processing your request.",
                details = exception.GetRootCause().Message,
                traceId = context.TraceIdentifier
            }
        };

        context.Response.StatusCode = exception switch
        {
            ArgumentException => 400,
            UnauthorizedAccessException => 401,
            FileNotFoundException => 404,
            _ => 500
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
```

### Retry Logic with Exception Analysis

```csharp
public class RetryHandler
{
    public async Task<T> ExecuteWithRetryAsync<T>(
        Func<Task<T>> operation,
        int maxRetries = 3,
        TimeSpan delay = default)
    {
        var currentDelay = delay == default ? TimeSpan.FromSeconds(1) : delay;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (!ex.IsFatal() && attempt < maxRetries)
            {
                var rootCause = ex.GetRootCause();

                // Don't retry certain types of exceptions
                if (rootCause is ArgumentException or UnauthorizedAccessException)
                {
                    throw;
                }

                await Task.Delay(currentDelay);
                currentDelay = TimeSpan.FromMilliseconds(currentDelay.TotalMilliseconds * 2); // Exponential backoff
            }
        }

        // Final attempt without catch
        return await operation();
    }
}
```

## Best Practices

1. **Root Cause Analysis**: Always examine the root cause for meaningful error handling
2. **Fatal Exception Handling**: Detect and handle fatal exceptions appropriately
3. **Detailed Logging**: Include full exception context in logs
4. **Stack Trace Preservation**: Maintain stack trace information when re-throwing
5. **Exception Classification**: Categorize exceptions for appropriate handling strategies
