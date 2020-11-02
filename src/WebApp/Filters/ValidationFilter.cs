using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.Linq;
using System.Threading.Tasks;

using WebApplicationAPI.Contracts.V1.Responses;

namespace WebApplicationAPI.Filters {
    public class ValidationFilter : IAsyncActionFilter {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            // before controller
            if (!context.ModelState.IsValid) {
                var errorsInModelState = context.ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage))
                    .ToArray();

                var errorResponse = new ErrorResponse();
                foreach (var error in errorsInModelState) {
                    foreach (var subError in error.Value) {
                        var errorModel = new ErrorModel {
                            FieldName = error.Key,
                            Message = subError
                        };
                        errorResponse.Errors.Add(errorModel);
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();

            // after controller
        }
    }
}
