using System.Collections.Generic;
using System.Web.Optimization;

namespace MVC5_R.WebApp.Infrastructure.Bundling
{
    public class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}