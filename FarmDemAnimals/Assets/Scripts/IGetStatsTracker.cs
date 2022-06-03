/// <summary>
/// Interface to decouple objects that need to interact with the StatsTracker from whatever object creates it.
/// Especially useful if you're using the C# constructor method to create a StatsTracker but still want the 
/// built-in stats bar to work with it.
/// </summary>
public interface IGetStatsTracker {

    StatsTracker GetStatsTracker();

}