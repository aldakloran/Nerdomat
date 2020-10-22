using System.Collections.Generic;

namespace Nerdomat.Models
{
    public class Config
    {
        public ulong MyGuildId { get; set; }
        public ulong TestChannelId { get; set; }
        public ulong FlaskChannelId { get; set; }
        public ulong DefaultUserRole { get; set; }
        public ulong LogChannelId { get; set; }
        public string SettingsDirectory { get; set; }
        public GoogleSettings GoogleSettings { get; set; }
        public WarcraftLogs WarcraftLogs { get; set; }
    }

    public class GoogleSettings
    {
        public string ApplicationName { get; set; }
        public string SpreadsheetId { get; set; }
        public string ServiceAccountEmail { get; set; }
        public string Jsonfile { get; set; }
        public string[] Scopes { get; set; }
        public FlaskData FlaskData { get; set; }
        public string ReportChartUrl { get; set; }
    }

    public class FlaskData
    {
        public string ReportDateAddres { get; set; }
        public string ReportValuesAddres { get; set; }
    }

    public class WarcraftLogs
    {
        public string Url { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public List<string> IgnoreTypes { get; set; }
    }
}