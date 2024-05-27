//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using System.Runtime.CompilerServices;
using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata);
        IQueryable<VideoMetadata> SelectAllVideoMetadatas();
        ValueTask<VideoMetadata> SelectVideoMetadataByIdAsync(Guid videoMetadataId);
        ValueTask<VideoMetadata> UpdateVideoMetadataAsync(VideoMetadata videoMetadata);
        ValueTask<VideoMetadata> DeleteVideoMetadataAsync(VideoMetadata videoMetadata);
    }
}
