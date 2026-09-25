namespace ShopGame.Interaction.Runtime
{
    public interface IKnowledgeUsable
    {
        KnowledgeUseResult TryUseKnowledge(string knowledgeId);
    }
}
