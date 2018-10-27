using Panmedia.EventManager.Models;
using System;
using System.Collections.Generic;

namespace Panmedia.EventManager.ViewModels
{
    public class MemberEventVM
    {

        public MemberEventVM() {
        }

        public MemberEventVM(MemberEventPart part)
        {
            EventId = part.Id;
            EventAddress = part.EventAddress == null ? "" : part.EventAddress;
            EventDescription = part.EventDescription == null ? "" : part.EventDescription;
            EventStartUtc = part.EventStartUtc == null ? DateTime.Now : part.EventStartUtc.Value;
            EventStopUtc = part.EventStopUtc == null ? DateTime.Now : part.EventStopUtc.Value;
            GoogleStaticMapLink = part.GoogleStaticMapLink == null ? "" : part.GoogleStaticMapLink;
            MaxAttendees = part.MaxAttendees;
            Shown = part.Shown;
            WaitingList = part.WaitingList;            
        }

        public MemberEventVM(MemberEventPartRecord partRecord, IList<ParticipantRecord> participants)
        {
            EventId = partRecord.Id;
            EventAddress = partRecord.EventAddress;
            EventDescription = partRecord.EventDescription;
            EventStartUtc = partRecord.EventStartUtc;
            EventStopUtc = partRecord.EventStopUtc;
            GoogleStaticMapLink = partRecord.GoogleStaticMapLink;
            MaxAttendees = partRecord.MaxAttendees;
            Shown = partRecord.Shown;
            WaitingList = partRecord.WaitingList;
            ParticipatingMembers = participants;
        }

        public IList<ParticipantRecord> ParticipatingMembers;
        public int EventId { get; set; }
        public string EventDescription { get; set; }
        public string EventAddress { get; set; }
        public string GoogleStaticMapLink { get; set; }
        public DateTime? EventStartUtc { get; set; }
        public DateTime? EventStopUtc { get; set; }
        public int MaxAttendees { get; set; }
        public bool Shown { get; set; }
        public bool WaitingList { get; set; }
    }
}