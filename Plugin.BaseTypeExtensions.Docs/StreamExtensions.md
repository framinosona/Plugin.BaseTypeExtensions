# StreamExtensions

Provides stream operations and utilities.

## Overview

The `StreamExtensions` class offers utilities for stream operations including reading, writing, and stream manipulation.

## API Reference

<xref:Plugin.BaseTypeExtensions.StreamExtensions>

## Core Methods

### Reading Operations

#### `ReadToEnd(Stream stream)`
Reads all content from a stream to a byte array.

```csharp
using var fileStream = File.OpenRead("data.txt");
var content = fileStream.ReadToEnd();
var text = Encoding.UTF8.GetString(content);
```

#### `ReadToString(Stream stream, Encoding encoding = null)`
Reads stream content as a string.

```csharp
using var stream = new MemoryStream(data);
var text = stream.ReadToString(Encoding.UTF8);
```

### Writing Operations

#### `WriteString(Stream stream, string content, Encoding encoding = null)`
Writes a string to a stream.

```csharp
using var stream = new MemoryStream();
stream.WriteString("Hello World", Encoding.UTF8);
```

### Copy Operations

#### `CopyToWithProgress(Stream source, Stream destination, IProgress<long> progress)`
Copies stream with progress reporting.

```csharp
var progress = new Progress<long>(bytesRead =>
    Console.WriteLine($"Copied {bytesRead} bytes"));

using var source = File.OpenRead("large-file.zip");
using var dest = File.Create("copy.zip");
source.CopyToWithProgress(dest, progress);
```

## Best Practices

1. **Disposal**: Always dispose streams properly using `using` statements
2. **Buffer Sizes**: Use appropriate buffer sizes for performance
3. **Progress Reporting**: Implement progress reporting for large operations
4. **Error Handling**: Handle I/O exceptions appropriately
