using System.Net;
using System.Text.Json;

namespace E_Commerce.Middleware
{
    //Tratamento global de exceções evita duplicação de try/catch nos Controllers e centraliza a definição das respostas de erro da API.
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next; //RequestDelegate representa o próximo componente do pipeline da aplicação.

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); //Passe a requisição para o próximo componente do pipeline.
            }
            catch (ArgumentException ex) //quando tiver algum dos argumentos invalidos inseridos pelas services.
            {
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (Exception) // tratamento genérico de erro
            {
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode = (int)statusCode,
                message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)); //Serialize - Ele transforma um objeto C# em JSON. 
        }
    }
}
