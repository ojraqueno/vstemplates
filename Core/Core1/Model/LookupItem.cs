namespace Core1.Model
{
    public interface ILookupItem
    {
        string Value { get; }
        string Text { get; }
    }

    public class LookupItem : ILookupItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}