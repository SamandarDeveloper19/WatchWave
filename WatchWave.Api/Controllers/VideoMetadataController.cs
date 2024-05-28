//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Services.VideoMetadatas;

namespace WatchWave.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class VideoMetadataController : RESTFulController
	{
		private readonly IVideoMetadataService videoMetadataService;

		public VideoMetadataController(IVideoMetadataService videoMetadataService)
		{
			this.videoMetadataService = videoMetadataService;
		}

		[HttpPost]
		public async ValueTask<ActionResult<VideoMetadata>> PostVideoMetadataAsync(VideoMetadata videoMetadata)
		{
			VideoMetadata postedVideoMetadata = 
				await this.videoMetadataService.AddVideoMetadataAsync(videoMetadata);

			return Created(postedVideoMetadata);
		}
	}
}
