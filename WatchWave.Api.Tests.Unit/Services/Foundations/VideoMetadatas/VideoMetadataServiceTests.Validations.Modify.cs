﻿//==================================================
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
        public async Task ShouldThrowValidationExceptionOnModifyIfVideoMetadataIsNullAndLogItAsync()
        {
            // given
            VideoMetadata nullVideoMetadata = null;
            var nullVideoMetadataException = new NullVideoMetadataException("Video Metadata is null.");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video Metadata Validation Exception occured, fix the errors and try again.",
                    innerException: nullVideoMetadataException);

            // when
            ValueTask<VideoMetadata> modifyVideoMetadataTask =
                this.videoMetadataService.ModifyVideoMetadataAsync(nullVideoMetadata);

            VideoMetadataValidationException actualVideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(
                    modifyVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
                    Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfVideoMetadataIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            VideoMetadata invalidVideoMetadata = new VideoMetadata
            {
                Title = invalidText
            };

            var invalidVideoMetadataException =
                new InvalidVideoMetadataException(message: "Video Metadata is invalid.");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.Id),
                values: "Id is required.");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.Title),
                values: "Text is required.");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.BlobPath),
                values: "Text is required.");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.CreatedDate),
                values: "Date is required.");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.UpdatedDate),
                values: "Date is required.");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video Metadata Validation Exception occured, fix the errors and try again.",
                    innerException: invalidVideoMetadataException);

            // when
            ValueTask<VideoMetadata> modifyVideoMetadataTask =
                this.videoMetadataService.ModifyVideoMetadataAsync(invalidVideoMetadata);

            VideoMetadataValidationException actualVideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(
                    modifyVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateVideoMetadataAsync(It.IsAny<VideoMetadata>()),
                    Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
