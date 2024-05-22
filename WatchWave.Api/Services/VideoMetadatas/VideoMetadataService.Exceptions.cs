//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

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
		}

		private VideoMetadataValidationException CreateAndLogValidationException(Xeption exception)
		{
			var videoMetadataValidationException = new VideoMetadataValidationException(
				"Video Metadata Validation Exception occured, fix the errors and try again.", 
					exception);

			this.loggingBroker.LogError(videoMetadataValidationException);
			//this.loggingBroker.LogCritical(videoMetadataValidationException);

			return videoMetadataValidationException;
		}
	}
}
