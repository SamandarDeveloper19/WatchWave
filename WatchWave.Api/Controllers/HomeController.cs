//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using FluentAssertions.Specialized;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Services.VideoMetadatas;

namespace WatchWave.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : RESTFulController
    {
        private readonly IVideoMetadataService VideoMetadataService;

		public HomeController(IVideoMetadataService videoMetadataService)
		{
			VideoMetadataService = videoMetadataService;
		}

		/*[HttpGet]
        public ActionResult<string> Get() =>
            Ok("Hello Mario, the princes is in another castle");*/

        [HttpPost]
        public async ValueTask<ActionResult<VideoMetadata>> GetVideoMetadata(VideoMetadata videoMetadata) =>
            await this.VideoMetadataService.AddVideoMetadataAsync(videoMetadata);


    }
}
