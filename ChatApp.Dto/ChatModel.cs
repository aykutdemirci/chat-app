using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Dto
{
    public class ChatRoom : ModelBase
    {
        /// <summary>
        /// Chat odasının adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Chat odasındaki mesajlar
        /// </summary>
        public ICollection<Message> Messages { get; set; }
    }

    public class Message : ModelBase
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
        [ForeignKey(nameof(RoomId))]
        public ChatRoom Room { get; set; }
    }
}
