using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class InvalidVideoMetadataException : Xeption
	{
        public InvalidVideoMetadataException(string message)
            :base(message)
        { }
    }
}
