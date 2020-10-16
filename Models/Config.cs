namespace Nerdomat.Models
{
    public class Config
    {
        public ulong MyGuildId { get; set; }
        public ulong TestChannelId { get; set; }
        public ulong DefaultUserRole { get; set; }
        public ulong LogChannelId { get; set; }
        public string SettingsDirectory { get; set; }
        public GoogleSettings GoogleSettings { get; set; }
    }

    public class GoogleSettings
    {
        public string ApplicationName { get; set; }
        public string SpreadsheetId { get; set; }
        public string ServiceAccountEmail { get; set; }
        public string Jsonfile { get; set; }
        public string[] Scopes { get; set; }
    }

}