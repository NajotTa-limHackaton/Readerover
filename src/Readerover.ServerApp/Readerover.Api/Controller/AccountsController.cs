using Microsoft.AspNetCore.Mvc;
using Readerover.Application.Common.Identity.Services;

namespace Readerover.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IAccountService accountService) : ControllerBase
{
    
}
