using EPiServer.Core;
using EPiServer.Find.Cms;
using EPiServer.Find.Cms.Conventions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.InitializationModules
{
    [ModuleDependency(typeof(InitializationModule))]
    public class EPiServerFindInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            ContentIndexer.Instance.Conventions.ForInstancesOf<PageData>().ShouldIndex(x => false);
            ContentIndexer.Instance.Conventions.ForInstancesOf<BlockData>().ShouldIndex(x => false);
            ContentIndexer.Instance.Conventions.ForInstancesOf<MediaData>().ShouldIndex(x => false);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}