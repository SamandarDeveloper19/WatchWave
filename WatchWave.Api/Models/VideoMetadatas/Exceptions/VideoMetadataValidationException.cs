using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class VideoMetadataValidationException : Xeption
	{
        public VideoMetadataValidationException(Exception innerException)
            :base(message: "VideoMetadata Validation Exception occured, fix the errors and try again.",
                 innerException)
        { }
    }
}
