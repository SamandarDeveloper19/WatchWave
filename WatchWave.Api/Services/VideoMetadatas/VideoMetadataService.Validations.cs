using System.Data;
using System.Reflection.Metadata;
using WatchWave.Api.Models.VideoMetadatas;
using WatchWave.Api.Models.VideoMetadatas.Exceptions;

namespace WatchWave.Api.Services.VideoMetadatas
{
	public partial class VideoMetadataService
	{
		private void ValidateVideoMetadataOnAdd(VideoMetadata videoMetadata)
		{
			ValidateVideoMetadataNotNull(videoMetadata);

			Validate(
				(Rule: IsInvalid(videoMetadata.Id), Parameter: nameof(VideoMetadata.Id)),
				(Rule: IsInvalid(videoMetadata.Title), Parameter: nameof(VideoMetadata.Title)),
				(Rule: IsInvalid(videoMetadata.BlobPath), Parameter: nameof(VideoMetadata.BlobPath)),
				(Rule: IsInvalid(videoMetadata.CreatedDate), Parameter: nameof(VideoMetadata.CreatedDate)),
				(Rule: IsInvalid(videoMetadata.UpdatedDate), Parameter: nameof(VideoMetadata.UpdatedDate)));
		}

		private void ValidateVideoMetadataNotNull(VideoMetadata videoMetadata)
		{
			if(videoMetadata is null)
			{
				throw new NullVideoMetadataException();
			}
		}

		private static dynamic IsInvalid(Guid Id) => new
		{
			Condition = Id == Guid.Empty,
			Message = "Id is required"
		};

		private static dynamic IsInvalid(string text) => new
		{
			Condition = string.IsNullOrWhiteSpace(text),
			Message = "Text is required"
		};

		private static dynamic IsInvalid(DateTimeOffset date) => new
		{
			Condition = date == default(DateTimeOffset),
			Message = "Date is required"
		};




		private static void Validate(params (dynamic Rule, string Parameter)[] validations)
		{
			var invalidVideoMetadataException = new InvalidVideoMetadataException(message: "Video Metadata is invalid");

			foreach((dynamic rule, string parameter) in validations)
			{
				if (rule.Condition)
				{
					invalidVideoMetadataException.UpsertDataList(parameter, rule.Message);
				}
			}

			invalidVideoMetadataException.ThrowIfContainsErrors();
		}


	}
}
