using MonkeyLanguage;

internal class Program
{
    public static ushort[] stack;

    // Cmd parameter 1: path leading to file with Chimpfuck program
    // Cmd parameter 2: stack size (30 000 if missing)
    private static void Main(string[] args)
    {
        //Check if a file path was actually fed into the program to prevent exceptions
        //if (args.Length < 2 || File.Exists(args[0]) || !int.TryParse(args[1], out int stackSize))
        //{
        //    Console.WriteLine("Invalid command line arguments.");
        //    return;
        //}

        //Interpreter.Interpret(File.ReadAllText(args[0]), stackSize);
        Interpreter.Interpret(File.ReadAllText(@"D:\Coding\Seminary\MonkeyLanguage\NumbersToThousand.txt"));
    }
}