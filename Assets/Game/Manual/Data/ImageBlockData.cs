using System;

namespace ShopGame.Manual.Data
{
    public sealed class ImageBlockData : ManualBlockData
    {
        public string ImagePath { get; }

        public ImageBlockData(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath)) throw new ArgumentException("Image path must not be null, empty, or whitespace.", nameof(imagePath));

            ImagePath = imagePath;
        }
    }
}