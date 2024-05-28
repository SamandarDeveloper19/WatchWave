//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using FluentAssertions;
using Moq;
using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
	public partial class VideoMetadataServiceTests
	{
		[Fact]
		public void ShouldReturnVideoMetadatas()
		{
			//given
			IQueryable<VideoMetadata> randomVideoMetadatas = CreateRandomVideoMetadatas();
			IQueryable<VideoMetadata> storageVideoMetadatas = randomVideoMetadatas;
			IQueryable<VideoMetadata> expectedVideoMetadatas = storageVideoMetadatas;

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllVideoMetadatas()).Returns(storageVideoMetadatas);

			//when
			IQueryable<VideoMetadata> actualVideoMetadatas = 
				this.videoMetadataService.RetrieveAllVideoMetadatas();
 
			//then
			actualVideoMetadatas.Should().BeEquivalentTo(expectedVideoMetadatas);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllVideoMetadatas(), Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
