using System;
using UnityEngine;

namespace ShopGame.Manual.Data
{
    public sealed class StyledTextInlineData : ManualInlineData
    {
        public string Text { get; }
        public string StyleId { get; }

        public StyledTextInlineData(string text, string styleId)
        {
            if(text == null) throw new ArgumentNullException(nameof(text));

            if (string.IsNullOrWhiteSpace(styleId)) throw new ArgumentNullException("Style ID must not be null, empty, or whitespace.", nameof(styleId));

            Text = text;
            StyleId = styleId;
        }
    }
}