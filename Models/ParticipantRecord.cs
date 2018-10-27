using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Panmedia.EventManager.Models
{
    public class ParticipantRecord
    {
        public virtual int Id { get; set; }        
        public virtual string UserName { get; set; }        
        public virtual string ParticipantName { get; set; }
        public virtual string ParticipantComment { get; set; }        
        public virtual int MemberEventPartRecord_Id { get; set; }             
        public virtual bool IsOnWaitingList { get; set; } 
    }
}