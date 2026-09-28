using UnityEngine;

namespace ShopGame.Adventure.View
{
    public sealed class OwnerView : MonoBehaviour
    {
        [SerializeField]
        private GameObject ownerObject;

        public void SetPresent(bool isPresent)
        {
            if (ownerObject == null)
            {
                return;
            }

            ownerObject.SetActive(isPresent);
        }
    }
}