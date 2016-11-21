using System.IO;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IFileHelperService
    {
        T[] GetImportData<T>(Stream file) where T : class;
    }
}
