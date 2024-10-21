using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatedChatApp.model.Entities.Base;

namespace UpdatedChatApp.model.Entities
{
    public class ChatMessage : BaseIdEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }

        /* [ForeignKey("SenderId")]
         public virtual Register Sender { get; set; }

         [ForeignKey("ReceiverId")]
         public virtual Register Receiver { get; set; }*/
    }
}
