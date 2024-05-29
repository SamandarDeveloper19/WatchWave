//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using WatchWave.Api.Models.VideoMetadatas;

namespace WatchWave.Api.Services.VideoMetadatas
{
	public interface IVideoMetadataService
	{
		ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata);
		IQueryable<VideoMetadata> RetrieveAllVideoMetadatas();
		ValueTask<VideoMetadata> RetrieveVideoMetadataByIdAsync(Guid videoMetadataId);
	}
}
