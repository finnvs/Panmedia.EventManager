using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System;

namespace Panmedia.EventManager.Models
{
    public class MemberEventPart : ContentPart<MemberEventPartRecord>
    {
  
        [Required(ErrorMessage = "En adresse skal indtastes")]
        public string EventAddress
        {
            get { return Retrieve(x => x.EventAddress); }
            set { Store(x => x.EventAddress, value); }
        }

        [Required(ErrorMessage = "En kort beskrivelse skal indtastes")]
        public string EventDescription
        {
            get { return Retrieve(x => x.EventDescription); }
            set { Store(x => x.EventDescription, value); }
        }

        public string GoogleStaticMapLink
        {
            get { return Retrieve(x => x.GoogleStaticMapLink); }
            set { Store(x => x.GoogleStaticMapLink, value); }
        }

        public DateTime? EventStartUtc
        {
            get { return Retrieve(x => x.EventStartUtc); }
            set { Store(x => x.EventStartUtc, value); }
        }

        public DateTime? EventStopUtc
        {
            get { return Retrieve(x => x.EventStopUtc); }
            set { Store(x => x.EventStopUtc, value); }
        }

        public int MaxAttendees
        {
            get { return Retrieve(x => x.MaxAttendees); }
            set { Store(x => x.MaxAttendees, value); }
        }

        public bool Shown
        {
            get { return Retrieve(x => x.Shown); }
            set { Store(x => x.Shown, value); }
        }

        public bool WaitingList
        {
            get { return Retrieve(x => x.WaitingList); }
            set { Store(x => x.WaitingList, value); }
        }

    }
}