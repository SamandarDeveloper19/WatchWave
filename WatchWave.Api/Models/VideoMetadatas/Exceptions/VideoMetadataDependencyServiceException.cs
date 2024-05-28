//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class VideoMetadataDependencyServiceException : Xeption
	{
        public VideoMetadataDependencyServiceException(string message, Xeption innerException)
            :base(message, innerException) 
        { }
    }
}
