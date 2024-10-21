using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatedChatApp.model.Entities.Base;

namespace UpdatedChatApp.model.Entities
{
    public class UserTokens : BaseEntity
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
