using ShopGame.Core.Bootstrap;
using UnityEngine;

public class RoomDebugTest : MonoBehaviour
{
    [SerializeField]
    private GameBootstrap gameBootstrap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(gameBootstrap.Context.Rooms.TryMoveThroughConnection("Shop.ToStorage"));
        Debug.Log(gameBootstrap.Context.World.CurrentRoomId);

        Debug.Log(gameBootstrap.Context.Navigation.MoveRight());
        Debug.Log(gameBootstrap.Context.World.CurrentRoomId);

        gameBootstrap.Context.KnowledgeSelection.SelectOrToggle("wood", "木");
        var result = gameBootstrap.Context.KnowledgeInteraction.TryUseKnowledge(gameBootstrap.Context.CraftingStation);
        Debug.Log(result);
        Debug.Log(gameBootstrap.Context.KnowledgeSlot.HasSelection);
        Debug.Log(string.Join(",", gameBootstrap.Context.CraftingStation.InsertedMaterialIds));

        gameBootstrap.Context.KnowledgeSelection.SelectOrToggle("wood", "木");
        gameBootstrap.Context.KnowledgeInteraction.TryUseKnowledge(gameBootstrap.Context.CraftingStation);
        gameBootstrap.Context.KnowledgeSelection.SelectOrToggle("wood", "木");
        result = gameBootstrap.Context.KnowledgeInteraction.TryUseKnowledge(gameBootstrap.Context.CraftingStation);
        Debug.Log(result);
        Debug.Log(gameBootstrap.Context.KnowledgeSlot.HasSelection);
        Debug.Log(string.Join(",", gameBootstrap.Context.CraftingStation.InsertedMaterialIds));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
