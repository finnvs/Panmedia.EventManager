using Panmedia.EventManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Panmedia.EventManager.ViewModels
{
    public class EventIndexVM
    {
        public IList<EventEntry> Events { get; set; }
        public EventIndexOptions Options { get; set; }
        public dynamic Pager { get; set; }
    }

    public class EventEntry
    {
        public string EventTitle { get; set; }               
        public MemberEventPartRecord Event { get; set; }
        public bool IsChecked { get; set; }
    }

    public class EventIndexOptions
    {
        public EventIndexFilter Filter { get; set; }
        public EventIndexBulkAction BulkAction { get; set; }
    }

    public enum EventIndexBulkAction
    {
        None,
        Close,
        Open,
        Delete
    }

    public enum EventIndexFilter
    {
        All,
        Closed,
        Open
    }
}