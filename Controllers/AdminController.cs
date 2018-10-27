using Orchard.ContentManagement;
using Orchard.Core.Contents.Controllers;
using Orchard.DisplayManagement;
using Orchard.Settings;
using Orchard.UI.Navigation;
using Panmedia.EventManager.Models;
using Panmedia.EventManager.Services;
using Panmedia.EventManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Panmedia.EventManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMemberEventService _eventService;
        private readonly ISiteService _siteService;
        private readonly IXmlService _xmlService;

        dynamic Shape { get; set; }
        public AdminController(

            ISiteService siteService,
            IMemberEventService eventService,
            IXmlService xmlService,
            IShapeFactory shapeFactory       
            )
        {
            _siteService = siteService;
            _eventService = eventService;
            _xmlService = xmlService;
            Shape = shapeFactory;
        }

        // GET - Admin UI for Events
        public ActionResult EventIndex(EventIndexOptions options, PagerParameters pagerParameters)
        {
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            // Default options
            if (options == null)
                options = new EventIndexOptions();

            // Filtering
            IContentQuery<MemberEventPart, MemberEventPartRecord> eventsQuery;
            switch (options.Filter)
            {
                case EventIndexFilter.All:
                    eventsQuery = _eventService.GetEvents();
                    break;
                case EventIndexFilter.Open:
                    eventsQuery = _eventService.GetEvents(true);
                    break;
                case EventIndexFilter.Closed:
                    eventsQuery = _eventService.GetEvents(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("options");
            }

            var pagerShape = Shape.Pager(pager).TotalItemCount(eventsQuery.Count());
            var entries = eventsQuery
                .OrderByDescending<MemberEventPartRecord>(e => e.Id)
                .Slice(pager.GetStartIndex(), pager.PageSize)
                .ToList()
                .Select(e => CreateEventEntry(e.Record));

            var model = new EventIndexVM
            {
                Events = entries.ToList(),
                Options = options,
                Pager = pagerShape
            };
            return View((object)model);
        }

        public EventEntry CreateEventEntry(MemberEventPartRecord e)
        {
            if (e == null)
                throw new ArgumentNullException("MemberEvent");
            return new EventEntry
            {
                EventTitle = _eventService.GetEventTitle(e.Id),
                Event = e,
                IsChecked = false
            };
        }

        // POST - bulk delete action for Member Events
        [HttpPost]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult EventIndex(FormCollection form)
        {
            var viewModel = new EventIndexVM { Events = new List<EventEntry>(), Options = new EventIndexOptions() };
            UpdateModel(viewModel);

            IEnumerable<EventEntry> checkedEntries = viewModel.Events.Where(s => s.IsChecked);
            switch (viewModel.Options.BulkAction)
            {
                case EventIndexBulkAction.None:
                    break;
                case EventIndexBulkAction.Open:
                    foreach (EventEntry entry in checkedEntries)
                    {
                        throw new NotImplementedException();
                        // _eventService.OpenEvent(entry.Event.Id);
                    }
                    break;
                case EventIndexBulkAction.Close:
                    foreach (EventEntry entry in checkedEntries)
                    {
                        throw new NotImplementedException();
                        // _eventService.CloseEvent(entry.Event.Id);
                    }
                    break;
                case EventIndexBulkAction.Delete:
                    foreach (EventEntry entry in checkedEntries)
                    {
                        _eventService.DeleteEvent(entry.Event.Id);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("form");
            }
            return RedirectToAction("EventIndex");
        }

        [HttpGet]
        public FileResult ExportToXml(int id)
        {
            return File(_xmlService.ExportToXml(id), "text/xml", String.Format("MemberEvent_{0}.xml", id));
        }

        [HttpPost]
        public ActionResult ImportFromXml()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (Request.Files[i].InputStream != null)
                    _xmlService.Import(Request.Files[i].InputStream);
            }
            return RedirectToAction("EventIndex");
        }
    }
}