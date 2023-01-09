namespace LifeNote.Models
{
    internal class About
    {
        public string Title => AppInfo.Name;
        public string Version => AppInfo.VersionString;
        public string MoreInfoUrl => "https://github.com/amerina/LifeNote";
        public string Message => "This is a microservice architecture MAUI sample APP.";
    }
}
