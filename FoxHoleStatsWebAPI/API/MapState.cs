using Newtonsoft.Json.Linq;

namespace FoxHoleStatsWebAPI;
public record MapState(ulong TimeInSeconds, int Day, int TotalPlayers, int ScorchedVictoryTowns, int WardenCasualties, int ColonialCasualties, int WardenRate, int ColonialRate, string Captures, List<HexState> HexStates, int WebUsersOnline)
{
    public static MapState FromJson(object[] rawJson)
    {   
        (MapStateJson[] states, int webUsersOnline) = ((rawJson[0] as JArray)!.ToObject<MapStateJson[]>()!, (int)(long)rawJson[1]);        

        var map = states[0];
        var hexes = states.Select(state => new HexState(state.RegionID, state.WardenCasualties, state.ColonialCasualties, state.WardenRate, state.ColonialRate, state.Captures)).ToList();

        return new(map.TimeInSeconds, map.Day, map.TotalPlayers, map.ScorchedVictoryTowns, map.WardenCasualties, map.ColonialCasualties, map.WardenRate, map.ColonialRate, map.Captures, hexes, webUsersOnline);
    }
}

public record HexState(int RegionID, int WardenCasualties, int ColonialCasualties, int WardenRate, int ColonialRate, string Captures);