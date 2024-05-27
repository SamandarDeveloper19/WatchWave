namespace WatchWave.Api.Brokers.DateTimes
{
	public class DateTimeBroker : IDateTimeBroker
	{
		public DateTimeOffset GetCurrentDateTimeOffset() =>
			DateTimeOffset.UtcNow;
	}
}
