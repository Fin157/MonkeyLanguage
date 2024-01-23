using MonkeyLanguage;

internal class Program
{
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
        Console.Write("Code file path: ");
        string codeFilePath = Console.ReadLine();
        Console.WriteLine("Stack size: ");
        int stackSize = int.Parse(Console.ReadLine());
        Interpreter.Interpret(File.ReadAllText(codeFilePath), stackSize);
    }
}