namespace FoxHoleStatsWebAPI;
public record MapStateJson(
    [Prop("regionID")] int RegionID,
    [Prop("wardenCasualties")] int WardenCasualties,
    [Prop("colonialCasualties")] int ColonialCasualties,
    [Prop("wardenRate")] int WardenRate,
    [Prop("colonialRate")] int ColonialRate,
    [Prop("captures")] string Captures,

    /* Used only if RegionID == 0 */
    [Prop("time")] ulong TimeInSeconds,
    [Prop("day")] int Day,
    [Prop("totalPlayers")] int TotalPlayers,
    [Prop("scorchedVictoryTowns")] int ScorchedVictoryTowns
);