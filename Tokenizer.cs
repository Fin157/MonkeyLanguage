namespace MonkeyLanguage;

internal sealed class Tokenizer
{
    public Dictionary<string, string> TokenMappings { get; set; }

    public Tokenizer(Dictionary<string, string> tokenMappings)
    {
        // Process given token mappings to remove any spaces and case sensitivity.
        // There is plenty of space for improvement such as duplicate mapping detection but I deemed
        // it unnecessary and way too heavyweight for this kind of project.
        List<KeyValuePair<string, string>> mappingsSimplified = [];
        foreach (var tokenMapping in tokenMappings)
            mappingsSimplified.Add(new(tokenMapping.Key.Simplify(), tokenMapping.Value.Simplify()));
        TokenMappings = new(mappingsSimplified);
    }

    public List<string> Tokenize(string code)
    {
        // Return an empty list immediately if no token mappings have been given
        if (TokenMappings.Count == 0)
            return [];

        List<string> tokens = [];

        List<string> matches = [.. TokenMappings.Keys];

        code = code.Simplify();

        int chunkSize = 0;

        // Read the whole code in the string and process it one by one character
        foreach (var c in code)
        {
            // For every still matching mapping
            for (int i = matches.Count - 1; i >= 0 && matches.Count > 0; i--)
            {
                // Compare the i-th character of the mapping and the character read from the code
                var mapping = matches.ElementAt(i);

                // If the characters don't match, this mapping is not a match anymore and we, thus,
                // remove it from matches
                if (mapping[chunkSize] != c)
                    matches.Remove(mapping);
                // If we've reached the end of the current mapping (100% match), this mapping is the one
                else if (chunkSize + 1 == mapping.Length)
                {
                    tokens.Add(TokenMappings[mapping]);
                    matches.Clear();
                    continue;
                }
            }

            // If there are no matching mappings left, the currently checked chunk of our code
            // is not a token --> we just start over with an empty chunk and all mappings in matching
            if (matches.Count == 0)
            {
                chunkSize = 0;
                matches = [.. TokenMappings.Keys];
            }
            // Otherwise just increase the chunk size by 1
            else
                chunkSize++;
        }

        return tokens;
    }
}