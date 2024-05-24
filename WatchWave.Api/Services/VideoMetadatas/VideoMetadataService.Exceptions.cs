//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.Data.SqlClient;
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
			catch(InvalidVideoMetadataException invalidVideoMetadataException)
			{
				throw CreateAndLogValidationException(invalidVideoMetadataException);
			}
			catch(SqlException sqlException)
			{
				FailedVideoMetadataStorageException failedVideoMetadataStorageException =
					new("Failed Video Metadata storage error occured, please contact support.",
						sqlException);

				throw CreateAndLogCriticalDependencyException(failedVideoMetadataStorageException);
			}
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
