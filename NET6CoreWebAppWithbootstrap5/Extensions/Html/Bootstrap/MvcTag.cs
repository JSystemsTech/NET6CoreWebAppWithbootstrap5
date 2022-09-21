using Microsoft.AspNetCore.Mvc.Rendering;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html.Bootstrap
{
    public class MvcTag : IDisposable
    {
        private bool _disposed;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;
        private readonly string _endingTags;

        public MvcTag(ViewContext viewContext, string endingTags = "</div>")
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            _viewContext = viewContext;
            _writer = viewContext.Writer;
            _endingTags = endingTags;
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Write(_endingTags);
            }
        }
        public void EndDiv()
        {
            Dispose(true);
        }
    }
}