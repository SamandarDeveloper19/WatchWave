namespace WatchWave.Api.Brokers.DateTimes
{
	public interface IDateTimeBroker
	{
		DateTimeOffset GetCurrentDateTimeOffset();
	}
}
