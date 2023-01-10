namespace Motto.API.Model
{
    public class MottoType : BaseEntity
    {

        public string Type { get; private set; }

        public MottoType(string type)
        {
            Type = type;
        }
    }
}
