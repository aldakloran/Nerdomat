using System.Collections.Generic;

namespace Nerdomat.Models
{
    public class Config
    {
        public ulong MyGuildId { get; set; }
        public ulong AdminChannelId { get; set; }
        public ulong GeneralChannelId { get; set; }
        public ulong TestChannelId { get; set; }
        public ulong FlaskChannelId { get; set; }
        public ulong DefaultUserRole { get; set; }
        public ulong LogChannelId { get; set; }
        public string SettingsDirectory { get; set; }
        public int WatchdogMaxFailLimit { get; set; }
        public GoogleSettings GoogleSettings { get; set; }
        public WarcraftLogs WarcraftLogs { get; set; }
        public GuildData GuildData { get; set; }
    }

    public class GuildData
    {
        public string GuildName { get; set; }
        public string GuildUrl { get; set; }
        public ulong GuildRaidLeadreId { get; set; }
        public string GuildRaidLeaderName { get; set; }
    }

    public class GoogleSettings
    {
        public string ApplicationName { get; set; }
        public string SpreadsheetId { get; set; }
        public string ServiceAccountEmail { get; set; }
        public string Jsonfile { get; set; }
        public string[] Scopes { get; set; }
        public FlaskData FlaskData { get; set; }
        public NerdsData NerdsData { get; set; }
        public NewMembers NewMembers { get; set; }
        public string ReportChartUrl { get; set; }
    }

    public class FlaskData
    {
        public string ReportDateFormat { get; set; }
        public string ReportDateAddres { get; set; }
        public string ReportValuesAddres { get; set; }
        public string ReportSubstarctDateAddres { get; set; }
        public string ReportFlaskCountAddresTemplate { get; set; }
    }

    public class NerdsData
    {
        public string ConfigAltRowsAddres { get; set; }
        public string ConfigAltColumnsAddres { get; set; }
        public string MainData { get; set; }
        public int AltsDataOffset { get; set; }
        public string AltsDataTemplate { get; set; }
    }

    public class NewMembers
    {
        public string ConfigLastRow { get; set; }
        public string NewMembersAddresTemplate { get; set; }
    }

    public class WarcraftLogs
    {
        public string Url { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public List<string> IgnoreTypes { get; set; }
        public string FightKeyRegex { get; set; }
        public string PaidEmaoteCode { get; set; }
        public string DoneEmoteCode { get; set; }
    }
}