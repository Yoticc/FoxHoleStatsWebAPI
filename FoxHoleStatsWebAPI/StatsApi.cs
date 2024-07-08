namespace FoxHoleStatsWebAPI;
public static class StatsApi
{
    public static Stream StaticMapWebp => new WebClient().OpenRead("https://foxholestats.com/images/worldmap_warapi.webp");
    public static Stream DynamicMapWebp => new WebClient().OpenRead("https://foxholestats.com/images/WorldMapControl.webp");
}