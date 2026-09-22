using UnityEngine;

namespace ShopGame.Adventure.Data
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Shop Game/Adventure/Room Data")]
    public sealed class RoomData : ScriptableObject
    {
        [SerializeField]
        private string roomId;

        [SerializeField]
        private string displayName;

        [SerializeField]
        private RoomConnectionData[] connections = System.Array.Empty<RoomConnectionData>();

        public string RoomId => roomId;

        public string DisplayName => displayName;

        public RoomConnectionData[] Connections => connections;
    }
}
