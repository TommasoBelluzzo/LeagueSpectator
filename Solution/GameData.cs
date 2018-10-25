#region Using Directives
using System;
using System.Runtime.Serialization;
#endregion

namespace LeagueSpectator
{
    [DataContract]
    public abstract class GameDTO { }

    [DataContract]
    public sealed class BannedChampion : GameDTO
    {
        #region Properties
        [DataMember(IsRequired=true, Name="pickTurn")]
        public Int32 PickTurn { get; set; }

        [DataMember(IsRequired=true, Name="championId")]
        public Int64 ChampionId { get; set; }

        [DataMember(IsRequired=true, Name="teamId")]
        public Int64 TeamId { get; set; }
        #endregion
    }

    [DataContract]
    public sealed class GameCustomizationObject : GameDTO
    {
        #region Properties
        [DataMember(IsRequired=true, Name="category")]
        public String Category { get; set; }

        [DataMember(IsRequired=true, Name="content")]
        public String Content { get; set; }
        #endregion
    }

    [DataContract]
    public sealed class GameInfo : GameDTO
    {
        #region Properties
        [DataMember(IsRequired=true, Name="bannedChampions")]
        public BannedChampion[] BannedChampions { get; set; }
          
        [DataMember(IsRequired=true, Name="gameLength")]
        public Int64 ElapsedTime { get; set; }

        [DataMember(IsRequired=true, Name="gameId")]
        public Int64 Id { get; set; }

        [DataMember(IsRequired=true, Name="mapId")]
        public Int64 MapId { get; set; }
        
        [DataMember(IsRequired=false, Name="gameQueueConfigId")]
        public Int64 QueueId { get; set; }

        [DataMember(IsRequired=true, Name="gameStartTime")]
        public Int64 StartTime { get; set; }

        [DataMember(IsRequired=true, Name="observers")]
        public Observer Observer { get; set; }

        [DataMember(IsRequired=true, Name="participants")]
        public Participant[] Participants { get; set; }

        [DataMember(IsRequired=true, Name="gameMode")]
        public String Mode { get; set; }

        [DataMember(IsRequired=true, Name="platformId")]
        public String Platform { get; set; }

        [DataMember(IsRequired=true, Name="gameType")]
        public String Type { get; set; }
        #endregion
    }

    [DataContract]
    public sealed class Observer : GameDTO
    {
        #region Properties
        [DataMember(IsRequired=true, Name="encryptionKey")]
        public String EncryptionKey { get; set; }
        #endregion
    }

    [DataContract]
    public sealed class Participant : GameDTO
    {
        #region Properties
        [DataMember(IsRequired=true, Name="bot")]
        public Boolean IsBot { get; set; }

        [DataMember(IsRequired=true, Name="gameCustomizationObjects")]
        public GameCustomizationObject[] GameCustomizationObjects { get; set; }

        [DataMember(IsRequired=true, Name="championId")]
        public Int64 ChampionId { get; set; }

        [DataMember(IsRequired=true, Name="profileIconId")]
        public Int64 IconId { get; set; }

        [DataMember(IsRequired=true, Name="spell1Id")]
        public Int64 SpellId1 { get; set; }

        [DataMember(IsRequired=true, Name="spell2Id")]
        public Int64 SpellId2 { get; set; }

        [DataMember(IsRequired=true, Name="summonerId")]
        public Int64 SummonerId { get; set; }

        [DataMember(IsRequired=true, Name="teamId")]
        public Int64 TeamId { get; set; }

        [DataMember(IsRequired=true, Name="perks")]
        public Perks Perks { get; set; }

        [DataMember(IsRequired=true, Name="summonerName")]
        public String SummonerName { get; set; }
        #endregion
    }

    [DataContract]
    public sealed class Perks : GameDTO
    {
        #region Properties
        [DataMember(IsRequired=true, Name="perkStyle")]
        public Int64 Style { get; set; }

        [DataMember(IsRequired=true, Name="perkSubStyle")]
        public Int64 SubStyle { get; set; }

        [DataMember(IsRequired=true, Name="perkIds")]
        public Int64[] Ids { get; set; }
        #endregion
    }

    [DataContract]
    public sealed class Summoner : GameDTO
    {
        #region Properties
        [DataMember(IsRequired=true, Name="accountId")]
        public Int64 AccountId { get; set; }

        [DataMember(IsRequired=true, Name="profileIconId")]
        public Int64 IconId { get; set; }

        [DataMember(IsRequired=true, Name="id")]
        public Int64 Id { get; set; }

        [DataMember(IsRequired=true, Name="summonerLevel")]
        public Int64 Level { get; set; }

        [DataMember(IsRequired=true, Name="revisionDate")]
        public Int64 RevisionDate { get; set; }

        [DataMember(IsRequired=true, Name="name")]
        public String Name { get; set; }
        #endregion
    }
}
