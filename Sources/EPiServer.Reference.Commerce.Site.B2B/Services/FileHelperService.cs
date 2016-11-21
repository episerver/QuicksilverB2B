using System;
using System.IO;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using FileHelpers;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IFileHelperService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class FileHelperService : IFileHelperService
    {
        public T[] GetImportData<T>(Stream file) where T : class
        {
            var reader = new StreamReader(file);

            var fileEngine = new FileHelperEngine(typeof(T));
            fileEngine.ErrorManager.ErrorMode = ErrorMode.IgnoreAndContinue;

            return fileEngine.ReadStream(reader, Int32.MaxValue) as T[];
        }
    }
}
