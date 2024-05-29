using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class NotFoundVideoMetadataException : Xeption
	{
        public NotFoundVideoMetadataException(string message)
            :base(message)
        { }
    }
}
