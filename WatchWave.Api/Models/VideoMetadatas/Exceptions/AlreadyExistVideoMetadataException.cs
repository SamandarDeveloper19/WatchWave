using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class AlreadyExistVideoMetadataException : Xeption
	{
        public AlreadyExistVideoMetadataException(string message, Exception innerException)
            :base(message, innerException)
        { }
    }
}
