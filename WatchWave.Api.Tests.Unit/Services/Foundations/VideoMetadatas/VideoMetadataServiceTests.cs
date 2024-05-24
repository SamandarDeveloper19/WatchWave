//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.Data.SqlClient;
using Moq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
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
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly IVideoMetadataService videoMetadataService;

		public VideoMetadataServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();

			this.videoMetadataService =
				new VideoMetadataService(storageBroker: storageBrokerMock.Object,
				loggingBroker: loggingBrokerMock.Object);
		}

		private static SqlException GetSqlException() =>
			(SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

		private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expecteedException) =>
			actualException => actualException.SameExceptionAs(expecteedException);

		private static VideoMetadata CreateRandomVideoMetadata() =>
			CreateVideoMetadataFiller(date: GetRandomDateTimeOffset).Create();

		private static DateTimeOffset GetRandomDateTimeOffset =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		private static Filler<VideoMetadata> CreateVideoMetadataFiller(DateTimeOffset date)
		{
			var filler = new Filler<VideoMetadata>();
			filler.Setup().OnType<DateTimeOffset>().Use(date);

			return filler;
		}
	}
}
