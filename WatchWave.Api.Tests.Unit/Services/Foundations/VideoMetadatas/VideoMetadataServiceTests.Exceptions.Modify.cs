﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Models.VideoMetadatas.Exceptions;

namespace WatchWave.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
            Guid videoMetadataId = someVideoMetadata.Id;
            SqlException sqlException = GetSqlException();

            var failedVideoMetadataStorageException =
                new FailedVideoMetadataStorageException(
                    message: "Failed Video Metadata storage error occured, please contact support.",
                    innerException: sqlException);

            var expectedVideoMetadataDependencyException =
                new VideoMetadataDependencyException(
                    message: "Video Metadata dependency exception error occured, please contact support.",
                    innerException: failedVideoMetadataStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectVideoMetadataByIdAsync(videoMetadataId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<VideoMetadata> modifyVideoMetadataTask =
                this.videoMetadataService.ModifyVideoMetadataAsync(someVideoMetadata);

            VideoMetadataDependencyException actualVideoMetadataDependencyException =
                await Assert.ThrowsAsync<VideoMetadataDependencyException>(
                    modifyVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataDependencyException.Should().BeEquivalentTo(
                expectedVideoMetadataDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectVideoMetadataByIdAsync(videoMetadataId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedVideoMetadataDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
