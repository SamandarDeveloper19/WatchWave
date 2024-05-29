//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Models.VideoMetadatas.Exceptions;
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
			try
			{
				VideoMetadata postedVideoMetadata =
				await this.videoMetadataService.AddVideoMetadataAsync(videoMetadata);

				return Created(postedVideoMetadata);
			}
			catch (VideoMetadataValidationException videoMetadataValidationException)
			{
				return BadRequest(videoMetadataValidationException.InnerException);
			}
			catch(VideoMetadataDependencyValidationException videoMetadataDependencyValidationException)
				when(videoMetadataDependencyValidationException.InnerException is 
					AlreadyExistsVideoMetadataException)
			{
				return Conflict(videoMetadataDependencyValidationException.InnerException);
			}
			catch(VideoMetadataDependencyException videoMetadataDependencyException)
			{
				return InternalServerError(videoMetadataDependencyException);
			}
			catch(VideoMetadataDependencyServiceException videoMetadataDependencyServiceException)
			{
				return InternalServerError(videoMetadataDependencyServiceException);
			}
		}

		[HttpGet]
		public ActionResult<IQueryable<VideoMetadata>> GetAllVideoMetadatas()
		{
			try
			{
				IQueryable<VideoMetadata> gettingAllVideoMetadatas =
				this.videoMetadataService.RetrieveAllVideoMetadatas();

				return Ok(gettingAllVideoMetadatas);
			}
			catch (VideoMetadataDependencyException videoMetadataDependencyException)
			{
				return InternalServerError(videoMetadataDependencyException);
			}
			catch(VideoMetadataDependencyServiceException videoMetadataDependencyServiceException)
			{
				return InternalServerError(videoMetadataDependencyServiceException);
			}
		}
	}
}
