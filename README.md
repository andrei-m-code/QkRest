# QkRest Overview / When to use
Microframework for quick REST API development on ASP.NET Core. It provides exception handling, simple custom authorization features and basic REST API models. It works the best for new projects as you would get standardized API models right from the start, you would use QkRest exceptions in your code and you wouldn't have to redefine default exception handling logic.

# Getting started
You can download the project and reference it in your solution but the recommended way would be to use NuGet package:

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
    error: null
}
```

Use `return new QkResponse();` for empty successful result. There is also `new QkResponse(exception)` if exception occurs but typically you want it to be handled in middleware. See next section.

# Exception handling

Currently we have 3 types of exceptions QkRestNotFoundException (HTTP 404), QkRestUnauthorizedException (HTTP 401) and QkRestException (HTTP 400). All you have to do is just throw one of these exceptions:

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
    error: {
        type: "QkNotFoundException",
        message: "User is not found.",
        stackTrace: "Exception stack trace goes here..."
    }
}
```

You can use `QkException` for validation purposes or use it as a base class: `EmailIsIncorrectException : QkException`. For non-QkExceptions HTTP status code will be set to 500 internal server error.

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
