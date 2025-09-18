# UriExtensions

Provides URI manipulation and validation utilities.

## Overview

The `UriExtensions` class offers utilities for URI operations including query parameter manipulation, validation, and URI building.

## API Reference

<xref:Plugin.BaseTypeExtensions.UriExtensions>

## Core Methods

### Query Parameter Operations

#### `AddQueryParameter(Uri uri, string name, string value)`
Adds a query parameter to a URI.

```csharp
var baseUri = new Uri("https://api.example.com/users");
var withParam = baseUri.AddQueryParameter("page", "1");
// Result: "https://api.example.com/users?page=1"
```

#### `GetQueryParameter(Uri uri, string name)`
Gets the value of a query parameter.

```csharp
var uri = new Uri("https://example.com?name=John&age=25");
var name = uri.GetQueryParameter("name"); // "John"
var age = uri.GetQueryParameter("age"); // "25"
```

### Validation Operations

#### `IsHttps(Uri uri)`
Checks if a URI uses HTTPS scheme.

```csharp
var httpsUri = new Uri("https://secure.example.com");
var isSecure = httpsUri.IsHttps(); // true

var httpUri = new Uri("http://example.com");
var isSecure2 = httpUri.IsHttps(); // false
```

#### `IsAbsolute(Uri uri)`
Checks if a URI is absolute.

```csharp
var absolute = new Uri("https://example.com/path");
var isAbs = absolute.IsAbsolute(); // true

var relative = new Uri("/path/to/resource", UriKind.Relative);
var isRel = relative.IsAbsolute(); // false
```

## Practical Examples

### API Client Builder

```csharp
public class ApiUriBuilder
{
    private Uri _baseUri;

    public ApiUriBuilder(string baseUrl)
    {
        _baseUri = new Uri(baseUrl);
    }

    public Uri BuildEndpoint(string path, Dictionary<string, string> parameters = null)
    {
        var uri = new Uri(_baseUri, path);

        if (parameters != null)
        {
            foreach (var (name, value) in parameters)
            {
                uri = uri.AddQueryParameter(name, value);
            }
        }

        return uri;
    }

    public bool IsSecureEndpoint(Uri uri)
    {
        return uri.IsHttps();
    }
}
```

### URL Validation

```csharp
public class UrlValidator
{
    public ValidationResult ValidateUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return new ValidationResult("Invalid URL format");
        }

        if (!uri.IsAbsolute())
        {
            return new ValidationResult("URL must be absolute");
        }

        if (!uri.IsHttps())
        {
            return new ValidationResult("URL must use HTTPS");
        }

        return ValidationResult.Success;
    }
}
```

## Best Practices

1. **Validation**: Always validate URIs before manipulation
2. **Encoding**: Properly encode query parameter values
3. **Security**: Prefer HTTPS for sensitive operations
4. **Absolute URIs**: Use absolute URIs for external references
