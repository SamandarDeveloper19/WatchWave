using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class VideoMetadataDependencyValidationException : Xeption
	{
        public VideoMetadataDependencyValidationException(string message, Xeption innerException)
            :base(message, innerException)
        { }
    }
}
