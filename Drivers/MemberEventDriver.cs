using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization.Services;
using Panmedia.EventManager.Models;
using Panmedia.EventManager.Services;
using Panmedia.EventManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Panmedia.EventManager.Drivers
{
    public class MemberEventDriver : ContentPartDriver<MemberEventPart>
    {
        private readonly IContentManager _contentManager;
        private readonly IMemberEventService _eventService;
        private readonly ILocalizationService _localizationService;
        private readonly IOrchardServices _orchardServices;

        public MemberEventDriver(
            IContentManager contentManager,
            IMemberEventService eventService,
            ILocalizationService localizationService,
            IOrchardServices orchardServices
            )
        {
            _contentManager = contentManager;
            _eventService = eventService;
            _localizationService = localizationService;
            _orchardServices = orchardServices;
        }
        protected override DriverResult Display(MemberEventPart part, string displayType, dynamic shapeHelper)
        {
            var participants = new List<ParticipantRecord>();
            var wc = _orchardServices.WorkContext;
            if (wc.CurrentCulture != "da-DK")
            {
                var eventLocalizations = _localizationService.GetLocalizations(_contentManager.Get(part.ContentItem.Id));
                if (eventLocalizations.Count() >= 1)
                {
                    // get the Main Event Content Item, based on hardcoded site culture DK
                    var mainCI = eventLocalizations.Where(e => e.Culture.Culture == "da-DK").FirstOrDefault();
                    var masterContentItemId = mainCI.ContentItem.Id;
                    participants = _eventService.GetEventParticipants(masterContentItemId).ToList();
                }
            }
            else
            {
                participants = _eventService.GetEventParticipants(part.ContentItem.Id).ToList();
            }

            return ContentShape(
                    "Parts_MemberEvent",
                    () => shapeHelper.Parts_MemberEvent(
                        EventAddress: part.EventAddress,
                        EventDescription: part.EventDescription,
                        EventStartUtc: part.EventStartUtc,
                        EventStopUtc: part.EventStopUtc,
                        GoogleStaticMapLink: part.GoogleStaticMapLink,
                        MaxAttendees: part.MaxAttendees,
                        Shown: part.Shown,
                        WaitingList: part.WaitingList,
                        EventId: part.ContentItem.Id,
                        ParticipatingMembers: participants
                        ));

        }

        //GET
        protected override DriverResult Editor(MemberEventPart part, dynamic shapeHelper)
        {
            if (part == null)
                throw new ArgumentNullException("part");

            var model = new MemberEventVM(part);

            return ContentShape("Parts_MemberEvent_Edit",
                   () => shapeHelper.EditorTemplate(
                       TemplateName: "Parts/MemberEvent",
                       Model: model,
                       Prefix: Prefix
                       ));
        }
        //POST
        protected override DriverResult Editor(MemberEventPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        // Importing content
        protected override void Importing(MemberEventPart part, ImportContentContext context)
        {
            context.ImportAttribute(part.PartDefinition.Name, "EventAddress", eventAddress => part.EventAddress = eventAddress);
            context.ImportAttribute(part.PartDefinition.Name, "EventDescription", eventDescription => part.EventDescription = eventDescription);
            context.ImportAttribute(part.PartDefinition.Name, "GoogleStaticMapLink", googleStaticMapLink => part.GoogleStaticMapLink = googleStaticMapLink);            
            context.ImportAttribute(part.PartDefinition.Name, "EventStartUtc", eventStartUtc => part.EventStartUtc = DateTime.Parse(eventStartUtc));
            context.ImportAttribute(part.PartDefinition.Name, "EventStopUtc", eventStopUtc => part.EventStopUtc = DateTime.Parse(eventStopUtc));
            context.ImportAttribute(part.PartDefinition.Name, "MaxAttendees", maxAttendees => part.MaxAttendees = Convert.ToInt32(maxAttendees));
            context.ImportAttribute(part.PartDefinition.Name, "Shown", shown => part.Shown = Convert.ToBoolean(shown));
            context.ImportAttribute(part.PartDefinition.Name, "WaitingList", waitingList => part.WaitingList = Convert.ToBoolean(waitingList));
        }

        // Exporting content
        protected override void Exporting(MemberEventPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("EventAddress", part.EventAddress);
            context.Element(part.PartDefinition.Name).SetAttributeValue("EventDescription", part.EventDescription);
            context.Element(part.PartDefinition.Name).SetAttributeValue("GoogleStaticMapLink", part.GoogleStaticMapLink);
            context.Element(part.PartDefinition.Name).SetAttributeValue("EventStartUtc", part.EventStartUtc);
            context.Element(part.PartDefinition.Name).SetAttributeValue("EventStopUtc", part.EventStopUtc);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MaxAttendees", part.MaxAttendees);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Shown", part.Shown);
            context.Element(part.PartDefinition.Name).SetAttributeValue("WaitingList", part.WaitingList);
        }

        // TODO: Exp / Imp af attendees (participant) liste for hvert member event
    }
}
