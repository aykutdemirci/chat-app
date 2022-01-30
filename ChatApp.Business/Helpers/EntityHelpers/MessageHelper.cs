using ChatApp.Business.Factories;
using ChatApp.Business.Helpers.Base;
using ChatApp.Dto;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Business.Helpers.EntityHelpers
{
    public class MessageHelper : HelperBase
    {
        public MessageHelper(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        /// <summary>
        /// Mesajı veri tabanına kaydeder
        /// </summary>
        /// <param name="message">MessageViewModel örneği</param>
        /// <returns>Kaydetme işlemi başarılı ise true, değilse false döndürür.</returns>
        public bool SaveMessage(MessageViewModel message)
        {
            if (ChatRooms.Table.Any(q => q.Id == message.RoomId))
            {
                Messages.Save(message);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Veri tabanındaki tüm mesajları getirir.
        /// </summary>
        /// <returns>List<MessageViewModel> döndürür.</returns>
        public List<MessageViewModel> GetAll()
        {
            return Messages.Table.Select(q => q.AsViewModel<MessageViewModel>()).ToList();
        }

        /// <summary>
        /// Veri tabanından roomId'si verilen mesajları döndürür.
        /// </summary>
        /// <param name="roomId">Mesajları istenen odanın Id'si</param>
        /// <returns>List<MessageViewModel> döndürür.</returns>
        public List<MessageViewModel> GetLastMessages(int roomId)
        {
            return Messages.Table.Where(q => q.RoomId == roomId)
                                 .Select(q => new MessageViewModel
                                 {
                                     RoomId = q.RoomId,
                                     UserName = q.UserName,
                                     CreatedAt = q.CreatedAt,
                                     MessageText = q.MessageText
                                 }).ToList();
        }
    }
}
