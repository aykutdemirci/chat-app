using ChatApp.Business.Factories;
using ChatApp.Business.Helpers.Base;
using ChatApp.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChatApp.Business.Helpers.EntityHelpers
{
    public class ChatRoomHelper : HelperBase
    {
        public ChatRoomHelper(ServiceFactory serviceFactory) : base(serviceFactory)
        {

        }

        public bool CreateChatRoom(string roomName)
        {
            try
            {
                roomName = roomName?.Trim();
                if (!string.IsNullOrEmpty(roomName))
                {
                    ChatRooms.Save(new ChatRoomViewModel { Name = roomName });
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
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
