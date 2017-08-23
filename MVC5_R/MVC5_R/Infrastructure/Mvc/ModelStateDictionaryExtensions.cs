using Microsoft.AspNet.Identity;

namespace System.Web.Mvc
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddErrors(this ModelStateDictionary modelStateDictionary, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                modelStateDictionary.AddModelError("", error);
            }
        }
    }
}