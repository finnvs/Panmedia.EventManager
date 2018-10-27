using Orchard;
using System.IO;

namespace Panmedia.EventManager.Services
{
    public interface IXmlService : IDependency
    {
        byte[] ExportToXml(int eventId);
        void Import(Stream stream);
    }
}