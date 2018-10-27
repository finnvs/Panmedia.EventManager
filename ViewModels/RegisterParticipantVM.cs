using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Panmedia.EventManager.ViewModels
{
    public class RegisterParticipantVM
    {
        public int ParticipantProfileId { get; set; }
        public string UserName { get; set; }
        public string ParticipantName { get; set; }
        public string Comment { get; set; }
        public int EventId { get; set; }
    }
}