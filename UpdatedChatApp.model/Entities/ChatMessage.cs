using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatedChatApp.model.Entities.Base;

namespace UpdatedChatApp.model.Entities
{
    public class ChatMessage : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}
