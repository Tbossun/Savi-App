using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Savings_App.Swagger
{
    internal class CustomDocumentFilter : IDocumentFilter
    {
        public void Apply(
            OpenApiDocument swaggerDoc,
            DocumentFilterContext context)
        {
            swaggerDoc.Info.Title = "Savi Web API";
        }
    }
}
