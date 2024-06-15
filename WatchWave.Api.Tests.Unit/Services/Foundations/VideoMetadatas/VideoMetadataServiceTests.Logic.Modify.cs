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
        public async Task ShouldModifyVideoMetadataAsync()
        {
            // given
            VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata();
            VideoMetadata inputVideoMetadata = randomVideoMetadata;
            VideoMetadata storageVideoMetadata = inputVideoMetadata.DeepClone();
            VideoMetadata updatedVideoMetadata = inputVideoMetadata;
            VideoMetadata expectedVideoMetadata = updatedVideoMetadata.DeepClone();
            Guid videoMetadataId = inputVideoMetadata.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectVideoMetadataByIdAsync(videoMetadataId))
                    .ReturnsAsync(storageVideoMetadata);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateVideoMetadataAsync(inputVideoMetadata))
                    .ReturnsAsync(updatedVideoMetadata);

            // when
            VideoMetadata actualVideoMetadata =
                await this.videoMetadataService.ModifyVideoMetadataAsync(inputVideoMetadata);

            // then
            actualVideoMetadata.Should().BeEquivalentTo(expectedVideoMetadata);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectVideoMetadataByIdAsync(videoMetadataId),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateVideoMetadataAsync(inputVideoMetadata),
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
