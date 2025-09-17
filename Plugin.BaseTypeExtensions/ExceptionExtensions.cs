using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="Exception"/> analysis and utility functions.
/// </summary>
public static class ExceptionExtensions
{
    #region Full Message Extraction

    /// <summary>
    /// Gets the full exception message including all inner exceptions.
    /// </summary>
    /// <param name="exception">The exception to analyze.</param>
    /// <returns>A string containing the full exception message chain.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    public static string GetFullMessage(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var messages = new List<string>();
        var current = exception;

        while (current != null)
        {
            messages.Add(current.Message);
            current = current.InnerException;
        }

        return string.Join(" --> ", messages);
    }

    #endregion

    #region Stack Trace Utilities

    /// <summary>
    /// Gets the stack trace with system calls filtered out for cleaner debugging.
    /// </summary>
    /// <param name="exception">The exception to analyze.</param>
    /// <returns>A filtered stack trace string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    public static string GetStackTraceWithoutSystemCalls(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (string.IsNullOrEmpty(exception.StackTrace))
        {
            return string.Empty;
        }

        var lines = exception.StackTrace.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var filteredLines = lines.Where(line =>
            !line.Contains("System.", StringComparison.Ordinal) &&
            !line.Contains("Microsoft.", StringComparison.Ordinal) &&
            !line.Contains("mscorlib", StringComparison.Ordinal) &&
            !line.Contains("netstandard", StringComparison.Ordinal)).ToArray();

        return string.Join(Environment.NewLine, filteredLines);
    }

    #endregion

    #region Inner Exception Handling

    /// <summary>
    /// Gets all inner exceptions in the exception chain.
    /// </summary>
    /// <param name="exception">The exception to analyze.</param>
    /// <returns>An enumerable of all inner exceptions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    public static IEnumerable<Exception> GetInnerExceptions(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var current = exception.InnerException;
        while (current != null)
        {
            yield return current;
            current = current.InnerException;
        }
    }

    /// <summary>
    /// Gets the innermost exception in the exception chain.
    /// </summary>
    /// <param name="exception">The exception to analyze.</param>
    /// <returns>The innermost exception, or the original exception if no inner exceptions exist.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    public static Exception GetInnermostException(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var current = exception;
        while (current.InnerException != null)
        {
            current = current.InnerException;
        }

        return current;
    }

    /// <summary>
    /// Finds the first inner exception of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of exception to find.</typeparam>
    /// <param name="exception">The exception to search.</param>
    /// <returns>The first inner exception of the specified type, or null if not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    public static T? GetInnerException<T>(this Exception exception) where T : Exception
    {
        ArgumentNullException.ThrowIfNull(exception);

        return exception.GetInnerExceptions().OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Determines whether the exception chain contains an inner exception of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of exception to check for.</typeparam>
    /// <param name="exception">The exception to check.</param>
    /// <returns>True if an inner exception of the specified type exists; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasInnerException<T>(this Exception exception) where T : Exception
    {
        ArgumentNullException.ThrowIfNull(exception);

        return exception.GetInnerException<T>() != null;
    }

    #endregion

    #region Data Extraction

    /// <summary>
    /// Gets all exception data as a dictionary.
    /// </summary>
    /// <param name="exception">The exception to analyze.</param>
    /// <returns>A dictionary containing all exception data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    public static Dictionary<string, object> GetExceptionData(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var result = new Dictionary<string, object>();

        foreach (var key in exception.Data.Keys)
        {
            if (key != null && exception.Data[key] != null)
            {
                result[key.ToString()!] = exception.Data[key]!;
            }
        }

        return result;
    }

    #endregion

    #region Exception Classification

    /// <summary>
    /// Determines whether the exception is considered fatal and should not be caught.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns>True if the exception is fatal; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    public static bool IsFatal(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return exception is OutOfMemoryException ||
               exception is StackOverflowException ||
               exception is AccessViolationException ||
               exception is AppDomainUnloadedException ||
               exception is BadImageFormatException ||
               exception is InvalidProgramException ||
               exception is ThreadAbortException;
    }

    #endregion

    #region Logging and Formatting

    /// <summary>
    /// Formats the exception for logging purposes with comprehensive information.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <returns>A formatted string suitable for logging.</returns>
    /// <exception cref="ArgumentNullException">Thrown when exception is null.</exception>
    public static string ToLogString(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var sb = new StringBuilder();

        sb.AppendLine(CultureInfo.InvariantCulture, $"Exception Type: {exception.GetType().FullName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"Message: {exception.Message}");

        if (exception.Data.Count > 0)
        {
            sb.AppendLine("Data:");
            foreach (var key in exception.Data.Keys)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"  {key}: {exception.Data[key]}");
            }
        }

        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            sb.AppendLine("Stack Trace:");
            sb.AppendLine(exception.StackTrace);
        }

        var innerExceptions = exception.GetInnerExceptions().ToArray();
        if (innerExceptions.Length > 0)
        {
            sb.AppendLine("Inner Exceptions:");
            for (var i = 0; i < innerExceptions.Length; i++)
            {
                var inner = innerExceptions[i];
                sb.AppendLine(CultureInfo.InvariantCulture, $"  [{i + 1}] {inner.GetType().FullName}: {inner.Message}");
            }
        }

        return sb.ToString();
    }

    #endregion
}
