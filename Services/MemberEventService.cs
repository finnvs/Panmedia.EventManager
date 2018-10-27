using Panmedia.EventManager.Models;
// using Panmedia.Artist.Models;
using Panmedia.EventManager.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Core.Title.Models;

namespace Panmedia.EventManager.Services
{
    public class MemberEventService : IMemberEventService
    {
        private readonly IRepository<MemberEventPartRecord> _eventRepository;
        private readonly IRepository<ParticipantRecord> _participantRepository;        
        private readonly IOrchardServices _orchardServices;


        public MemberEventService(
            IRepository<MemberEventPartRecord> eventRepository,
            IRepository<ParticipantRecord> participantRepository,
            IOrchardServices orchardServices)
        {
            _eventRepository = eventRepository;
            _participantRepository = participantRepository;           
            _orchardServices = orchardServices;
        }

        public IQueryable<ParticipantRecord> GetEventParticipants(int eventId)
        {
           return _participantRepository.Fetch(p => p.MemberEventPartRecord_Id == eventId).OrderBy(p => p.Id).AsQueryable<ParticipantRecord>(); // .ToList();
        }

        public void DeleteParticipant(ParticipantRecord p)
        {
            _participantRepository.Delete(p);
        }

        public MemberEventPartRecord GetEvent(int Id)
        {
            return _eventRepository.Get(Id);
        }

        // Expensive operation to get a Title part
        public string GetEventTitle(int Id)
        {
            var itemRecord = _orchardServices.ContentManager
                .Query<MemberEventPart, MemberEventPartRecord>()
                .Where(e => e.Id == Id);          
            
            var contentItem = itemRecord.ContentManager.Get(Id);

            return contentItem.Content.TitlePart.Title;
        }

        public IContentQuery<MemberEventPart, MemberEventPartRecord> GetEvents()
        {
            return _orchardServices.ContentManager
                .Query<MemberEventPart, MemberEventPartRecord>();
        }

        public IContentQuery<MemberEventPart, MemberEventPartRecord> GetEvents(bool isShown)
        {
            return _orchardServices.ContentManager
                .Query<MemberEventPart, MemberEventPartRecord>()
                .Where(e => e.Shown == isShown);
        }

        public void UpdateEvent(MemberEventPartRecord eventRecord)
        {
            _eventRepository.Update(eventRecord);
        }

        public void DeleteEvent(int id)
        {
            _orchardServices.ContentManager.Remove(_orchardServices.ContentManager.Get(id));
            _eventRepository.Delete(_eventRepository.Get(id));
        }

        public bool IsEventClosed(int eventId)
        {
            var eventRecord = GetEvent(eventId);
            return DateTime.Now < eventRecord.EventStartUtc || DateTime.Now > eventRecord.EventStopUtc;
        }

        public void EditEvent(MemberEventPart part, MemberEventVM model)
        {
            if (part == null)
                throw new ArgumentNullException("part");
            if (model == null)
                throw new ArgumentNullException("model");

            var eventRecord = new MemberEventPartRecord
            {
                Id = part.Record.ContentItemRecord.Id,

                EventAddress = model.EventAddress,
                EventDescription = model.EventDescription,
                GoogleStaticMapLink = model.GoogleStaticMapLink,
                EventStartUtc = model.EventStartUtc,
                EventStopUtc = model.EventStopUtc,
                Shown = model.Shown,
                MaxAttendees = model.MaxAttendees,
                WaitingList = model.WaitingList,

                ContentItemRecord = part.Record.ContentItemRecord
            };
            UpdateEvent(eventRecord);
        }
    }
}