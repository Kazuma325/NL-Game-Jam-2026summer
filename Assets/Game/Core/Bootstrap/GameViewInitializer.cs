using System;
using ShopGame.Interaction.View;
using UnityEngine;

namespace ShopGame.Core.Bootstrap
{
    public sealed class GameViewInitializer : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;

        [SerializeField]
        private CraftingStationViewController
            craftingStationViewController;

        [SerializeField]
        private CraftingStationInteractionViewController
            craftingStationInteractionViewController;

        private void Start()
        {
            if (gameBootstrap == null)
            {
                throw new InvalidOperationException(
                    "GameBootstrap reference is not assigned.");
            }

            if (craftingStationViewController == null)
            {
                throw new InvalidOperationException(
                    "CraftingStationViewController reference is not assigned.");
            }

            if (craftingStationInteractionViewController == null)
            {
                throw new InvalidOperationException(
                    "CraftingStationInteractionViewController reference is not assigned.");
            }

            var context = gameBootstrap.Context;

            if (context == null)
            {
                throw new InvalidOperationException(
                    "GameContext has not been created.");
            }

            craftingStationViewController.Initialize(context);

            craftingStationInteractionViewController.Initialize(
                context);
        }
    }
}