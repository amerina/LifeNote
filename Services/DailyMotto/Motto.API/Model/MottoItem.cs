namespace Motto.API.Model
{
    public class MottoItem : BaseEntity
    {
        public string Author { get; private set; }

        public string Content { get; private set; }

        public int MottoTypeId { get; private set; }

        public MottoType MottoType { get; private set; }

        public int MottoLanguageId { get; private set; }

        public MottoLanguage MottoLanguage { get; private set; }

        public MottoItem(int mottoTypeId, int mottoLanguageId, string author, string content)
        {
            MottoTypeId = mottoTypeId;
            MottoLanguageId = mottoLanguageId;
            Author = author;
            Content = content;
        }
    }
}
