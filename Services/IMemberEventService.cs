using System;
using Orchard.ContentManagement;
using System.Web.Mvc;
using Orchard;
using Panmedia.EventManager.Models;
using Panmedia.EventManager.ViewModels;
using System.Linq;

namespace Panmedia.EventManager.Services
{
    public interface IMemberEventService : IDependency
    {       
        void EditEvent(MemberEventPart part, MemberEventVM model);
        void UpdateEvent(MemberEventPartRecord partRecord);
        MemberEventPartRecord GetEvent(int id);
        string GetEventTitle(int id); 
        IContentQuery<MemberEventPart, MemberEventPartRecord> GetEvents();
        IContentQuery<MemberEventPart, MemberEventPartRecord> GetEvents(bool isShown);
        IQueryable<ParticipantRecord> GetEventParticipants(int eventId);
        void DeleteParticipant(ParticipantRecord p);
        void DeleteEvent(int id);        
        bool IsEventClosed(int id);
    }
}