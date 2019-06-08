using Microsoft.AspNetCore.Mvc;
using Research.Web.Framework.Controllers;

namespace Research.Web.Controllers
{
    //[HttpsRequirement(SslRequirement.NoMatter)]
    //[WwwRequirement]
    //[CheckAccessPublicStore]
    public abstract partial class BasePublicController : BaseController
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}