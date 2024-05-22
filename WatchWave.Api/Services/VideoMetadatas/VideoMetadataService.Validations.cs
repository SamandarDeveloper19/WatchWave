using System.Data;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Models.VideoMetadatas.Exceptions;

namespace WatchWave.Api.Services.VideoMetadatas
{
	public partial class VideoMetadataService
	{


		private void ValidateVideoMetadataNotNull(VideoMetadata videoMetadata)
		{
			if(videoMetadata is null)
			{
				throw new NullVideoMetadataException();
			}
		}

		
	}
}
