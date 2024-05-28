using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class LockedVideoMetadataException : Xeption
	{
        public LockedVideoMetadataException(string message, Exception innerException)
            :base(message, innerException)
        { }
    }
}
