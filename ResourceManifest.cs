using Orchard.UI.Resources;
using System;

namespace Panmedia.EventManager
{
    public class ResourceManifest : IResourceManifestProvider
    {   
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            var manifest = builder.Add();
            
            manifest.DefineScript("jQueryUI").SetUrl("ui/jquery-ui.min.js");            
            manifest.DefineScript("jQueryUI_DatePicker").SetUrl("ui/datepicker.min.js", "ui/datepicker.js").SetVersion("1.11.2").SetDependencies("jQueryUI_Core");
            manifest.DefineScript("jQueryUI_TimePicker").SetUrl("ui/jquery-ui-timepicker-addon.min.js").SetDependencies("jQueryUI_Core");
            manifest.DefineScript("jQueryUI_SliderAccess").SetUrl("ui/jquery-ui-sliderAccess.js").SetDependencies("jQueryUI_Core");       
            manifest.DefineScript("Toastr").SetUrl("toastr.min.js");
                 
            manifest.DefineStyle("jQueryUI_DatePicker").SetUrl("Styles/jquery-datetime-editor.css");
            manifest.DefineStyle("jQueryUI_TimePicker").SetUrl("Styles/jquery-ui-timepicker-addon.css");
            manifest.DefineStyle("jQueryUI").SetUrl("Styles/jquery-ui.css");
            manifest.DefineStyle("Toastr").SetUrl("Styles/toastr.min.css");
        }
    }
}