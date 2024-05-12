﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.EntityFrameworkCore;
using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Brokers.Storages
{
    internal partial class StorageBroker
    {
        public DbSet<VideoMetadata> VideoMetadatas { get; set; }
    }
}
