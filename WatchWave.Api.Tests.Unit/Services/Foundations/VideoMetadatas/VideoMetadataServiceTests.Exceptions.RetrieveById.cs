//==================================================
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
		public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfsqlErrorsOccursAndLogItAsync()
		{
			//given
			Guid someId = Guid.NewGuid();
			SqlException sqlException = GetSqlException();

			FailedVideoMetadataStorageException failedVideoMetadataStorageException =
				new FailedVideoMetadataStorageException(
					"Failed Video Metadata storage error occured, please contact support.",
						sqlException);

			var expectedVideoMetadataDependencyException =
				new VideoMetadataDependencyException(
					"Video Metadata dependency exception error occured, please contact support.", 
						failedVideoMetadataStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectVideoMetadataByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(sqlException);

			//when
			ValueTask<VideoMetadata> retrieveVideoMetadataByIdTask =
				this.videoMetadataService.RetrieveVideoMetadataByIdAsync(someId);

			VideoMetadataDependencyException actualVideoMetadataDependencyException =
				await Assert.ThrowsAsync<VideoMetadataDependencyException>(
					retrieveVideoMetadataByIdTask.AsTask);

			//then
			actualVideoMetadataDependencyException.Should().BeEquivalentTo(
				expectedVideoMetadataDependencyException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectVideoMetadataByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(expectedVideoMetadataDependencyException))),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
