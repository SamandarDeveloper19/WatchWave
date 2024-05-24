using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using STX.EFxceptions.Abstractions.Models.Exceptions;
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
		public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
		{
			//given
			VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
			SqlException sqlException = GetSqlException();

			FailedVideoMetadataStorageException failedVideoMetadataStorageException =
				new("Failed Video Metadata storage error occured, please contact support.",
					sqlException);

			VideoMetadataDependencyException expectedVideoMetadataDependencyException = 
				new("Video Metadata dependency exception error occured, please contact support.",
					failedVideoMetadataStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertVideoMetadataAsync(someVideoMetadata))
					.ThrowsAsync(sqlException);

			//when
			ValueTask<VideoMetadata> AddVideoMetadataTask = 
				this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

			VideoMetadataDependencyException actualVideoMetadataDependencyException =
				await Assert.ThrowsAsync<VideoMetadataDependencyException>(AddVideoMetadataTask.AsTask);

			//then
			actualVideoMetadataDependencyException.Should().BeEquivalentTo(expectedVideoMetadataDependencyException);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
					Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(expectedVideoMetadataDependencyException))),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowExceptionAddIfDublicateKeyErrorOccurs()
		{
			//given
			VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
			string someString = GetRandomString();

			var duplicateKeyException = new DuplicateKeyException(someString);

			AlreadyExistVideoMetadataException alreadyExistVideoMetadataException =
				new("Video Metadata already exist, please try again.",
					duplicateKeyException);

			VideoMetadataDependencyValidationException expectedVideoMetadataDependencyValidationException
				= new("Video Metadata dependency error occured. Fix errors and try again.",
					alreadyExistVideoMetadataException);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertVideoMetadataAsync(someVideoMetadata))
					.ThrowsAsync(duplicateKeyException);

			//when
			ValueTask<VideoMetadata> addVideoMetadataTask =
				this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

			VideoMetadataDependencyValidationException actualVideoMetadataDependencyValidationException =
				await Assert.ThrowsAnyAsync<VideoMetadataDependencyValidationException>(addVideoMetadataTask.AsTask);

			//then
			actualVideoMetadataDependencyValidationException.Should().BeEquivalentTo(
				expectedVideoMetadataDependencyValidationException);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
					Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataDependencyValidationException))),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
