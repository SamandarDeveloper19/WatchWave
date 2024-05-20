using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class NullVideoMetadataException : Xeption
	{
        public NullVideoMetadataException()
            :base(message: "VideoMetadata is null.")
        { }
    }
}
