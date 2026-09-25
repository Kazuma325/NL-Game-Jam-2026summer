using System;
using UnityEngine;

namespace ShopGame.Manual.Data
{
    public sealed class TextInlineData : ManualInlineData
    {
        public string Text { get; }

        public TextInlineData(string text)
        {
            if(text == null) throw new ArgumentNullException(nameof(text));

            Text = text;
        }
    }
}