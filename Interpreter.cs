namespace MonkeyLanguage;

internal static class Interpreter
{
    // These fields are just internal strings representing all the possible operations our
    // language can handle. The operation names don't have any effect on the code provided by the user.
    private const string POINTER_RIGHT = "ptrr";
    private const string POINTER_LEFT = "ptrl";
    private const string VALUE_INCREMENT = "incr";
    private const string VALUE_DECREMENT = "decr";
    private const string CONSOLE_OUTPUT = "cout";
    private const string CONSOLE_INPUT = "cinp";
    private const string WHILE_NOT_ZERO_START = "wnzs";
    private const string WHILE_NOT_ZERO_END = "wnze";
    private const string BANANA_REWARD = "rewd";

    // Changing the mappings in this dictionary change the actual keywords this language uses
    // to understand code. The reason why the keywords aren't directly mapped to their corresponding
    // operation is because the system is more flexible when built this way.
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

    private static ushort[] stack;
    private static int pointer;

    public static void Interpret(string program, int stackSize = 30000)
    {
        stack = new ushort[stackSize];
        pointer = 0;

        Tokenizer tokenizer = new(tokenMappings);
        List<string> instructions = tokenizer.Tokenize(program);

        // Make sure to stop if there is a syntax error detected by the parser
        if (!Parser.Parse(instructions, WHILE_NOT_ZERO_START, WHILE_NOT_ZERO_END, out Dictionary<int, int> cycleMappings))
            return;

        // All of the instructions are defined here. In case of demand for a large amount of
        // complicated and long instructions, this class could just define a method for each
        // instruction and call it via the following switch statement.
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
                    Console.WriteLine("Reading input..");
                    stack[pointer] = Console.ReadKey().KeyChar;
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
                    // This instruction has been changed to be a numerical equivalent
                    // of the cout instruction. Instead of a char, it outputs the actual value of
                    // the active cell as a number.
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
