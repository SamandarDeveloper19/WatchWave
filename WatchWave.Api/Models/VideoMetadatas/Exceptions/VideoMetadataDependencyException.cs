using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class VideoMetadataDependencyException : Xeption
	{
        public VideoMetadataDependencyException(string message, Xeption innerException)
            :base(message, innerException)
        { }
    }
}
