﻿using WatchWave.Api.Brokers.Loggings;
using WatchWave.Api.Brokers.Storages;
using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Services.VideoMetadatas
{
	public partial class VideoMetadataService : IVideoMetadataService
	{
		private readonly IStorageBroker storageBroker;
		private readonly ILoggingBroker loggingBroker;

		public VideoMetadataService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
		{
			this.storageBroker = storageBroker;
			this.loggingBroker = loggingBroker;
		}

		public ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata) =>
			TryCatch(async () =>
			{
				ValidateVideoMetadataNotNull(videoMetadata);

				return await this.storageBroker.InsertVideoMetadataAsync(videoMetadata);
			});


			
	}
}
