using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ShopGame.Ending
{
    public class EndingController : MonoBehaviour
    {
        private const string titleSceneName = "Title";

        // Update is called once per frame
        void Update()
        {
            if (Mouse.current == null)
            {
                return;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                SceneManager.LoadScene(titleSceneName);
            }
        }
    }
}
