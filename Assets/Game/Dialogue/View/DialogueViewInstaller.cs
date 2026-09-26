using System;
using UnityEngine;
using ShopGame.Core.Bootstrap;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueViewInstaller : MonoBehaviour
    {
        [SerializeField] private GameBootstrap gameBootstrap;
        [SerializeField] private DialogueViewPresenter presenter;

        private void Start()
        {
            if (gameBootstrap == null)
            {
                throw new InvalidOperationException(
                    "Game Bootstrap is not assigned.");
            }

            if (presenter == null)
            {
                throw new InvalidOperationException(
                    "Dialogue View Presenter is not assigned.");
            }

            presenter.Initialize(
                gameBootstrap.Context.DialogueManager,
                gameBootstrap.Context.EventBus);
        }
    }
}