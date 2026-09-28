using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public interface ICondition
    {
        bool Evaluate(GameContext context);
    }
}