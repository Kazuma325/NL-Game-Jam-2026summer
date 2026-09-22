using System;
using ShopGame.Adventure.Data;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Adventure.View
{
    public sealed class RoomView : MonoBehaviour
    {
        [SerializeField]
        private Text roomNameText;

        public void DisplayRoom(RoomData roomData)
        {
            if (roomData == null)
            {
                throw new ArgumentNullException(nameof(roomData));
            }

            if (roomNameText == null)
            {
                return;
            }

            roomNameText.text = roomData.DisplayName;
        }
    }
}
