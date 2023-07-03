using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Utility;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace CE_API_V2.Controllers.Filters;

public class ValidationExceptionFilter : IExceptionFilter
{
    private readonly IEnumerable<BiomarkerSchemaDto> template;
    public ValidationExceptionFilter(IBiomarkersTemplateService templateService)
    {
        template = templateService.GetTemplate().GetAwaiter().GetResult();
    } 
        
    
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException valEx)
        {
            foreach (var error in valEx.Errors)
            {
                var property = error.FormattedMessagePlaceholderValues["PropertyName"]?.ToString() ?? string.Empty;
                var propertyTrimmed = property.Replace("{", "")?.Replace("}", "");
                var formattedMessage = error.ErrorMessage.Replace(property, template.FirstOrDefault(x => x.Id == propertyTrimmed)?.Fieldname);
                error.ErrorMessage = formattedMessage;
            }
            context.Result = new BadRequestObjectResult(valEx.Errors);
            context.ExceptionHandled = true;
        }
    }
}