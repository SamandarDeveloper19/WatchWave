using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class FailedVideoMetadataStorageException : Xeption
	{
        public FailedVideoMetadataStorageException(string message, Exception innerException)
            :base(message, innerException)
        { }
    }
}
