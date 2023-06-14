using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Commons;

namespace Store.Web.Areas.Admin.Controllers
{
    [Area(Constants.AdminAreaName)]
    [Authorize(Roles = Constants.Roles.Admin)]
    public abstract class AdminController : Controller
    {
    }
}
