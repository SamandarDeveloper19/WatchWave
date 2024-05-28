using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
		{
			//given
			SqlException sqlException = GetSqlException();

			var failedVideoMetadataStorageException =
				new FailedVideoMetadataStorageException(
					"Failed Video Metadata storage error occured, please contact support.",
						sqlException);

			VideoMetadataDependencyException expectedVideoMetadataDependencyException =
				new VideoMetadataDependencyException(
					"Video Metadata dependency exception error occured, please contact support.",
						failedVideoMetadataStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllVideoMetadatas()).Throws(sqlException);

			//when
			Action retrieveAllVideoMetadatasAction = () =>
				this.videoMetadataService.RetrieveAllVideoMetadatas();

			VideoMetadataDependencyException actualVideoMetadataDependencyException =
				Assert.Throws<VideoMetadataDependencyException>(retrieveAllVideoMetadatasAction);

			//then
			actualVideoMetadataDependencyException.Should().BeEquivalentTo(
				expectedVideoMetadataDependencyException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllVideoMetadatas(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(expectedVideoMetadataDependencyException))),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
		{
			//given
			string exceptionMessage = GetRandomString();
			var serviceException = new Exception(exceptionMessage);

			FailedVideoMetadataServiceException failedVideoMetadataServiceException = 
				new FailedVideoMetadataServiceException(
					"Unexpected error of Video Metadata occured.", 
						serviceException);

			VideoMetadataDependencyServiceException expectedVideoMetadataDependencyServiceException =
				new VideoMetadataDependencyServiceException(
					"Unexpected service error occured. Contact support.", 
						failedVideoMetadataServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllVideoMetadatas()).Throws(serviceException);
			
			//when
			Action retrieveAllVideoMetadatasAction = () =>
				this.videoMetadataService.RetrieveAllVideoMetadatas();

			VideoMetadataDependencyServiceException actualVideoMetadataDependencyServiceException =
				Assert.Throws<VideoMetadataDependencyServiceException>(retrieveAllVideoMetadatasAction);

			//then
			actualVideoMetadataDependencyServiceException.Should().BeEquivalentTo(
				expectedVideoMetadataDependencyServiceException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllVideoMetadatas(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedVideoMetadataDependencyServiceException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
