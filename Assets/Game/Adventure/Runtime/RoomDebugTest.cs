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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
