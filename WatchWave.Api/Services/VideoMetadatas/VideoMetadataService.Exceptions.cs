//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using STX.EFxceptions.Abstractions.Models.Exceptions;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Models.VideoMetadatas.Exceptions;
using Xeptions;

namespace WatchWave.Api.Services.VideoMetadatas
{
	public partial class VideoMetadataService
	{
		private delegate ValueTask<VideoMetadata> ReturningVideoMetadataFunction();
		
		private async ValueTask<VideoMetadata> TryCatch(ReturningVideoMetadataFunction returningVideoMetadataFunction)
		{
			try
			{
				return await returningVideoMetadataFunction();
			}
			catch (NullVideoMetadataException nullVideoMetadataException)
			{
				throw CreateAndLogValidationException(nullVideoMetadataException);
			}
			catch (InvalidVideoMetadataException invalidVideoMetadataException)
			{
				throw CreateAndLogValidationException(invalidVideoMetadataException);
			}
			catch (SqlException sqlException)
			{
				FailedVideoMetadataStorageException failedVideoMetadataStorageException =
					new("Failed Video Metadata storage error occured, please contact support.",
						sqlException);

				throw CreateAndLogCriticalDependencyException(failedVideoMetadataStorageException);
			}
			catch (DuplicateKeyException duplicateKeyException)
			{
				AlreadyExistVideoMetadataException alreadyExistVideoMetadataException
					= new("Video Metadata already exist, please try again.",
						duplicateKeyException);

				throw CreateAndLogDuplicateKeyException(alreadyExistVideoMetadataException);
			}
			catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
			{
				LockedVideoMetadataException lockedVideoMetadataException = new LockedVideoMetadataException(
					"Video Metadata is locked, please try again.", 
						dbUpdateConcurrencyException);

				throw CreateAndLogDependencyValidationException(lockedVideoMetadataException);
			}
			catch (Exception exception)
			{
				FailedVideoMetadataServiceException failedVideoMetadataServiceException =
					new("Unexpected error of Video Metadata occured",
						exception);

				throw CreateAndLogVideoMetadataDependencyServiceErrorOccurs(failedVideoMetadataServiceException);
			}
		}

		private VideoMetadataDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
		{
			var videoMetadataDependencyValidationException = new VideoMetadataDependencyValidationException(
				"Video Metadata dependency error occured. Fix errors and try again.", 
					exception);
			
			this.loggingBroker.LogError(videoMetadataDependencyValidationException);

			return videoMetadataDependencyValidationException;
		}

		private VideoMetadataDependencyServiceException CreateAndLogVideoMetadataDependencyServiceErrorOccurs(Xeption exception)
		{
			VideoMetadataDependencyServiceException videoMetadataDependencyServiceException = 
				new("Unexpected service error occured. Contact support.",
					exception);

			this.loggingBroker.LogError(videoMetadataDependencyServiceException);

			return videoMetadataDependencyServiceException;
		}

		private VideoMetadataDependencyValidationException CreateAndLogDuplicateKeyException(Xeption exception)
		{
			VideoMetadataDependencyValidationException videoMetadataDependencyValidationException =
				new("Video Metadata dependency error occured. Fix errors and try again.",
					exception);
			this.loggingBroker.LogError(videoMetadataDependencyValidationException);

			return videoMetadataDependencyValidationException;
		}

		private VideoMetadataDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
		{
			VideoMetadataDependencyException videoMetadataDependencyException =
				new("Video Metadata dependency exception error occured, please contact support.",
					exception);

			this.loggingBroker.LogCritical(videoMetadataDependencyException);

			return videoMetadataDependencyException;
		}

		private VideoMetadataValidationException CreateAndLogValidationException(Xeption exception)
		{
			var videoMetadataValidationException = new VideoMetadataValidationException(
				"Video Metadata Validation Exception occured, fix the errors and try again.", 
					exception);

			this.loggingBroker.LogError(videoMetadataValidationException);

			return videoMetadataValidationException;
		}
	}
}
