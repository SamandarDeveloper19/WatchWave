//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
	public partial class VideoMetadataServiceTests
	{
		[Fact]
		public async Task ShouldAddVideoMetadataAsync()
		{
			//given
			DateTimeOffset randomDate = GetRandomDateTimeOffset();
			VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata();
			VideoMetadata inputVideoMetadata = randomVideoMetadata;
			VideoMetadata storageVideoMetadata = inputVideoMetadata;
			VideoMetadata expectedVideoMetadata = storageVideoMetadata.DeepClone();

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset()).Returns(randomDate);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertVideoMetadataAsync(inputVideoMetadata))
					.ReturnsAsync(expectedVideoMetadata);

			//when
			VideoMetadata actualVideoMetadata =
				await this.videoMetadataService.AddVideoMetadataAsync(inputVideoMetadata);

			//then
			actualVideoMetadata.Should().BeEquivalentTo(expectedVideoMetadata);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(), Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
					Times.Once());

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
