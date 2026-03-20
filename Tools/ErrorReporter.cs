namespace NoBadConflicts
{
    internal class ErroR
    {
        static public void Report(string msg)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error Reported: " + msg);
            Console.ForegroundColor = temp;
        }
    }
}
