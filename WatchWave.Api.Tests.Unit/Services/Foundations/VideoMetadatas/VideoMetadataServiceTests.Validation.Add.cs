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
		public async Task ShouldThrowValidationexceptionOnAddIfVideoMetadataIsNullAndLogErrorAsync()
		{
			//given
			VideoMetadata nullVideoMetadata = null;
			NullVideoMetadataException nullVideoMetadataException = new("Video Metadata is null.");

			VideoMetadataValidationException expectedvideoMetadataValidationException =
				new("Video Metadata Validation Exception occured, fix the errors and try again.",
					nullVideoMetadataException);
			//when
			ValueTask<VideoMetadata> addVideoMetadataTask =
				this.videoMetadataService.AddVideoMetadataAsync(nullVideoMetadata);

			//then
			await Assert.ThrowsAsync<VideoMetadataValidationException>(() =>
				addVideoMetadataTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedvideoMetadataValidationException))),
					Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		public async Task ShouldThrowValidationExceptionOnAddIfVideoMetadataIsInvalidDataAndLogItAsync(string invalidData)
		{
			//given
			var invalidVideoMetadata = new VideoMetadata()
			{
				Title = invalidData
			};

			InvalidVideoMetadataException invalidVideoMetadataException = new("Video Metadata is invalid.");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.Id),
				values: "Id is required.");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.Title),
				values: "Text is required.");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.BlobPath),
				values: "Text is required.");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.CreatedDate),
				values: "Date is required.");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.UpdatedDate),
				values: "Date is required.");

			var expectedVideoMetadataValidationException =
				new VideoMetadataValidationException("Video Metadata Validation Exception occured, fix the errors and try again.",
					invalidVideoMetadataException);

			//when
			ValueTask<VideoMetadata> addVideoMetadataTask =
				this.videoMetadataService.AddVideoMetadataAsync(invalidVideoMetadata);

			VideoMetadataValidationException actualVideoMetadataValidationException =
				await Assert.ThrowsAsync<VideoMetadataValidationException>(addVideoMetadataTask.AsTask);


			//then
			actualVideoMetadataValidationException.Should().BeEquivalentTo(expectedVideoMetadataValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
