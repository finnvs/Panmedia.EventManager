using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace Panmedia.EventManager.Models
{
    public class MemberEventPartRecord : ContentPartRecord
    {
        public virtual string EventAddress { get; set; }
        public virtual string EventDescription { get; set; }        
        public virtual string GoogleStaticMapLink { get; set; }
        public virtual DateTime? EventStartUtc { get; set; }
        public virtual DateTime? EventStopUtc { get; set; }
        public virtual int MaxAttendees { get; set; }
        public virtual bool Shown { get; set; }
        public virtual bool WaitingList { get; set; }
    }
}