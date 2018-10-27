using Orchard.ContentManagement.Handlers;
using Panmedia.EventManager.Models;
using Orchard.Data;
using Panmedia.EventManager.Services;

namespace Panmedia.EventManager.Handlers {
  public class MemberEventHandler : ContentHandler {
    public MemberEventHandler(IRepository<MemberEventPartRecord> repository, IMemberEventService eventService) {
      Filters.Add(StorageFilter.For(repository));

            // Cascade delete of Participants when a MemberEvent is deleted
            OnRemoving<MemberEventPart>((context, eventPart) =>
            {
                var participants = eventService.GetEventParticipants(eventPart.Id);
                foreach (var p in participants)
                {
                    eventService.DeleteParticipant(p);
                }
            });
        }
  }
}
