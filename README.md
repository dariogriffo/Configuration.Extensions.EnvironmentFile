
[![NuGet Info](https://img.shields.io/nuget/dt/Configuration.Extensions.EnvironmentFile)](https://www.nuget.org/packages/Configuration.Extensions.EnvironmentFile/)
![GitHub License](https://img.shields.io/github/license/dariogriffo/Configuration.Extensions.EnvironmentFile)
![CI](https://github.com/dariogriffo/Configuration.Extensions.EnvironmentFile/workflows/CI/badge.svg)


# Configuration.Extensions.EnvironmentFile
**Add support for Unix-style `.env` files in your .NET applications.**

This package lets you use `.env` files as a configuration source. 

```env
ConnectionStrings__Logs=User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;

# Comments are ignored
Security__Jwt__Key=q2bflxWAHB4fAHEU
Security__Jwt__ExpirationTime=00:05:00
Security__Jwt__Audience=https://always-use-https.com
```

# Motivation
This package bridges the gap between development and production environments for 
.NET applications deployed as **Unix system services** on controlled servers.

When deploying .NET applications to Unix/Linux servers as systemd services, 
daemon processes, or other system-level services, the standard approach is to 
use environment files (`.env`) for configuration management. These files are 
referenced directly by service definitions and provide a clean, relatively secure way to 
manage application settings on controlled infrastructure.

However, .NET's built-in configuration providers do not support this Unix 
environment file format, creating a disconnect between your development 
environment and production deployment. This forces developers to use alternative 
configuration approaches during development that don't match the actual 
production setup.

# Security Considerations
- **Never commit `.env` files** - Add them to your `.gitignore` immediately
- **Restrict file permissions** - In production, place `.env` files in secure directories (e.g., `/etc/systemd/system/`) with read access limited to the service user (root)
- **Or level up with secrets management** - For production workloads, consider upgrading to dedicated solutions like HashiCorp Vault (see [VaultSharp](https://github.com/rajanadar/VaultSharp) for .NET integration)

 
# How to use it

Install the package via Nuget

```
Install-Package Configuration.Extensions.EnvironmentFile
```

or .NET command line


```
dotnet add package Configuration.Extensions.EnvironmentFile
```

Then configure your app like:


```
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .AddEnvironmentFile()
                .AddEnvironmentVariables(prefix: "MyCustomPrefix_");
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```

or with the minimal hosting model (.NET 6+): 

```
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentFile();
```


# Default behavior (in development)
- The variables are loaded from a file called `.env` that is placed in the same directory as your applications.
- Trimming is performed (usually spaces at the end are mistakes).
- No quotes in values are trimmed (there is no need to add quotes, the library will handle `=` just fine).

```
config
  .AddEnvironmentFile(removeWrappingQuotes: true, trim: false)
  .AddEnvironmentVariables();
```


# Load multiple files

You can have several files also loaded, remember the last file will override the first one (if same variables are present)

```
config
  .AddEnvironmentFile() // Configuring from '.env' file
  .AddEnvironmentFile("database-config.env") // Overriding with 'database-config.env'
  .AddEnvironmentVariables();  // Overriding with environment variables
```


# Variable prefixes

You can specify variable prefixes to be omitted

```
 config
   .AddEnvironmentFile() // Configuring from '.env' file
   .AddEnvironmentFile("with-prefix.env") // Variables like MyPrefix_MyVariable are loaded as MyPrefix_MyVariable
   .AddEnvironmentFile("with-prefix.env", prefix: "MyPrefix_") // Variables like MyPrefix_MyVariable are loaded as MyVariable
   .AddEnvironmentVariables();  // Overriding with environment variables
```


# Reload configuration on file change

Configuration can automatically update on file changes

```
config
  .AddEnvironmentFile() // Configuring from '.env' file
  .AddEnvironmentFile("reloadable.env", reloadOnChange: true) // This file will be watched for changes
  .AddEnvironmentVariables();  // Overriding with environment variables
```

Logo Provided by [Vecteezy](https://vecteezy.com)
