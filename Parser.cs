namespace MonkeyLanguage;

internal static class Parser
{
    public static bool Parse(List<string> tokens, out Dictionary<int, int> cycleMappings)
    {
        Stack<int> unclosedCycles = [];
        cycleMappings = [];

        for (int i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (token == "wnzs")
                unclosedCycles.Push(i);
            else if (token == "wnze")
            {
                if (!unclosedCycles.TryPop(out int cycleStart))
                {
                    Console.WriteLine("Unopened cycle.");
                    return false;
                }

                cycleMappings.Add(i, cycleStart);
                cycleMappings.Add(cycleStart, i);
            }
        }

        if (unclosedCycles.Count > 0)
        {
            Console.WriteLine("Unclosed cycle");
            return false;
        }

        return true;
    }
}