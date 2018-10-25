#region Using Directives
using System;
using LeagueSpectator.Properties;

#endregion

namespace LeagueSpectator
{
    public sealed class GameRegion
    {
        #region Members (Static)
        private static readonly GameRegion[] s_List =
        {
            new GameRegion("Brazil", "BR", "BR1", "BR", 80),
            new GameRegion("Europe Nordic & East", "EUNE", "EUN1", "EU", 8088),
            new GameRegion("Europe West", "EUW", "EUW1", "EUW1", 80),
            new GameRegion("Japan", "JP", "JP1", "JP1", 80),
            new GameRegion("Korea", "KR", "KR", "KR", 80),
            new GameRegion("Latin America North", "LAN", "LA1", "LA1", 80),
            new GameRegion("Latin America South", "LAS", "LA2", "LA2", 80),
            new GameRegion("North America", "NA", "NA1", "NA", 80),
            new GameRegion("Oceania", "OCE", "OC1", "OC1", 80),
            new GameRegion("Russia", "RU", "RU", "RU", 80),
            new GameRegion("Turkey", "TR", "TR1", "TR", 80),
            new GameRegion("Public Beta Environment", "PBE", "PBE1", "PBE1", 80)
        };
        #endregion

        #region Properties
        public String Code { get; }

        public String EndPoint { get; }

        public String Name { get; }

        public String SpectatorEndPoint { get; }

        public UInt16 SpectatorPort { get; }
        #endregion

        #region Constructors
        public GameRegion(String name, String code, String endPoint, String spectatorEndPoint, UInt16 spectatorPort)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException(Resources.ErrorAttribute, nameof(name));

            if (String.IsNullOrWhiteSpace(code))
                throw new ArgumentException(Resources.ErrorAttribute, nameof(code));

            if (String.IsNullOrWhiteSpace(endPoint))
                throw new ArgumentException(Resources.ErrorAttribute, nameof(endPoint));

            if (String.IsNullOrWhiteSpace(spectatorEndPoint))
                throw new ArgumentException(Resources.ErrorAttribute, nameof(spectatorEndPoint));

            EndPoint = endPoint.ToLowerInvariant();
            Name = name;
            Code = code;
            SpectatorEndPoint = spectatorEndPoint.ToLowerInvariant();
            SpectatorPort = spectatorPort;
        }
        #endregion

        #region Methods (Static)
        public static GameRegion[] GetList()
        {
            return (GameRegion[])s_List.Clone();
        }
        #endregion
    }
}
