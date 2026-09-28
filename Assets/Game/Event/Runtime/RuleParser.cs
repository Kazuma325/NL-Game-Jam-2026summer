using System;
using System.Collections.Generic;
using ShopGame.Event.Data;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleParser
    {
        public IReadOnlyList<RuleData> Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Rule text must not be null, empty, or whitespace.",
                    nameof(text));
            }

            string[] lines = text.Split(
                new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<RuleData> rules = new();

            int index = 0;

            while (index < lines.Length)
            {
                string line = lines[index].Trim();

                if (string.IsNullOrWhiteSpace(line))
                {
                    index++;
                    continue;
                }

                if (!line.StartsWith("rule "))
                {
                    throw new FormatException(
                        $"Expected 'rule' at line {index + 1}: {line}");
                }

                RuleData rule = ParseRule(lines, ref index);
                rules.Add(rule);
            }

            return rules;
        }

        private RuleData ParseRule(
            string[] lines,
            ref int index)
        {
            string ruleLine = lines[index].Trim();
            string[] ruleTokens = SplitTokens(ruleLine);

            if (ruleTokens.Length != 2)
            {
                throw new FormatException(
                    $"Invalid rule declaration at line {index + 1}: {ruleLine}");
            }

            string ruleId = ruleTokens[1];

            index++;

            if (index >= lines.Length)
            {
                throw new FormatException(
                    $"Rule '{ruleId}' is incomplete.");
            }

            string triggerLine = lines[index].Trim();
            string[] triggerTokens = SplitTokens(triggerLine);

            if (triggerTokens.Length != 3 ||
                triggerTokens[0] != "trigger" ||
                triggerTokens[1] != "interaction")
            {
                throw new FormatException(
                    $"Expected interaction trigger at line {index + 1}: {triggerLine}");
            }

            string targetId = triggerTokens[2];

            index++;

            if (index >= lines.Length)
            {
                throw new FormatException(
                    $"Rule '{ruleId}' is missing priority.");
            }

            string priorityLine = lines[index].Trim();
            string[] priorityTokens = SplitTokens(priorityLine);

            if (priorityTokens.Length != 2 ||
                priorityTokens[0] != "priority")
            {
                throw new FormatException(
                    $"Expected priority at line {index + 1}: {priorityLine}");
            }

            if (!int.TryParse(
                    priorityTokens[1],
                    out int priority))
            {
                throw new FormatException(
                    $"Invalid priority at line {index + 1}: {priorityLine}");
            }

            index++;

            if (index >= lines.Length)
            {
                throw new FormatException(
                    $"Rule '{ruleId}' is missing conditions.");
            }

            string conditionsLine = lines[index].Trim();

            if (conditionsLine != "conditions all" &&
                conditionsLine != "conditions any")
            {
                throw new FormatException(
                    $"Expected 'conditions all' or 'conditions any' at line {index + 1}.");
            }

            ConditionData condition =
                ParseConditionGroup(
                    lines,
                    ref index);

            if (index >= lines.Length ||
                lines[index].Trim() != "effects")
            {
                throw new FormatException(
                    $"Expected 'effects' after conditions of rule '{ruleId}'.");
            }

            index++;

            List<EffectData> effects = new();

            while (index < lines.Length)
            {
                string line = lines[index].Trim();

                if (line == "end")
                {
                    index++;
                    break;
                }

                effects.Add(
                    ParseEffect(
                        line,
                        index + 1));

                index++;
            }

            if (effects.Count == 0)
            {
                throw new FormatException(
                    $"Rule '{ruleId}' must contain at least one effect.");
            }

            return new RuleData(
                ruleId,
                targetId,
                priority,
                condition,
                effects);
        }

        private ConditionData ParseConditionGroup(
    string[] lines,
    ref int index)
        {
            string groupLine =
                lines[index].Trim();

            ConditionOperator @operator =
                groupLine switch
                {
                    "conditions all" =>
                        ConditionOperator.All,

                    "conditions any" =>
                        ConditionOperator.Any,

                    _ => throw new FormatException(
                        $"Invalid condition group at line {index + 1}: {groupLine}")
                };

            index++;

            List<ConditionData> children =
                new List<ConditionData>();

            while (index < lines.Length)
            {
                string line =
                    lines[index].Trim();

                if (line == "end")
                {
                    index++;

                    if (children.Count == 0)
                    {
                        throw new FormatException(
                            $"Condition group at line {index} must contain at least one condition.");
                    }

                    return ConditionData.Group(
                        @operator,
                        children);
                }

                if (line == "conditions all" ||
                    line == "conditions any")
                {
                    children.Add(
                        ParseConditionGroup(
                            lines,
                            ref index));

                    continue;
                }

                children.Add(
                    ParseSimpleCondition(
                        line,
                        index + 1));

                index++;
            }

            throw new FormatException(
                $"Condition group starting at line {index + 1} is missing 'end'.");
        }

        private ConditionData ParseSimpleCondition(
            string line,
            int lineNumber)
        {
            string[] tokens = SplitTokens(line);

            if (tokens.Length == 3 &&
                tokens[0] == "progress")
            {
                if (!int.TryParse(
                        tokens[2],
                        out int value))
                {
                    throw new FormatException(
                        $"Invalid progress value at line {lineNumber}: {line}");
                }

                ConditionOperator @operator =
                    ParseConditionOperator(
                        tokens[1],
                        lineNumber);

                return ConditionData.Progress(
                    @operator,
                    value);
            }

            if (tokens.Length == 3 &&
                tokens[0] == "slot")
            {
                ConditionOperator @operator =
                    ParseConditionOperator(
                        tokens[1],
                        lineNumber);

                return ConditionData.KnowledgeSlot(
                    @operator,
                    tokens[2]);
            }

            if (tokens.Length == 3 &&
                tokens[0] == "owner" &&
                tokens[1] == "present")
            {
                if (!bool.TryParse(
                        tokens[2],
                        out bool isPresent))
                {
                    throw new FormatException(
                        $"Invalid owner presence value at line {lineNumber}: {line}");
                }

                return ConditionData.OwnerState(
                    isPresent);
            }

            if (tokens.Length == 3 &&
    tokens[0] == "customer" &&
    tokens[1] == "present")
            {
                if (!bool.TryParse(
                        tokens[2],
                        out bool isPresent))
                {
                    throw new FormatException(
                        $"Invalid customer presence value at line {lineNumber}: {line}");
                }

                return ConditionData.CustomerState(
                    isPresent);
            }

            throw new FormatException(
                $"Unknown condition at line {lineNumber}: {line}");
        }

        private EffectData ParseEffect(
            string line,
            int lineNumber)
        {
            string[] tokens = SplitTokens(line);

            if (tokens.Length == 2 &&
                tokens[0] == "dialogue")
            {
                return EffectData.StartDialogue(
                    tokens[1]);
            }

            if (tokens.Length == 3 &&
                tokens[0] == "progress" &&
                tokens[1] == "advance")
            {
                if (!int.TryParse(
                        tokens[2],
                        out int targetProgress))
                {
                    throw new FormatException(
                        $"Invalid progress target at line {lineNumber}: {line}");
                }

                return EffectData.AdvanceProgress(
                    targetProgress);
            }

            if (tokens.Length == 2 &&
                tokens[0] == "loop" &&
                tokens[1] == "end_after_dialogue")
            {
                return EffectData.RequestLoopEndAfterDialogue();
            }

            throw new FormatException(
                $"Unknown effect at line {lineNumber}: {line}");
        }

        private static ConditionOperator ParseConditionOperator(
            string value,
            int lineNumber)
        {
            return value switch
            {
                "equals" =>
                    ConditionOperator.Equals,

                "greater_or_equal" =>
                    ConditionOperator.GreaterOrEqual,

                "not_equals" =>
                    ConditionOperator.NotEquals,

                _ => throw new FormatException(
                    $"Unknown condition operator at line {lineNumber}: {value}")
            };
        }

        private static string[] SplitTokens(
            string line)
        {
            return line.Split(
                new[] { ' ', '\t' },
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}