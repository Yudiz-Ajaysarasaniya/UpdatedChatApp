using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedChatApp.model.Entities.Base
{
    public class BaseIdEntity
    {
        public Guid Id { get; set; }
        public BaseIdEntity() => Id = Guid.NewGuid();
    }
}
