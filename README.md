⚠️ The library is not supported anymore. I recommend using [Halifax.Api](https://github.com/andrei-m-code/halifax) instead.

# QkRest Overview / When to use
Micro-library for quick REST API development on ASP.NET Core. It provides exception handling, simple custom authorization features, basic REST API models and Swagger support. It works the best for new projects as you would get standardized API models right from the start, you would use QkRest exceptions in your code and you wouldn't have to redefine default exception handling logic.

# Getting started
You can download the project and reference it in your solution but the recommended way would be to use [NuGet package](https://www.nuget.org/packages/QkRest/):

```
Install-Package QkRest
```

In `Startup` class of your ASP.NET Core application we need to register and configure QkRest dependencies as well as QkRest middleware:

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddQkRest();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    /* Right before MVC */
    app.UseQkRest();
    app.UseMvc();
}
```

# QkRest models

QkRest models are designed based on years of developing different API projects. I recommend to use them for every single API response to make your API fully standardized. Default exception handling uses same API models. Here is how you use QkRest response model:

```
[HttpGet, Route("users")]
public QkResponse<List<string>> GetUsers()
{
    var users = new List<string> { "John Doe", "Lorem Ipsum" };
    return new QkResponse<List<string>>(users);
}
```

The response will look like this:

```
{
    data: [ "John Doe", "Lorem Ipsum" ],
    success: true,
    error: null
}
```

Use `return new QkResponse();` for empty successful result. There is also `new QkResponse(exception)` if exception occurs but typically you want it to be handled in middleware. See next section.

# Exception handling

Currently we have 3 types of exceptions `QkNotFoundException` (HTTP 404), `QkUnauthorizedException` (HTTP 401) and `QkException` (HTTP 400). All you have to do is just throw one of these exceptions:

```
[HttpGet, Route("users/{userId}")]
public QkResponse<User> GetUser(int userId)
{
    var user = GetUserById(userId);
    
    if (user == null)
    {
        throw new QkNotFoundException("User is not found.");
    }
    
    return new QkResponse<User>(user);
}
```

Default exception handling middleware will take care of the rest and return the following model:

```
{
    success: false,
    error: {
        type: "QkNotFoundException",
        message: "User is not found.",
        stackTrace: "Exception stack trace goes here..."
    }
}
```

You can use `QkException` for validation purposes or use it as a base class e.g. ` class EmailIsIncorrectException : QkException { ... }`. For non-QkExceptions HTTP status code will be set to 500 internal server error.

If you want to extend or override exception handling, you will need to extend default `QkExceptionHandler` or implement `IQkExceptionHandler` interface. Only one exception handler should be implemented and registered. Once you have it:

```
services.AddQkRest(options => options.ExceptionHandler<MyExceptionHandler>());
```

As an example here is default exception handler implementation:

```
public class QkExceptionHandler : IQkExceptionHandler
{
    public virtual object HandleException(Exception exception, out HttpStatusCode code)
    {
        // this is HTTP code that will be returned.
        code = HttpStatusCode.InternalServerError;

        if (exception is QkNotFoundException) code = HttpStatusCode.NotFound;
        else if (exception is QkUnauthorizedException) code = HttpStatusCode.Unauthorized;
        else if (exception is QkException) code = HttpStatusCode.BadRequest;
        
        // this is resulting error model.
        return new QkResponse(exception);
    }
}
```

# Authorization

QkRest provides simplified way to implement custom authorization logic. To get started you need to implement `IAuthorizationHandler`. And register it in `Startup`:

```
services.AddQkRest(options => options.AuthorizationHandler<MyAuthorizationHandler>());
```
Or for simple scenarios it can be implemented inline, see example:

```
services.AddQkRest(options => options.AuthorizationHandler(context => 
            new GenericPrincipal(new GenericIdentity("John Doe"), new[] {"User"})));
```
So all you need to do is assemble `ClaimsPrincipal` from request context or return `null`. All your API actions become secured, mark it as `[AllowAnonymous]` if you want to open action for everybody. If Web API action is secured and null ClaimsPrincipal returned from handler, QkUnauthorizedException will be thrown. That simple. 

# Swagger

QkRest strives to simplify initial REST application setup and puts all the necessary things under it's hood. We added Swashbuckle package to enable Swagger support out-of-the-box. There are a few important customizations that you might want to have in your project:

```
services.AddQkRest(options => options.ConfigureSwagger("Sample App"));
```

The code above sets application name for Swagger. This method has a few optional parameters. Please check out method signature and overloads and find the one, that works the best for you. ConfigureSwagger methods can be called multiple times. There is an overload that lets you access Swashbuckle options directly. Important to mention, the overload below completely overrides QkRest initial Swashbuckle setup (not really recommended):

```
services.AddQkRest(options => options.OverrideQkSwagger(swashOptions => { ... }));
```
By default QkRest includes all project xml documentations it could find to provide swagger comments to API methods, fields, errors etc. By the way, to enable xml documentation generation for your projects you should go to Project Properties -> Build -> Xml documentation file checkbox set checked. Path like that usually works "bin\\Debug\\netcoreapp1.1\\[PROJECT_NAME].xml" fine. Please note, xml generation for Debug and Release is enabled separately.

One of the desired swagger customizations is to add Authorization header field for APIs that require token to be passed in header for methods/controllers not marked as [AllowAnonymous]. Especially for this specific but very common use-case we have extension method:

```
services.AddQkRest(options => options.EnableSwaggerAuthorizationTokenField());
```

If you wish to disable QkRest swagger (maybe you want to configure Swashbuckle directly or don't want swagger at all):

```
services.AddQkRest(options => options.DisableSwagger());
```

And finally, if you don't want swagger UI or you need to change swagger JSON or swagger UI URLs, you can suppress Swashbuckle registration in UseQkRest and call it's methods directly:

```
app.UseQkRest(suppressUseSwagger: true);
app.UseSwagger(...);
app.UseSwaggerUi(...);
```
