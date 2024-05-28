//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

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
				(Rule: IsInvalid(videoMetadata.UpdatedDate), Parameter: nameof(VideoMetadata.UpdatedDate)),
				(Rule: IsNotRecent(videoMetadata.CreatedDate), Parameter: nameof(VideoMetadata.CreatedDate)),

				(Rule: IsNotSame(
					firstDate: videoMetadata.CreatedDate,
					secondDate: videoMetadata.UpdatedDate,
					secondDateName: nameof(VideoMetadata.UpdatedDate)),
				Parameter: nameof(VideoMetadata.CreatedDate))
				);
		}

		private void ValidateVideoMetadataNotNull(VideoMetadata videoMetadata)
		{
			if (videoMetadata is null)
			{
				throw new NullVideoMetadataException("Video Metadata is null.");
			}
		}

		private static dynamic IsNotSame(
			DateTimeOffset firstDate,
			DateTimeOffset secondDate,
			string secondDateName) => new
			{
				Condition = firstDate != secondDate,
				Message = $"Date is not same as {secondDateName}"
			};

		private dynamic IsNotRecent(DateTimeOffset date) => new
		{
			Condition = IsDateNotRecent(date),
			Message = "Date is not recent"
		};

		private bool IsDateNotRecent(DateTimeOffset date)
		{
			DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
			TimeSpan timeDifference = currentDateTime.Subtract(date);

			return timeDifference.TotalSeconds is > 60 or < 0;
		}

		private static dynamic IsInvalid(Guid Id) => new
		{
			Condition = Id == Guid.Empty,
			Message = "Id is required."
		};

		private static dynamic IsInvalid(string text) => new
		{
			Condition = string.IsNullOrWhiteSpace(text),
			Message = "Text is required."
		};

		private static dynamic IsInvalid(DateTimeOffset date) => new
		{
			Condition = date == default(DateTimeOffset),
			Message = "Date is required."
		};

		private static void Validate(params (dynamic Rule, string Parameter)[] validations)
		{
			var invalidVideoMetadataException = new InvalidVideoMetadataException(message: "Video Metadata is invalid.");

			foreach ((dynamic rule, string parameter) in validations)
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
