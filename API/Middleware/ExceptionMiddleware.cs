using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _inv;
        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment inv)
        {
            _inv = inv;//to see if the application is running in development mode tot production mode
            _logger = logger; //to log exception into the terminal
            _next = next; // whats going to happen after this part..what is the next middleware?
            
        }
         //this method decides what is going to happen next
        public async Task InvokeAsync(HttpContext context) //gets access to the http request //This methods defines what is going to happen next
        {
            //handling the exception
            try{
                await _next(context); //if there is any excpetion, this middleware will catch the exception
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message); //log the error in the terminal
                context.Response.ContentType = "application/json"; //returning the type to the client as application/json //since this is not a API controller it needs to be specified
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //gives a status code of 500

                var response = _inv.IsDevelopment() //responses when the application is in development mode or production mode.
                 ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) //development mode
                 :  new ApiException(context.Response.StatusCode, ex.Message, "Internal server error"); //production mode

                 var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; //options for Json//since this is not a API controller it needs to be specified
                 var json = JsonSerializer.Serialize(response,options); //json response

                 await context.Response.WriteAsync(json);//return a http response on what we have done above
            }
        }
    }
}