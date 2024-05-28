//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Xeptions;

namespace WatchWave.Api.Models.VideoMetadatas.Exceptions
{
	public class VideoMetadataDependencyException : Xeption
	{
        public VideoMetadataDependencyException(string message, Xeption innerException)
            :base(message, innerException)
        { }
    }
}
