using UnityEngine;

namespace ShopGame.Adventure.View
{
    public class DoorView : MonoBehaviour
    {
        [SerializeField]
        private string connectionId;

        public string ConnectionId => connectionId;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(connectionId)) throw new System.InvalidOperationException($"Connection ID is not assigned on DoorView: {name}");
        }
    }
}