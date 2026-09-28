using System;
using System.Collections.Generic;

namespace ShopGame.Event.Data
{
    public enum ConditionType
    {
        Group,
        Progress,
        KnowledgeSlot,
        OwnerState,
        CustomerState
    }

    public enum ConditionOperator
    {
        All,
        Any,
        Equals,
        GreaterOrEqual,
        NotEquals
    }

    public sealed class ConditionData
    {
        public ConditionType Type { get; }

        public ConditionOperator Operator { get; }

        public int IntValue { get; }

        public string StringValue { get; }

        public bool BoolValue { get; }

        public IReadOnlyList<ConditionData> Children { get; }

        private ConditionData(
            ConditionType type,
            ConditionOperator @operator,
            int intValue,
            string stringValue,
            bool boolValue,
            IReadOnlyList<ConditionData> children)
        {
            Type = type;
            Operator = @operator;
            IntValue = intValue;
            StringValue = stringValue;
            BoolValue = boolValue;
            Children = children;
        }

        public static ConditionData Group(
            ConditionOperator @operator,
            IReadOnlyList<ConditionData> children)
        {
            if (@operator != ConditionOperator.All &&
                @operator != ConditionOperator.Any)
            {
                throw new ArgumentException(
                    "Condition group operator must be All or Any.",
                    nameof(@operator));
            }

            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }

            if (children.Count == 0)
            {
                throw new ArgumentException(
                    "Condition group must contain at least one child.",
                    nameof(children));
            }

            return new ConditionData(
                ConditionType.Group,
                @operator,
                0,
                null,
                false,
                children);
        }

        public static ConditionData Progress(
            ConditionOperator @operator,
            int value)
        {
            if (@operator != ConditionOperator.Equals &&
                @operator != ConditionOperator.GreaterOrEqual)
            {
                throw new ArgumentException(
                    "Progress condition operator is invalid.",
                    nameof(@operator));
            }

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Progress value must not be negative.");
            }

            return new ConditionData(
                ConditionType.Progress,
                @operator,
                value,
                null,
                false,
                null);
        }

        public static ConditionData KnowledgeSlot(
            ConditionOperator @operator,
            string knowledgeId)
        {
            if (@operator != ConditionOperator.Equals &&
                @operator != ConditionOperator.NotEquals)
            {
                throw new ArgumentException(
                    "Knowledge slot condition operator is invalid.",
                    nameof(@operator));
            }

            if (string.IsNullOrWhiteSpace(knowledgeId))
            {
                throw new ArgumentException(
                    "Knowledge ID must not be null, empty, or whitespace.",
                    nameof(knowledgeId));
            }

            return new ConditionData(
                ConditionType.KnowledgeSlot,
                @operator,
                0,
                knowledgeId,
                false,
                null);
        }

        public static ConditionData OwnerState(
            bool isPresent)
        {
            return new ConditionData(
                ConditionType.OwnerState,
                ConditionOperator.Equals,
                0,
                null,
                isPresent,
                null);
        }

        public static ConditionData CustomerState(
    bool isPresent)
        {
            return new ConditionData(
                ConditionType.CustomerState,
                ConditionOperator.Equals,
                0,
                null,
                isPresent,
                null);
        }
    }
}