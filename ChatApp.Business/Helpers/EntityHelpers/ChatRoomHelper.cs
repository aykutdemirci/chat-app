using ChatApp.Business.Factories;
using ChatApp.Business.Helpers.Base;
using ChatApp.Dto;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Business.Helpers.EntityHelpers
{
    public class ChatRoomHelper : HelperBase
    {
        public ChatRoomHelper(ServiceFactory serviceFactory) : base(serviceFactory)
        {

        }

        public List<ChatRoomViewModel> GetChatRooms()
        {
            return ChatRooms.Table.Select(q => new ChatRoomViewModel
            {
                Id = q.Id,
                Name = q.Name
            }).ToList();
        }
    }
}
