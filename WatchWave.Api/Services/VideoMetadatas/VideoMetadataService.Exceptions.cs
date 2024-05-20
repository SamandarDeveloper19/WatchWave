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
		}

		private VideoMetadataValidationException CreateAndLogValidationException(Xeption exception)
		{
			var videoMetadataValidationException = new VideoMetadataValidationException(exception);
			this.loggingBroker.LogCritical(videoMetadataValidationException);
			return videoMetadataValidationException;
		}
	}
}
