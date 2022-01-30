using System;
using System.Collections.Generic;

namespace ChatApp.Dto
{
    public class ChatRoomViewModel : ViewModelBase
    {
        /// <summary>
        /// Chat odasının adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Chat odasındaki mesajlar
        /// </summary>
        public ICollection<MessageViewModel> Messages { get; set; }
    }

    public class MessageViewModel : ViewModelBase
    {
        /// <summary>
        /// Mesaj metni
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Mesajın oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Mesajı gönderen kullanıcının adı
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Mesaj gönderilecek odanın Id'si
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Mesaj gönderilecek oda
        /// </summary>
        public ChatRoomViewModel Room { get; set; }
    }
}
