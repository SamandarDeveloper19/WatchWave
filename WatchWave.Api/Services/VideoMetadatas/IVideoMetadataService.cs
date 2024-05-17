using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Services.VideoMetadatas
{
	public interface IVideoMetadataService
	{
		ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata);
	}
}
