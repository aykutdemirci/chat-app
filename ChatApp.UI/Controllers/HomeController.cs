using ChatApp.Business.Factories;
using ChatApp.Dto;
using ChatApp.Extensions;
using ChatApp.UI.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.UI.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ServiceFactory serviceFactory) : base(serviceFactory)
        {
            //RedisCacheService.Remove(CacheKeys.LastMessages);
        }

        public IActionResult Index()
        {
            ViewBag.ChatRooms = ChatRoomHelper.GetChatRooms();
            return View();
        }

        public JsonResult SignIn(string nickName)
        {
            if (!string.IsNullOrEmpty(nickName?.Trim()))
            {
                HttpContext.Session.SetString(SessionNames.CurrentUser, nickName);
                return Json(new { result = true });
            }

            return Json(new { result = false });
        }

        public JsonResult SaveMessage(MessageViewModel message)
        {
            bool saved = MessageHelper.SaveMessage(message);
            if (saved)
            {
                List<MessageViewModel> lastMessages = RedisCacheService.Get<List<MessageViewModel>>(CacheKeys.LastMessages);
                lastMessages.Add(message);
                RedisCacheService.Set(CacheKeys.LastMessages, lastMessages);
            }
            return Json(new { result = saved });
        }

        public JsonResult CheckSession()
        {
            return Json(new { result = CurrentUser != null, nickName = CurrentUser });
        }

        public JsonResult GetLastMessages(int roomId)
        {
            List<MessageViewModel> lastMessages = RedisCacheService.Get<List<MessageViewModel>>(CacheKeys.LastMessages);
            if (lastMessages.Count > 0)
            {
                lastMessages = lastMessages.Where(q => q.RoomId == roomId).ToList();
            }
            else
            {
                lastMessages = MessageHelper.GetLastMessages(roomId);
                RedisCacheService.Set(CacheKeys.LastMessages, lastMessages);
            }

            return Json(new { lastMessages });
        }

        public IActionResult CreateRoom(string roomName)
        {
            bool created = ChatRoomHelper.CreateChatRoom(roomName);
            return Json(new { result = created });
        }
    }
}
