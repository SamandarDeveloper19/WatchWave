﻿//==================================================
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
        public async Task ShouldRemoveVideoMetadataByIdAsync()
        {
            // given
            Guid videoMetadataId = Guid.NewGuid();
            VideoMetadata storageVideoMetadata = CreateRandomVideoMetadata();
            VideoMetadata expectedInputVideoMetadata = storageVideoMetadata;
            VideoMetadata deletedVideoMetadata = expectedInputVideoMetadata;
            VideoMetadata expectedVideoMetadata = deletedVideoMetadata.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectVideoMetadataByIdAsync(videoMetadataId))
                    .ReturnsAsync(storageVideoMetadata);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteVideoMetadataAsync(expectedInputVideoMetadata))
                    .ReturnsAsync(deletedVideoMetadata);

            // when
            VideoMetadata actualVideoMetadata =
                await this.videoMetadataService.RemoveVideoMetadataByIdAsync(videoMetadataId);

            // then
            actualVideoMetadata.Should().BeEquivalentTo(expectedVideoMetadata);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectVideoMetadataByIdAsync(videoMetadataId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteVideoMetadataAsync(expectedInputVideoMetadata),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
