using System.Web.Mvc;
using Panmedia.EventManager.Models;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard;
using Orchard.Mvc;
using Orchard.DisplayManagement;
using Orchard.Themes;
using Panmedia.EventManager.Services;
using Panmedia.EventManager.ViewModels;
using Panmedia.Artist.Services;
using System.Linq;
using System.Collections.Generic;
using Orchard.Data;
using Orchard.Localization.Models;
using Orchard.Localization.Services;

namespace Panmedia.EventManager.Controllers
{
    [Themed]
    public class EventController : Controller, IUpdateModel
    {
        private readonly IContentManager _contentManager;
        private readonly IMemberEventService _eventService;
        private readonly IArtistUserService _artistUserService;
        private readonly IRepository<ParticipantRecord> _participantRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IOrchardServices _orchardServices;
        dynamic Shape { get; set; }

        public EventController(
            IContentManager contentManager,
            IShapeFactory shapeFactory,
            IMemberEventService eventService,
            IArtistUserService artistUserService,
            IRepository<ParticipantRecord> participantRepository,
            ILocalizationService localizationService,
            IOrchardServices orchardServices
            )
        {
            _contentManager = contentManager;
            Shape = shapeFactory;
            _eventService = eventService;
            _artistUserService = artistUserService;
            _participantRepository = participantRepository;
            _localizationService = localizationService;
            _orchardServices = orchardServices;
        }

        // GET Main Event display screen, if need be
        public ActionResult DisplayEvent(int Id)
        {
            var partRecord = _eventService.GetEvent(Id);
            // get master participant list from da-DK main content item, if id passed is en-US variant of content      
            var masterContentItemId = getMasterContentItemId(Id);
            var participants = _eventService.GetEventParticipants(masterContentItemId).ToList();
            var model = new MemberEventVM(partRecord, participants);
            return View("Parts/MemberEvent", model);
        }


        // GET Modal signup screen
        public ActionResult RegisterModalPopUp(string userName, int eventItemId)
        {
            var participants = _eventService.GetEventParticipants(eventItemId).ToList();
            
            // check if user is already registered for event
            bool isRegistered = false;

            foreach (var p in participants)
            {
                if (p.UserName == userName)
                {
                    isRegistered = true;
                }
            }

            if (!isRegistered)
            {
                var participantName = "";
                var participantProfileId = 0;

                if (userName == "admin")
                    participantName = "Rose Mary, the Admin";
                else
                    // get artist username and profile id for display / linkage from _artistService
                    participantName = _artistUserService.GetFullName(userName);
                    participantProfileId = _artistUserService.GetArtistProfileId(userName);                   

                // ready model for submit screen
                var model = new RegisterParticipantVM
                {
                    UserName = userName,
                    ParticipantProfileId = participantProfileId,
                    ParticipantName = participantName,
                    Comment = "",
                    EventId = eventItemId
                };
                return View("Event/RegisterModal", model);
            }
            return View("Event/RegisterModalNoEntry");
        }

        [HttpPost]
        // POST Modal signup screen
        public ActionResult RegisterModalPopUp(string userName, string participantName, string comment, int eventId)
        {
            // handle registration to master content item w culture da-DK only
            var masterContentItemId = getMasterContentItemId(eventId);

            _participantRepository.Create(new ParticipantRecord
            {
                UserName = userName,
                ParticipantName = participantName,
                ParticipantComment = comment,                
                MemberEventPartRecord_Id = masterContentItemId,
                IsOnWaitingList = false
            });

            // return view with updated list of participants
            var partRecord = _eventService.GetEvent(eventId);
            IList<ParticipantRecord> participants = _eventService.GetEventParticipants(masterContentItemId).ToList();
            var model = new MemberEventVM(partRecord, participants);
            return View("Parts/MemberEventSuccess", model);
        }

        public ActionResult DeleteParticipant(string userName, int eventId)
        {
            var masterContentItemId = getMasterContentItemId(eventId);
            var pRecord = _participantRepository.Fetch
                (p => p.UserName == userName && p.MemberEventPartRecord_Id == masterContentItemId).First();
            if(pRecord != null)
                _participantRepository.Delete(pRecord);

            // return 'Afmeldt' view with updated list of participants      
            var partRecord = _eventService.GetEvent(eventId);
            IList<ParticipantRecord> participants = _eventService.GetEventParticipants(masterContentItemId).ToList();
            var model = new MemberEventVM(partRecord, participants);
            return View("Parts/MemberEventAfmeld", model);
        }

        private int getMasterContentItemId (int eventId)
        {
            var masterContentItemId = eventId;
            var wc = _orchardServices.WorkContext;
            if (wc.CurrentCulture != "da-DK")
            {
                var eventLocalizations = _localizationService.GetLocalizations(_contentManager.Get(eventId));
                if (eventLocalizations.Count() >= 1)
                {
                    // get the Main Event Content Item, based on hardcoded DK site culture
                    var mainCI = eventLocalizations.Where(e => e.Culture.Culture == "da-DK").FirstOrDefault();
                    masterContentItemId = mainCI.ContentItem.Id;
                }
            }
            return masterContentItemId;
        }

        // required by IUpdateModel
        bool IUpdateModel.TryUpdateModel<TModel>(TModel model,
            string prefix,
            string[] includeProperties,
            string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

    }
}
