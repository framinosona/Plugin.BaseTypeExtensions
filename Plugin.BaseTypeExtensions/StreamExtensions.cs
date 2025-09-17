using System.Runtime.CompilerServices;
using System.Text;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="Stream"/> operations and utility functions.
/// </summary>
public static class StreamExtensions
{
    #region Conversion Operations

    /// <summary>
    /// Converts the stream to a byte array.
    /// </summary>
    /// <param name="stream">The stream to convert.</param>
    /// <returns>A byte array containing the stream data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
    public static byte[] ToByteArray(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Converts the stream to a byte array asynchronously.
    /// </summary>
    /// <param name="stream">The stream to convert.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing a byte array with the stream data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
        return ms.ToArray();
    }

    /// <summary>
    /// Converts the stream to a string using the specified encoding.
    /// </summary>
    /// <param name="stream">The stream to convert.</param>
    /// <param name="encoding">The encoding to use (default is UTF-8).</param>
    /// <returns>A string containing the stream content.</returns>
    /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
    public static string ToStringContent(this Stream stream, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNull(stream);

        encoding ??= Encoding.UTF8;

        using var reader = new StreamReader(stream, encoding, leaveOpen: true);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Converts the stream to a string asynchronously using the specified encoding.
    /// </summary>
    /// <param name="stream">The stream to convert.</param>
    /// <param name="encoding">The encoding to use (default is UTF-8).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing a string with the stream content.</returns>
    /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static async Task<string> ToStringContentAsync(this Stream stream, Encoding? encoding = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        encoding ??= Encoding.UTF8;

        using var reader = new StreamReader(stream, encoding, leaveOpen: true);
        return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region File Operations

    /// <summary>
    /// Saves the stream content to a file.
    /// </summary>
    /// <param name="stream">The stream to save.</param>
    /// <param name="filePath">The file path where to save the stream.</param>
    /// <exception cref="ArgumentNullException">Thrown when stream or filePath is null.</exception>
    /// <exception cref="ArgumentException">Thrown when filePath is empty or whitespace.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the file is denied.</exception>
    public static void SaveToFile(this Stream stream, string filePath)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(filePath);

        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be empty or whitespace.", nameof(filePath));
        }

        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);
    }

    /// <summary>
    /// Saves the stream content to a file asynchronously.
    /// </summary>
    /// <param name="stream">The stream to save.</param>
    /// <param name="filePath">The file path where to save the stream.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when stream or filePath is null.</exception>
    /// <exception cref="ArgumentException">Thrown when filePath is empty or whitespace.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the file is denied.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static async Task SaveToFileAsync(this Stream stream, string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(filePath);

        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be empty or whitespace.", nameof(filePath));
        }

        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Stream Utilities

    /// <summary>
    /// Converts the stream to a MemoryStream.
    /// </summary>
    /// <param name="stream">The stream to convert.</param>
    /// <returns>A MemoryStream containing the stream data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
    public static MemoryStream ToMemoryStream(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream is MemoryStream memoryStream)
        {
            return new MemoryStream(memoryStream.ToArray());
        }

        var ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0;
        return ms;
    }

    /// <summary>
    /// Resets the stream position to the beginning if possible.
    /// </summary>
    /// <param name="stream">The stream to reset.</param>
    /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
    /// <exception cref="NotSupportedException">Thrown when the stream does not support seeking.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        else
        {
            throw new NotSupportedException("Stream does not support seeking.");
        }
    }

    /// <summary>
    /// Determines whether the stream can be reset to the beginning.
    /// </summary>
    /// <param name="stream">The stream to check.</param>
    /// <returns>True if the stream can be reset; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanReset(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.CanSeek;
    }

    /// <summary>
    /// Gets the length of the stream safely, returning -1 if not available.
    /// </summary>
    /// <param name="stream">The stream to measure.</param>
    /// <returns>The length of the stream, or -1 if not available.</returns>
    /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
    public static long GetLength(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        try
        {
            return stream.Length;
        }
        catch (NotSupportedException)
        {
            return -1;
        }
    }

    #endregion

    #region Progress Operations

    /// <summary>
    /// Copies the stream to another stream with progress reporting.
    /// </summary>
    /// <param name="source">The source stream to copy from.</param>
    /// <param name="destination">The destination stream to copy to.</param>
    /// <param name="progress">An optional progress reporter for tracking copy progress.</param>
    /// <exception cref="ArgumentNullException">Thrown when source or destination is null.</exception>
    public static void CopyToWithProgress(this Stream source, Stream destination, IProgress<long>? progress = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        if (progress == null)
        {
            source.CopyTo(destination);
            return;
        }

        const int bufferSize = 81920; // 80KB buffer
        var buffer = new byte[bufferSize];
        long totalBytesRead = 0;
        int bytesRead;

        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
        {
            destination.Write(buffer, 0, bytesRead);
            totalBytesRead += bytesRead;
            progress.Report(totalBytesRead);
        }
    }

    /// <summary>
    /// Copies the stream to another stream asynchronously with progress reporting.
    /// </summary>
    /// <param name="source">The source stream to copy from.</param>
    /// <param name="destination">The destination stream to copy to.</param>
    /// <param name="progress">An optional progress reporter for tracking copy progress.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when source or destination is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public static async Task CopyToWithProgressAsync(this Stream source, Stream destination, IProgress<long>? progress = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        if (progress == null)
        {
            await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
            return;
        }

        const int bufferSize = 81920; // 80KB buffer
        var buffer = new byte[bufferSize];
        long totalBytesRead = 0;
        int bytesRead;

        while ((bytesRead = await source.ReadAsync(buffer.AsMemory(), cancellationToken).ConfigureAwait(false)) > 0)
        {
            await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
            totalBytesRead += bytesRead;
            progress.Report(totalBytesRead);
        }
    }

    #endregion
}
