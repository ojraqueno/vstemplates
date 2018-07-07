namespace System
{
    public static class ExceptionExtensions
    {
        public static Exception InnermostException(this Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            return ex.InnerException == null ? ex : ex.InnermostException();
        }
    }
}