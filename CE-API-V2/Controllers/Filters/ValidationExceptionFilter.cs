using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Exceptions;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace CE_API_V2.Controllers.Filters;

public class ValidationExceptionFilter : IExceptionFilter
{
    private readonly IBiomarkersTemplateService _templateService;
    public ValidationExceptionFilter(IBiomarkersTemplateService templateService)
    {
        _templateService = templateService;
    } 
    
    public void OnException(ExceptionContext context)
    {
        
        if (context.Exception is BiomarkersValidationException valEx)
        {
            var template = _templateService.GetTemplate(valEx.UserCulture.Name).GetAwaiter().GetResult();
            foreach (var error in valEx.Errors)
            {
                var property = error.FormattedMessagePlaceholderValues["PropertyName"]?.ToString() ?? string.Empty;
                var propertyTrimmed = property.Replace("{", "")?.Replace("}", "");
                var fieldName = template.LabResults.Concat<BiomarkerBase>(template.MedicalHistory).FirstOrDefault(x => x.Id == propertyTrimmed)?.DisplayName;
                var formattedMessage = error.ErrorMessage.Replace(property, fieldName);
                error.ErrorMessage = formattedMessage;
            }
            context.Result = new BadRequestObjectResult(valEx.Errors);
            context.ExceptionHandled = true;
        }
    }
}