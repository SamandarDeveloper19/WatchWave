//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.Data.SqlClient;
using Moq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
using WatchWave.Api.Brokers.DateTimes;
using WatchWave.Api.Brokers.Loggings;
using WatchWave.Api.Brokers.Storages;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Services.VideoMetadatas;
using Xeptions;

namespace WatchWave.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
	public partial class VideoMetadataServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly IVideoMetadataService videoMetadataService;

		public VideoMetadataServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();

			this.videoMetadataService =
				new VideoMetadataService(
					storageBroker: storageBrokerMock.Object,
					dateTimeBroker: dateTimeBrokerMock.Object,
					loggingBroker: loggingBrokerMock.Object);
		}

		private static string GetRandomString() =>
			new MnemonicString().GetValue().ToString();

		private static int GetRandomNumber() =>
			new IntRange(min: 2, max: 10).GetValue();

		private static SqlException GetSqlException() =>
			(SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

		private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expecteedException) =>
			actualException => actualException.SameExceptionAs(expecteedException);

		private static VideoMetadata CreateRandomVideoMetadata() =>
			CreateVideoMetadataFiller(date: GetRandomDateTimeOffset()).Create();

		private static IQueryable<VideoMetadata> CreateRandomVideoMetadatas()
		{
			return CreateVideoMetadataFiller(date: GetRandomDateTimeOffset())
				.Create(count: GetRandomNumber()).AsQueryable();
		}

		private static VideoMetadata CreateRandomVideoMetadata(DateTimeOffset dates) =>
				CreateVideoMetadataFiller(date: dates).Create();

		private static DateTimeOffset GetRandomDateTimeOffset() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		private static Filler<VideoMetadata> CreateVideoMetadataFiller(DateTimeOffset date)
		{
			var filler = new Filler<VideoMetadata>();
			filler.Setup().OnType<DateTimeOffset>().Use(date);

			return filler;
		}
	}
}
