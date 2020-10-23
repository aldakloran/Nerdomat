using System.Collections.Generic;

namespace Nerdomat.Models
{
    public class NerdModel
    {
        public string DiscordTag { get; set; }
        public string MainNick { get; set; }
        public List<string> AllNicks { get; set; }
        public bool Handled { get; set; }
    }
}