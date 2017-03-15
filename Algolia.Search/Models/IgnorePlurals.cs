namespace Algolia.Search.Models
{
    public interface IIgnorePlurals
    {
        string GetValue();
    }

    public class IgnorePluralsBool : IIgnorePlurals
    {
        public bool Ignored { get; set; }
        public string GetValue()
        {
            return Ignored.ToString().ToLower();
        }
    }

    public class IgnorePluralList : IIgnorePlurals
    {
        public string Ignored { get; set; }
        public string GetValue()
        {
            return Ignored;
        }
    }

}
