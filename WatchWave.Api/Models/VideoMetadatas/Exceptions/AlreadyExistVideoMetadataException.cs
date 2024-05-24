//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class AlreadyExistVideoMetadataException : Xeption
	{
        public AlreadyExistVideoMetadataException(string message, Exception innerException)
            :base(message, innerException)
        { }
    }
}
