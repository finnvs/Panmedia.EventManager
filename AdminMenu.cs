using Orchard;
using Orchard.Localization;
using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Panmedia.EventManager
{
    public class AdminMenu : INavigationProvider
    {
        public string MenuName
        {
            get { return "admin"; }
        }

        public AdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        private Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            builder.AddImageSet("Events")
                .Add(T("Events"), "4",
                    menu => menu.Add(T("Events"), "2", 
                        item => item.Action("EventIndex", "Admin", new { area = "Panmedia.EventManager" })));       
        }
    }
}