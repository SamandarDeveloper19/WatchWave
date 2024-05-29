//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using FluentAssertions;
using Moq;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Models.VideoMetadatas.Exceptions;

namespace WatchWave.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
	public partial class VideoMetadataServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
		{
			//given
			var invalidVideoMetadataId = Guid.Empty;

			var invalidVideoMetadataException =
				new InvalidVideoMetadataException("Video Metadata is invalid.");

			invalidVideoMetadataException.AddData(
				key: nameof(VideoMetadata.Id),
				values: "Id is required.");

			var expectedVideoMetadataValidationException = 
				new VideoMetadataValidationException(
					"Video Metadata Validation Exception occured, fix the errors and try again.",
						invalidVideoMetadataException);

			//when
			ValueTask<VideoMetadata> retrieveByIdVideoMetadataTask =
				this.videoMetadataService.RetrieveVideoMetadataByIdAsync(invalidVideoMetadataId);

			VideoMetadataValidationException actualVideoMetadataValidationException =
				await Assert.ThrowsAsync<VideoMetadataValidationException>(
					retrieveByIdVideoMetadataTask.AsTask);

			//then
			actualVideoMetadataValidationException.Should().BeEquivalentTo(
				expectedVideoMetadataValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectVideoMetadataByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
