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
```

# Motivation

Having a development environment that resembles as much as possible to production is the best.
In Unix servers, you can configure your background services with an environment file that has the format specified above, but what about in local?
So many options but none matches that, so you can have them now, having a `.env` file (or more) copied with your files on build and loaded in the configuration.

# How to use it

Install the package via Nuget

```
Install-Package MediatR

```

or .Net core command line


```
dotnet add package MediatR

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

# Options

By default the variables are loaded from a file called `.env` that is placed in the same directory as your applications.
You can have several files also loaded

```
public static IHostBuilder CreateHostBuilder(string[] args)
{
    Host
        .CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .AddEnvironmentFile() // This loads configuration from '.env' file
                .AddEnvironmentFile("database-config.env") // This loads configuration from 'database-config.env' file
                .AddEnvironmentVariables();
        })
        .ConfigureWebHostDefaults(webBuilder =>
    	{
            webBuilder.UseStartup<Startup>();
        });
}
```

