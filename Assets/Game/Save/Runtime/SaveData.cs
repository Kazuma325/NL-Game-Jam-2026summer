
using System;

namespace ShopGame.Save.Data
{
    [Serializable]
    public sealed class SaveData
    {
        public int saveVersion = 1;

        public int currentProgress;

        public int loopCount;
    }
}
