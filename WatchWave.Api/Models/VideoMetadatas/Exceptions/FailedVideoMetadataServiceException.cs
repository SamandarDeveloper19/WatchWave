using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class FailedVideoMetadataServiceException : Xeption
	{
        public FailedVideoMetadataServiceException(string message, Exception innerException)
            :base(message, innerException)
        { }
    }
}
