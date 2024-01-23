namespace MonkeyLanguage;

internal sealed class Tokenizer
{
    public Dictionary<string, string> TokenMappings { get; set; }

    public Tokenizer(Dictionary<string, string> tokenMappings)
    {
        List<KeyValuePair<string, string>> mappingsSimplified = [];
        foreach (var tokenMapping in tokenMappings)
            mappingsSimplified.Add(new(tokenMapping.Key.Simplify(), tokenMapping.Value.Simplify()));
        TokenMappings = new(mappingsSimplified);
    }

    public List<string> Tokenize(string code)
    {
        if (TokenMappings.Count == 0)
            return [];

        List<string> tokens = [];

        List<string> matches = [.. TokenMappings.Keys];

        code = code.Simplify();

        int chunkSize = 0;

        foreach (var c in code)
        {
            for (int i = matches.Count - 1; i >= 0 && matches.Count > 0; i--)
            {
                var mapping = matches.ElementAt(i);

                if (mapping[chunkSize] != c)
                    matches.Remove(mapping);
                else if (chunkSize + 1 == mapping.Length)
                {
                    tokens.Add(TokenMappings[mapping]);
                    matches.Clear();
                    continue;
                }
            }

            if (matches.Count == 0)
            {
                chunkSize = 0;
                matches = [.. TokenMappings.Keys];
            }
            else
                chunkSize++;
        }

        return tokens;
    }
}