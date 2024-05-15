//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Brokers.Storages
{
    internal partial class StorageBroker
    {
        public DbSet<VideoMetadata> VideoMetadatas { get; set; }

        public async ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<VideoMetadata> videoMetadataEntityEntry =
                await broker.VideoMetadatas.AddAsync(videoMetadata);

            await broker.SaveChangesAsync();

            return videoMetadataEntityEntry.Entity;
        }
    }
}
