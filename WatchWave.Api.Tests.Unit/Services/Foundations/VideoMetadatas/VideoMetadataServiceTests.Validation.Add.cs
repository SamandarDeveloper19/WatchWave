using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Models.VideoMetadatas.Exceptions;

namespace WatchWave.Api.Tests.Unit.Services.Foundations.VideoMetadatas
{
	public partial class VideoMetadataServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationexceptionOnAddIfVideoMetadataIsNullAndLogErrorAsync()
		{
			//given
			VideoMetadata nullVideoMetadata = null;
			NullVideoMetadataException nullVideoMetadataException = new();
			
			VideoMetadataValidationException expectedvideoMetadataValidationException =
				new(nullVideoMetadataException);
			//when
			ValueTask<VideoMetadata> addVideoMetadataTask = 
				this.videoMetadataService.AddVideoMetadataAsync(nullVideoMetadata);

			//then
			await Assert.ThrowsAsync<VideoMetadataValidationException>(() => 
				addVideoMetadataTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedvideoMetadataValidationException))),
					Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
