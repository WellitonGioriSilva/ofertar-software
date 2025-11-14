using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.Authorization.Requirements;
using api_ofertar.Services;
using Microsoft.AspNetCore.Authorization;

namespace api_ofertar.Authorization.Handlers
{
    public class ActiveChurchHandler : AuthorizationHandler<ActiveChurchRequirement>
    {
        private readonly ChurchService _churchService;

    public ActiveChurchHandler(ChurchService churchService)
    {
        _churchService = churchService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveChurchRequirement requirement)
        {
            var churchId = context.User.FindFirst("churchId")?.Value;

            if (churchId == null)
            {
                context.Fail();
                return;
            }

            bool ativa = await _churchService.IsChurchActiveAsync(int.Parse(churchId));

            if (ativa)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}