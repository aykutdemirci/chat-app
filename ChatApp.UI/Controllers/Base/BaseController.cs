using ChatApp.Business.Factories;
using ChatApp.Business.Helpers.EntityHelpers;
using ChatApp.Cache;
using ChatApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace ChatApp.UI.Controllers.Base
{
    public class BaseController : Controller
    {
        public readonly MessageHelper MessageHelper;
        public readonly ChatRoomHelper ChatRoomHelper;
        public readonly RedisCacheService RedisCacheService;

        private string currentUser;
        public string CurrentUser { get { return currentUser; } }

        public BaseController(ServiceFactory serviceFactory)
        {
            MessageHelper = serviceFactory.MessageHelper;
            ChatRoomHelper = serviceFactory.ChatRoomHelper;
            RedisCacheService = serviceFactory.RedisCacheService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetSessionUser(context.HttpContext);

            base.OnActionExecuting(context);
        }

        private void SetSessionUser(HttpContext httpContext)
        {
            currentUser = httpContext.Session.GetString(SessionNames.CurrentUser);
        }
    }
}
