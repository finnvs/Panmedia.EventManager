using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Orchard.Data;
using Panmedia.EventManager.Models;
using Orchard.ContentManagement;
using System.Xml.Serialization;
using System.Xml;

namespace Panmedia.EventManager.Services
{
    public class XmlService : IXmlService
    {

        private readonly IRepository<MemberEventPartRecord> _eventRepository;
        private readonly IRepository<ParticipantRecord> _participantRepository;       
        private readonly IContentManager _contentManager;
        private readonly IMemberEventService _memberEventService;

        public XmlService(IRepository<MemberEventPartRecord> eventRepository,            
            IRepository<ParticipantRecord> participantRepository,            
            IContentManager contentManager,
            IMemberEventService memberEventService)
        {
            _eventRepository = eventRepository;
            _participantRepository = participantRepository;            
            _contentManager = contentManager;
            _memberEventService = memberEventService;
        }

        public byte[] ExportToXml(int eventId)
        {

            var memberEvent = _eventRepository.Get(eventId);
            var participants = _memberEventService.GetEventParticipants(eventId);
            
            var entry = new EventEntry
            {
                EventAddress = memberEvent.EventAddress,
                EventDescription = memberEvent.EventDescription,
                GoogleStaticMapLink = memberEvent.GoogleStaticMapLink,
                EventStartUtc = memberEvent.EventStartUtc.Value,
                EventStopUtc = memberEvent.EventStopUtc.Value,
                MaxAttendees = memberEvent.MaxAttendees,
                Shown = memberEvent.Shown,
                WaitingList = memberEvent.WaitingList,
                Participants = participants.Select(r => new ParticipantRecord
                {                    
                    UserName = r.UserName,
                    ParticipantName = r.ParticipantName,
                    ParticipantComment = r.ParticipantComment,
                    MemberEventPartRecord_Id = r.MemberEventPartRecord_Id,
                    IsOnWaitingList = r.IsOnWaitingList
                }).ToList()
            };
            XmlSerializer serializer = new XmlSerializer(typeof(EventEntry));
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, new XmlWriterSettings() { CheckCharacters = true });
            serializer.Serialize(writer, entry);
            return ms.ToArray();
        }

        [Serializable]
        public class EventEntry
        {
            public string EventAddress { get; set; }
            public string EventDescription { get; set; }
            public string GoogleStaticMapLink { get; set; }
            
            public DateTime EventStartUtc { get; set; }
            public DateTime EventStopUtc { get; set; }
            public int MaxAttendees { get; set; }
            public bool Shown { get; set; }
            public bool WaitingList { get; set; }
            
            public List<ParticipantRecord> Participants { get; set; }
            public class Participant
            {
                public string UserName { get; set; }
                public string ParticipantName { get; set; }
                public string ParticipantComment { get; set; }
                public int MemberEventPartRecord_Id { get; set; }
                public bool IsOnWaitingList { get; set; }
            }
        }


        public void Import(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EventEntry));
                var entry = serializer.Deserialize(reader) as EventEntry;

                if (entry != null)
                {
                    var m = _contentManager.Create<MemberEventPart>("MemberEvent", memberEvent =>
                    {
                        memberEvent.Record.EventAddress = entry.EventAddress;
                        memberEvent.Record.EventDescription = entry.EventDescription;
                        memberEvent.Record.GoogleStaticMapLink = entry.GoogleStaticMapLink;
                        memberEvent.Record.EventStartUtc = entry.EventStartUtc;
                        memberEvent.Record.EventStopUtc = entry.EventStopUtc;
                        memberEvent.Record.MaxAttendees = entry.MaxAttendees;
                        memberEvent.Record.Shown = entry.Shown;
                        memberEvent.Record.WaitingList = entry.WaitingList;
                    });
                    foreach (var participant in entry.Participants)
                    {
                        ParticipantRecord p = new ParticipantRecord
                        {
                            UserName = participant.UserName,
                            ParticipantName = participant.ParticipantName,
                            ParticipantComment = participant.ParticipantComment,
                            IsOnWaitingList = participant.IsOnWaitingList,
                            MemberEventPartRecord_Id = m.Id

                        };
                        _participantRepository.Create(p);                        
                    }
                }
            }
        }
    }
}