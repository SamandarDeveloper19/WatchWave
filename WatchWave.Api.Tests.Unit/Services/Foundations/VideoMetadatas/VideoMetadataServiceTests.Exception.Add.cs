﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

		[Fact]
		public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbCurrencyErrorOccursAndLogItAsync()
		{
			//given
			VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
			var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

			var lockedVideoMetadataException =
				new LockedVideoMetadataException("Video Metadata is locked, please try again.",
					dbUpdateConcurrencyException);

			VideoMetadataDependencyValidationException expectedVideoMetadataDependencyValidationException =
				new VideoMetadataDependencyValidationException(
					"Video Metadata dependency error occured. Fix errors and try again.",
						lockedVideoMetadataException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(dbUpdateConcurrencyException);

			//when
			ValueTask<VideoMetadata> addVideoMetadataTask =
				this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

			var actualVideoMetadataDependencyValidationException =
				await Assert.ThrowsAsync<VideoMetadataDependencyValidationException>(addVideoMetadataTask.AsTask);

			//then
			actualVideoMetadataDependencyValidationException.Should()
				.BeEquivalentTo(expectedVideoMetadataDependencyValidationException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(), Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataDependencyValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertVideoMetadataAsync(someVideoMetadata), Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowExceptionOnAddIfServiceErrorOccurs()
		{
			//given
			VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
			var exception = new Exception();

			FailedVideoMetadataServiceException failedVideoMetadataServiceException =
				new("Unexpected error of Video Metadata occured",
					exception);

			VideoMetadataDependencyServiceException expectedVideoMetadataDependencyServiceException =
				new("Unexpected service error occured. Contact support.",
					failedVideoMetadataServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertVideoMetadataAsync(someVideoMetadata))
					.ThrowsAsync(exception);

			//when
			ValueTask<VideoMetadata> AddVideoMetadataTask = 
				this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

			VideoMetadataDependencyServiceException actualVideoMetadataDependencyServiceException =
				await Assert.ThrowsAsync<VideoMetadataDependencyServiceException>(AddVideoMetadataTask.AsTask);

			//then
			actualVideoMetadataDependencyServiceException.Should().BeEquivalentTo(expectedVideoMetadataDependencyServiceException);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataDependencyServiceException))),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}