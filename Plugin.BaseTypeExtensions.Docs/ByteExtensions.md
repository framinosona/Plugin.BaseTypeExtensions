# ByteExtensions

Provides byte array operations and conversions.

## Overview

The `ByteExtensions` class offers utilities for byte array manipulation, including conversions, encoding operations, and byte-level operations.

## API Reference

<xref:Plugin.BaseTypeExtensions.ByteExtensions>

## Core Methods

### Conversion Operations

#### `ToHexString(byte[] bytes)`
Converts byte array to hexadecimal string representation.

```csharp
var bytes = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
var hex = bytes.ToHexString(); // "48656C6C6F"
```

#### `FromHexString(string hexString)`
Converts hexadecimal string to byte array.

```csharp
var hex = "48656C6C6F";
var bytes = hex.FromHexString(); // [0x48, 0x65, 0x6C, 0x6C, 0x6F]
var text = Encoding.UTF8.GetString(bytes); // "Hello"
```

### Encoding Operations

#### `ToBase64(byte[] bytes)`
Converts byte array to Base64 string.

```csharp
var bytes = Encoding.UTF8.GetBytes("Hello World");
var base64 = bytes.ToBase64(); // "SGVsbG8gV29ybGQ="
```

#### `FromBase64(string base64String)`
Converts Base64 string to byte array.

```csharp
var base64 = "SGVsbG8gV29ybGQ=";
var bytes = base64.FromBase64();
var text = Encoding.UTF8.GetString(bytes); // "Hello World"
```

## Advanced Operations

### Byte Manipulation

#### `XorWith(byte[] bytes, byte[] key)`
Performs XOR operation with a key.

```csharp
var data = Encoding.UTF8.GetBytes("Secret");
var key = new byte[] { 0xAB, 0xCD, 0xEF };
var encrypted = data.XorWith(key);
var decrypted = encrypted.XorWith(key); // Back to original
```

### Analysis Operations

#### `CalculateChecksum(byte[] bytes)`
Calculates a simple checksum of the byte array.

```csharp
var data = Encoding.UTF8.GetBytes("Hello");
var checksum = data.CalculateChecksum(); // Simple sum of all bytes
```

## Best Practices

1. **Memory Management**: Be mindful of large byte arrays and memory usage
2. **Encoding Consistency**: Always specify encoding when converting strings
3. **Security**: Use proper cryptographic functions for sensitive operations
4. **Performance**: Consider streaming for large data operations
