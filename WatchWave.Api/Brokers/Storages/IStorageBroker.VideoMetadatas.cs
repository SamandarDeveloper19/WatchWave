using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Brokers.Storages
{
    internal partial interface IStorageBroker
    {
        ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata);
    }
}
