using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cw5.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next) { _next = next; }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext != null)
            {
                string method = httpContext.Request.Method;
                string path = httpContext.Request.Path;
                string queryString = httpContext.Request.QueryString.ToString();
                string body = "";

                using (StreamReader reader =
                    new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    body = await reader.ReadToEndAsync();
                }


                using StreamWriter write = File.AppendText("requestLog.txt");
                write.WriteLine($"metoda:  {method}");
                write.WriteLine($"scieżka:  {path}");
                write.WriteLine(body);
                write.WriteLine(queryString);


            }

            await _next(httpContext);
        }
    }

}

