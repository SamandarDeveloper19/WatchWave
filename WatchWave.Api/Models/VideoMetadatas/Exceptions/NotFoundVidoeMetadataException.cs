using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class NotFoundVidoeMetadataException : Xeption
	{
        public NotFoundVidoeMetadataException(string message)
            :base(message)
        { }
    }
}
