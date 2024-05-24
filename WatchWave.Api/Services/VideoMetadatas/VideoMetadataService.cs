﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using WatchWave.Api.Brokers.DateTimes;
using WatchWave.Api.Brokers.Loggings;
using WatchWave.Api.Brokers.Storages;
using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Services.VideoMetadatas
{
	public partial class VideoMetadataService : IVideoMetadataService
	{
		private readonly IStorageBroker storageBroker;
		private readonly ILoggingBroker loggingBroker;
		private readonly IDateTimeBroker dateTimeBroker;

		public VideoMetadataService(
			IStorageBroker storageBroker, 
			ILoggingBroker loggingBroker, 
			IDateTimeBroker dateTimeBroker)
		{
			this.storageBroker = storageBroker;
			this.loggingBroker = loggingBroker;
			this.dateTimeBroker = dateTimeBroker;
		}

		public ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata) =>
			TryCatch(async () =>
			{
				ValidateVideoMetadataOnAdd(videoMetadata);

				return await this.storageBroker.InsertVideoMetadataAsync(videoMetadata);
			});


			
	}
}