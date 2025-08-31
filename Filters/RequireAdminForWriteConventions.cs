using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace EasyDine.Filters;

// Automatically requires Admin role for all write endpoints (POST/PUT/PATCH/DELETE),
// unless [AllowAnonymous] is present or [Authorize]/[AllowAnonymous] already applied.

public class RequireAdminForWritesConvention : IApplicationModelConvention
{
    private static readonly string[] WriteVerbs = { "POST", "PUT", "PATCH", "DELETE" };

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var action in controller.Actions)
            {
                // Skip if action or controller explicitly allows anonymous
                if (HasAttribute<AllowAnonymousAttribute>(controller) || HasAttribute<AllowAnonymousAttribute>(action))
                    continue;

                // Skip if action or controller already has any [Authorize] (you can still decorate manually)
                if (HasAttribute<AuthorizeAttribute>(controller) || HasAttribute<AuthorizeAttribute>(action))
                    continue;

                // Is this a write endpoint?
                var isWrite = action.Selectors
                    .SelectMany(s => s.ActionConstraints ?? Enumerable.Empty<IActionConstraintMetadata>())
                    .OfType<HttpMethodActionConstraint>()
                    .Any(c => c.HttpMethods.Any(m => WriteVerbs.Contains(m)));

                if (!isWrite) continue;

                // Add [Authorize(Roles="Admin")] as a filter
                action.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireRole("Admin")
                    .Build()));
            }
        }
    }

    private static bool HasAttribute<T>(ControllerModel c) where T : Attribute =>
        c.Attributes.OfType<T>().Any();

    private static bool HasAttribute<T>(ActionModel a) where T : Attribute =>
        a.Attributes.OfType<T>().Any();
}
