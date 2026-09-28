using System;
using System.Collections.Generic;
using ShopGame.Core.Context;
using ShopGame.Event.Data;

namespace ShopGame.Event.Runtime
{
    public sealed class ConditionFactory
    {
        public ICondition Create(
            ConditionData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            switch (data.Type)
            {
                case ConditionType.Group:
                    return CreateGroup(data);

                case ConditionType.Progress:
                    return CreateProgress(data);

                case ConditionType.KnowledgeSlot:
                    return CreateKnowledgeSlot(data);

                case ConditionType.OwnerState:
                    return CreateOwnerState(data);

                case ConditionType.CustomerState:
                    return CreateCustomerState(data);

                default:
                    throw new InvalidOperationException(
                        $"Unsupported condition type: {data.Type}");
            }
        }

        private ICondition CreateGroup(
            ConditionData data)
        {
            List<ICondition> children = new();

            foreach (ConditionData childData in data.Children)
            {
                if (childData == null)
                {
                    throw new InvalidOperationException(
                        "Condition group contains a null child.");
                }

                children.Add(
                    Create(childData));
            }

            ConditionGroup.Operator @operator =
                data.Operator switch
                {
                    ConditionOperator.All =>
                        ConditionGroup.Operator.All,

                    ConditionOperator.Any =>
                        ConditionGroup.Operator.Any,

                    _ => throw new InvalidOperationException(
                        $"Invalid group operator: {data.Operator}")
                };

            return new ConditionGroup(
                @operator,
                children);
        }

        private ICondition CreateProgress(
            ConditionData data)
        {
            ProgressCondition.Operator @operator =
                data.Operator switch
                {
                    ConditionOperator.Equals =>
                        ProgressCondition.Operator.Equals,

                    ConditionOperator.GreaterOrEqual =>
                        ProgressCondition.Operator.GreaterOrEqual,

                    _ => throw new InvalidOperationException(
                        $"Invalid progress operator: {data.Operator}")
                };

            return new ProgressCondition(
                @operator,
                data.IntValue);
        }

        private ICondition CreateKnowledgeSlot(
            ConditionData data)
        {
            KnowledgeSlotCondition.Operator @operator =
                data.Operator switch
                {
                    ConditionOperator.Equals =>
                        KnowledgeSlotCondition.Operator.Equals,

                    ConditionOperator.NotEquals =>
                        KnowledgeSlotCondition.Operator.NotEquals,

                    _ => throw new InvalidOperationException(
                        $"Invalid knowledge slot operator: {data.Operator}")
                };

            return new KnowledgeSlotCondition(
                @operator,
                data.StringValue);
        }

        private ICondition CreateOwnerState(
            ConditionData data)
        {
            return new OwnerStateCondition(
                data.BoolValue);
        }

        private ICondition CreateCustomerState(
    ConditionData data)
        {
            return new CustomerStateCondition(
                data.BoolValue);
        }
    }
}