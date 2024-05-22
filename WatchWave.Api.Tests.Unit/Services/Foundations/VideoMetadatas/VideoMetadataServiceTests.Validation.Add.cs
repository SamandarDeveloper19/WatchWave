using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			NullVideoMetadataException nullVideoMetadataException = new();
			
			VideoMetadataValidationException expectedvideoMetadataValidationException =
				new(nullVideoMetadataException);
			//when
			ValueTask<VideoMetadata> addVideoMetadataTask = 
				this.videoMetadataService.AddVideoMetadataAsync(nullVideoMetadata);

			//then
			await Assert.ThrowsAsync<VideoMetadataValidationException>(() => 
				addVideoMetadataTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(expectedvideoMetadataValidationException))),
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
				values: "Id is required");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.Title),
				values: "Title is required");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.BlobPath),
				values: "BlopPath is required");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.CreatedDate),
				values: "Created date is required");

			invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.UpdatedDate),
				values: "Updated date is required");

			var expectedVideoMetadataValidationException =
				new VideoMetadataValidationException(invalidVideoMetadataException);

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
