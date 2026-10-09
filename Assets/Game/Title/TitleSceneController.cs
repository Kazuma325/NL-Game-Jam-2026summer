using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShopGame.Title.Runtime
{
    public sealed class TitleSceneController : MonoBehaviour
    {
        private const string GameSceneName = "SampleScene";

        public void StartGame()
        {
            SceneManager.LoadScene(GameSceneName);
        }
    }
}