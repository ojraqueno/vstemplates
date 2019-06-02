using System.Threading.Tasks;

namespace Enketo.RazorEmails.Infrastructure
{
    public abstract class BaseEmailViewModel
    {
        public async Task<string> ToHtmlString(IViewRenderer viewRenderer) => await viewRenderer.RenderViewToStringAsync(this);

        internal abstract string ViewPath { get; }
    }
}