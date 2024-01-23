namespace MonkeyLanguage;

internal static class Interpreter
{
    private const string POINTER_RIGHT = "ptrr";
    private const string POINTER_LEFT = "ptrl";
    private const string VALUE_INCREMENT = "incr";
    private const string VALUE_DECREMENT = "decr";
    private const string CONSOLE_OUTPUT = "cout";
    private const string CONSOLE_INPUT = "cinp";
    private const string WHILE_NOT_ZERO_START = "wnzs";
    private const string WHILE_NOT_ZERO_END = "wnze";
    private const string BANANA_REWARD = "rewd";

    private static readonly Dictionary<string, string> tokenMappings = new()
    {
        ["Oo. Oo?"] = POINTER_RIGHT,
        ["Oo? Oo."] = POINTER_LEFT,
        ["Oo. Oo."] = VALUE_INCREMENT,
        ["Oo! Oo!"] = VALUE_DECREMENT,
        ["Oo! Oo."] = CONSOLE_OUTPUT,
        ["Oo. Oo!"] = CONSOLE_INPUT,
        ["Oo! Oo?"] = WHILE_NOT_ZERO_START,
        ["Oo? Oo!"] = WHILE_NOT_ZERO_END,
        ["Oo? Oo?"] = BANANA_REWARD
    };

    private static readonly Dictionary<string, string> tokensBf = new()
    {
        [">"] = POINTER_RIGHT,
        ["<"] = POINTER_LEFT,
        ["+"] = VALUE_INCREMENT,
        ["-"] = VALUE_DECREMENT,
        ["."] = CONSOLE_OUTPUT,
        [","] = CONSOLE_INPUT,
        ["["] = WHILE_NOT_ZERO_START,
        ["]"] = WHILE_NOT_ZERO_END,
        ["*"] = BANANA_REWARD
    };

    private static ushort[] stack;
    private static int pointer;

    public static void Interpret(string program, int stackSize = 30000)
    {
        stack = new ushort[stackSize];
        pointer = 0;

        Tokenizer test = new(tokenMappings);
        List<string> instructions = test.Tokenize(program);

        // Make sure to stop if there is a syntax error detected by the parser
        if (!Parser.Parse(instructions, out Dictionary<int, int> cycleMappings))
            return;

        for (int i = 0; i < instructions.Count; i++)
        {
            string instr = instructions[i];

            switch (instr)
            {
                case POINTER_RIGHT:
                    ++pointer;
                    break;
                case POINTER_LEFT:
                    --pointer;
                    break;
                case VALUE_INCREMENT:
                    if (stack[pointer] == ushort.MaxValue)
                        stack[pointer] = 0;
                    else
                        ++stack[pointer];
                    break;
                case VALUE_DECREMENT:
                    if (stack[pointer] == 0)
                        stack[pointer] = ushort.MaxValue;
                    else
                        --stack[pointer];
                    break;
                case CONSOLE_OUTPUT:
                    Console.Write((char)stack[pointer]);
                    break;
                case CONSOLE_INPUT:
                    stack[pointer] = (ushort)Console.Read();
                    break;
                case WHILE_NOT_ZERO_START:
                    if (stack[pointer] == 0)
                        i = cycleMappings[i] + 1;
                    break;
                case WHILE_NOT_ZERO_END:
                    if (stack[pointer] != 0)
                        i = cycleMappings[i];
                    break;
                case BANANA_REWARD:
                    // This instruction, ultimately, does nothing.
                    Console.WriteLine(stack[pointer]);
                    break;
            }

            // Make sure we handle memory errors properly.
            if (pointer < 0)
            {
                Console.WriteLine("Memory underflow.");
                return;
            }
            else if (pointer >= stack.Length)
            {
                Console.WriteLine("Memory overflow.");
                return;
            }
        }
    }
}
