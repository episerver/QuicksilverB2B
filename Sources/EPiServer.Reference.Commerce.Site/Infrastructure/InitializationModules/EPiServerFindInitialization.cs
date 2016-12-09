using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find.Cms;
using EPiServer.Find.Cms.Conventions;
using EPiServer.Find.Commerce;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Reference.Commerce.Site.Infrastructure.Business;
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
            //ContentIndexer.Instance.Conventions.ForInstancesOf<VariationContent>().ShouldIndex(x => false);

            CatalogContentClientConventions instance2 = context.Locate.Advanced.GetInstance<B2BCatalogContentClientConventions>();
            CatalogContentClientConventions instance = context.Locate.Advanced.GetInstance<CatalogContentClientConventions>();
            if (!(instance is B2BCatalogContentClientConventions))
            {
                throw new InitializationException("catalog conventions not initialized properly");
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}