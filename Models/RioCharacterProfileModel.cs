using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nerdomat.Models
{
    // RioCharacterProfileModel myDeserializedClass = JsonSerializer.Deserialize<RioCharacterProfileModel>(myJsonResponse);
    public class RioCharacterProfileModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("race")]
        public string Race { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("active_spec_name")]
        public string ActiveSpecName { get; set; }

        [JsonPropertyName("active_spec_role")]
        public string ActiveSpecRole { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("faction")]
        public string Faction { get; set; }

        [JsonPropertyName("achievement_points")]
        public int AchievementPoints { get; set; }

        [JsonPropertyName("honorable_kills")]
        public int HonorableKills { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("realm")]
        public string Realm { get; set; }

        [JsonPropertyName("last_crawled_at")]
        public DateTime LastCrawledAt { get; set; }

        [JsonPropertyName("profile_url")]
        public string ProfileUrl { get; set; }

        [JsonPropertyName("profile_banner")]
        public string ProfileBanner { get; set; }

        [JsonPropertyName("covenant")]
        public Covenant Covenant { get; set; }

        [JsonPropertyName("mythic_plus_scores_by_season")]
        public List<MythicPlusScoresBySeason> MythicPlusScoresBySeason { get; set; }

        [JsonPropertyName("mythic_plus_ranks")]
        public MythicPlusRanks MythicPlusRanks { get; set; }

        [JsonPropertyName("gear")]
        public Gear Gear { get; set; }

        [JsonPropertyName("raid_progression")]
        public RaidProgression RaidProgression { get; set; }
    }

    public class Covenant
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("renown_level")]
        public int RenownLevel { get; set; }
    }

    public class Scores
    {
        [JsonPropertyName("all")]
        public double All { get; set; }

        [JsonPropertyName("dps")]
        public double Dps { get; set; }

        [JsonPropertyName("healer")]
        public double Healer { get; set; }

        [JsonPropertyName("tank")]
        public double Tank { get; set; }

        [JsonPropertyName("spec_0")]
        public double Spec0 { get; set; }

        [JsonPropertyName("spec_1")]
        public double Spec1 { get; set; }

        [JsonPropertyName("spec_2")]
        public double Spec2 { get; set; }

        [JsonPropertyName("spec_3")]
        public double Spec3 { get; set; }
    }

    public class MythicPlusScoresBySeason
    {
        [JsonPropertyName("season")]
        public string Season { get; set; }

        [JsonPropertyName("scores")]
        public Scores Scores { get; set; }
    }

    public class Overall
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class Class
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionOverall
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionClass
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class Tank
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class ClassTank
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionTank
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionClassTank
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class Healer
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class ClassHealer
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionHealer
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionClassHealer
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class Dps
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class ClassDps
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionDps
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionClassDps
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class Spec102
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionSpec102
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class Spec103
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionSpec103
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class Spec104
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionSpec104
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class Spec105
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class FactionSpec105
    {
        [JsonPropertyName("world")]
        public int World { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("realm")]
        public int Realm { get; set; }
    }

    public class MythicPlusRanks
    {
        [JsonPropertyName("overall")]
        public Overall Overall { get; set; }

        [JsonPropertyName("class")]
        public Class Class { get; set; }

        [JsonPropertyName("faction_overall")]
        public FactionOverall FactionOverall { get; set; }

        [JsonPropertyName("faction_class")]
        public FactionClass FactionClass { get; set; }

        [JsonPropertyName("tank")]
        public Tank Tank { get; set; }

        [JsonPropertyName("class_tank")]
        public ClassTank ClassTank { get; set; }

        [JsonPropertyName("faction_tank")]
        public FactionTank FactionTank { get; set; }

        [JsonPropertyName("faction_class_tank")]
        public FactionClassTank FactionClassTank { get; set; }

        [JsonPropertyName("healer")]
        public Healer Healer { get; set; }

        [JsonPropertyName("class_healer")]
        public ClassHealer ClassHealer { get; set; }

        [JsonPropertyName("faction_healer")]
        public FactionHealer FactionHealer { get; set; }

        [JsonPropertyName("faction_class_healer")]
        public FactionClassHealer FactionClassHealer { get; set; }

        [JsonPropertyName("dps")]
        public Dps Dps { get; set; }

        [JsonPropertyName("class_dps")]
        public ClassDps ClassDps { get; set; }

        [JsonPropertyName("faction_dps")]
        public FactionDps FactionDps { get; set; }

        [JsonPropertyName("faction_class_dps")]
        public FactionClassDps FactionClassDps { get; set; }

        [JsonPropertyName("spec_102")]
        public Spec102 Spec102 { get; set; }

        [JsonPropertyName("faction_spec_102")]
        public FactionSpec102 FactionSpec102 { get; set; }

        [JsonPropertyName("spec_103")]
        public Spec103 Spec103 { get; set; }

        [JsonPropertyName("faction_spec_103")]
        public FactionSpec103 FactionSpec103 { get; set; }

        [JsonPropertyName("spec_104")]
        public Spec104 Spec104 { get; set; }

        [JsonPropertyName("faction_spec_104")]
        public FactionSpec104 FactionSpec104 { get; set; }

        [JsonPropertyName("spec_105")]
        public Spec105 Spec105 { get; set; }

        [JsonPropertyName("faction_spec_105")]
        public FactionSpec105 FactionSpec105 { get; set; }
    }

    public class Corruption
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("cloakRank")]
        public int CloakRank { get; set; }

        [JsonPropertyName("spells")]
        public List<object> Spells { get; set; }
    }

    public class Spell
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("school")]
        public int School { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rank")]
        public object Rank { get; set; }
    }

    public class AzeritePower
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("spell")]
        public Spell Spell { get; set; }

        [JsonPropertyName("tier")]
        public int Tier { get; set; }
    }

    public class Corruption2
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Head
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<AzeritePower> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption2 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption3
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Neck
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption3 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<int> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption4
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Shoulder
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption4 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption5
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Back
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption5 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption6
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Chest
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("enchant")]
        public int Enchant { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption6 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption7
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Waist
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption7 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption8
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Shirt
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption8 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<object> Bonuses { get; set; }
    }

    public class Corruption9
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Wrist
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption9 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption10
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Hands
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("enchant")]
        public int Enchant { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption10 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption11
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Legs
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption11 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption12
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Feet
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption12 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption13
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Finger1
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("enchant")]
        public int Enchant { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption13 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption14
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Finger2
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("enchant")]
        public int Enchant { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption14 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption15
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Trinket1
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption15 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption16
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Trinket2
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption16 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Corruption17
    {
        [JsonPropertyName("added")]
        public int Added { get; set; }

        [JsonPropertyName("resisted")]
        public int Resisted { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Mainhand
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_level")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("item_quality")]
        public int ItemQuality { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("is_azerite_armor")]
        public bool IsAzeriteArmor { get; set; }

        [JsonPropertyName("azerite_powers")]
        public List<object> AzeritePowers { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption17 Corruption { get; set; }

        [JsonPropertyName("gems")]
        public List<object> Gems { get; set; }

        [JsonPropertyName("bonuses")]
        public List<int> Bonuses { get; set; }
    }

    public class Items
    {
        [JsonPropertyName("head")]
        public Head Head { get; set; }

        [JsonPropertyName("neck")]
        public Neck Neck { get; set; }

        [JsonPropertyName("shoulder")]
        public Shoulder Shoulder { get; set; }

        [JsonPropertyName("back")]
        public Back Back { get; set; }

        [JsonPropertyName("chest")]
        public Chest Chest { get; set; }

        [JsonPropertyName("waist")]
        public Waist Waist { get; set; }

        [JsonPropertyName("shirt")]
        public Shirt Shirt { get; set; }

        [JsonPropertyName("wrist")]
        public Wrist Wrist { get; set; }

        [JsonPropertyName("hands")]
        public Hands Hands { get; set; }

        [JsonPropertyName("legs")]
        public Legs Legs { get; set; }

        [JsonPropertyName("feet")]
        public Feet Feet { get; set; }

        [JsonPropertyName("finger1")]
        public Finger1 Finger1 { get; set; }

        [JsonPropertyName("finger2")]
        public Finger2 Finger2 { get; set; }

        [JsonPropertyName("trinket1")]
        public Trinket1 Trinket1 { get; set; }

        [JsonPropertyName("trinket2")]
        public Trinket2 Trinket2 { get; set; }

        [JsonPropertyName("mainhand")]
        public Mainhand Mainhand { get; set; }
    }

    public class Gear
    {
        [JsonPropertyName("item_level_equipped")]
        public int ItemLevelEquipped { get; set; }

        [JsonPropertyName("item_level_total")]
        public int ItemLevelTotal { get; set; }

        [JsonPropertyName("artifact_traits")]
        public int ArtifactTraits { get; set; }

        [JsonPropertyName("corruption")]
        public Corruption Corruption { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }
    }

    public class CastleNathria
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("total_bosses")]
        public int TotalBosses { get; set; }

        [JsonPropertyName("normal_bosses_killed")]
        public int NormalBossesKilled { get; set; }

        [JsonPropertyName("heroic_bosses_killed")]
        public int HeroicBossesKilled { get; set; }

        [JsonPropertyName("mythic_bosses_killed")]
        public int MythicBossesKilled { get; set; }
    }

    public class RaidProgression
    {
        [JsonPropertyName("castle-nathria")]
        public CastleNathria CastleNathria { get; set; }
    }
}