namespace Motto.API.Model
{
    public class MottoLanguage: BaseEntity
    {

        public string Language { get; private set; }

        public MottoLanguage(string language)
        {
            Language = language;
        }
    }
}
