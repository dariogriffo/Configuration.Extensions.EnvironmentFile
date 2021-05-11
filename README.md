[![NuGet](https://img.shields.io/nuget/v/Configuration.Extensions.EnvironmentFile.svg?style=flat)](https://www.nuget.org/packages/Configuration.Extensions.EnvironmentFile/) 
[![GitHub license](https://img.shields.io/github/license/griffo-io/Configuration.Extensions.EnvironmentFile.svg)](https://raw.githubusercontent.com/griffo-io/Configuration.Extensions.EnvironmentFile/master/LICENSE)

[![N|Solid](https://avatars2.githubusercontent.com/u/39886363?s=200&v=4)](https://github.com/griffo-io/Configuration.Extensions.EnvironmentFile)


# Configuration.Extensions.EnvironmentFile

Unix style Environment files to configure .Net core applications

```
ConnectionStrings__Logs=User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;

#Security section -- this line is omitted by the configuration provider
Security__Jwt__Key=q2bflxWAHB4fAHEU
Security__Jwt__ExpirationTime=00:05:00
Security__Jwt__Audience=https://always-use-https.com #This comment will not be loaded as part of the value
```

# Motivation

Having a development environment that resembles as much as possible to production is the best.
In Unix servers, you can configure your background services with an environment file that has the format specified above, but what about in local?
So many options but none matches that, so you can have them now, having a `.env` file (or more) copied with your files on build and loaded in the configuration.

# How to use it

Install the package via Nuget

```
Install-Package Configuration.Extensions.EnvironmentFile
```

or .Net core command line


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

# Default behavior
- The variables are loaded from a file called `.env` that is placed in the same directory as your applications.
- Trimming is performed (usually spaces at the end are mistakes).
- No quotes in values are trimmed (there is no need to add quotes, the library will handle `=` just fine).

```
public static IHostBuilder CreateHostBuilder(string[] args)
{
    Host
        .CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .AddEnvironmentFile(removeWrappingQuotes: true, trim: false)
                .AddEnvironmentVariables();
        })
        .ConfigureWebHostDefaults(webBuilder =>
    	{
            webBuilder.UseStartup<Startup>();
        });
}
```


# Load multiple files

You can have several files also loaded, remember the last file will override the first one (if same variables are present)

```
public static IHostBuilder CreateHostBuilder(string[] args)
{
    Host
        .CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .AddEnvironmentFile() // Configuring from '.env' file
                .AddEnvironmentFile("database-config.env") // Overriding with 'database-config.env'
                .AddEnvironmentVariables();  // Overriding with environment variables
        })
        .ConfigureWebHostDefaults(webBuilder =>
    	{
            webBuilder.UseStartup<Startup>();
        });
}
```


# Variable prefixes

You can specify variable prefixes to be omitted

```
public static IHostBuilder CreateHostBuilder(string[] args)
{
    Host
        .CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .AddEnvironmentFile() // Configuring from '.env' file
                .AddEnvironmentFile("with-prefix.env") // Variables like MyPrefix_MyVariable are loaded as MyPrefix_MyVariable
                .AddEnvironmentFile("with-prefix.env", prefix: "MyPrefix_") // Variables like MyPrefix_MyVariable are loaded as MyVariable
                .AddEnvironmentVariables();  // Overriding with environment variables
        })
        .ConfigureWebHostDefaults(webBuilder =>
    	{
            webBuilder.UseStartup<Startup>();
        });
}
```

Logo Provided by [Vecteezy](https://vecteezy.com)
