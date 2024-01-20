namespace MonkeyLanguage
{
    internal static class Interpreter
    {
        private static ushort[] stack;
        private static int pointer;

        public static void Interpret(string program, int stackSize)
        {
            stack = new ushort[stackSize];
            pointer = 0;

            for (int i = 0; i <= program.Length / 8; i++)
            {
                string operation = program.Substring(i * 8, 7);

                switch (operation)
                {
                    case "Oo. Oo?":
                        ++pointer;
                        break;
                    case "Oo? Oo.":
                        --pointer;
                        break;
                    case "Oo. Oo.":
                        ++stack[pointer];
                        break;
                    case "Oo! Oo!":
                        --stack[pointer];
                        break;
                    case "Oo! Oo.":
                        Console.Write((char)stack[pointer]);
                        break;
                    case "Oo. Oo!":
                        stack[pointer] = (ushort)Console.Read();
                        break;
                    case "Oo? Oo?":
                        Console.WriteLine("Chimp gets a banana. Chimp is happy (Oo!)");
                        break;
                }

                if (pointer < 0)
                {
                    Console.WriteLine("Memory underflow.");
                    return;
                }
                else if (pointer > stackSize - 1)
                {
                    Console.WriteLine("Memory overflow.");
                    return;
                }
            }
        }
    }
}
