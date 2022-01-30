using ChatApp.Business.Factories;
using ChatApp.Business.Interfaces;
using ChatApp.Dto;

namespace ChatApp.Business.Helpers.Base
{
    public class HelperBase
    {
        public readonly IRepository<Message, MessageViewModel> Messages;
        public readonly IRepository<ChatRoom, ChatRoomViewModel> ChatRooms;

        public HelperBase(ServiceFactory serviceFactory)
        {
            Messages = serviceFactory.Messages;
            ChatRooms = serviceFactory.ChatRooms;
        }
    }
}
