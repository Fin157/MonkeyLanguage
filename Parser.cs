namespace MonkeyLanguage;

internal static class Parser
{
    /// <summary>
    /// Detects cycles in lexed code, connects the corresponding cycle starts and ends and
    /// outputs a boolean based on if all cycles are complete (no beginning and ending is missing).
    /// </summary>
    public static bool Parse(List<string> tokens, string cycleStart, string cycleEnd, out Dictionary<int, int> cycleMappings)
    {
        Stack<int> unclosedCycles = [];
        cycleMappings = [];

        // Go through all tokens in the list
        for (int i = 0; i < tokens.Count; i++)
        {
            // Get the token currently being iterated over
            var token = tokens[i];

            // If this token is a cycle start, push its index to the unclosed cycle stack
            if (token == cycleStart)
                unclosedCycles.Push(i);
            // If this token is a cycle end
            else if (token == cycleEnd)
            {
                // If there is no unclosed cycle in the stack, we've got an unopened cycle
                if (!unclosedCycles.TryPop(out int cycleStartIndex))
                {
                    Console.WriteLine("Unopened cycle.");
                    return false;
                }

                // Otherwise pair the cycle start and end together
                cycleMappings.Add(i, cycleStartIndex);
                cycleMappings.Add(cycleStartIndex, i);
            }
        }

        // If the unclosed cycles stack is not empty after the for cycle ended, they are unclosed cycles
        if (unclosedCycles.Count > 0)
        {
            Console.WriteLine("Unclosed cycle");
            return false;
        }

        return true;
    }
}