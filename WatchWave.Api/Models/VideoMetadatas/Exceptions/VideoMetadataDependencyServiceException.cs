using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class VideoMetadataDependencyServiceException : Xeption
	{
        public VideoMetadataDependencyServiceException(string message, Xeption innerException)
            :base(message, innerException) 
        { }
    }
}
