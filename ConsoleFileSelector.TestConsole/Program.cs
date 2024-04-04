namespace ConsoleFileSelector.TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrintHeader();
            var fileSelector = new FileSelector(@"C:\", printHeaderCallback: PrintHeader);

            var filePath = fileSelector.SelectFile(@"C:\");

            PrintHeader();
            Console.WriteLine($"You selected: {filePath}");


        }

        static void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("==    Test Console for ConsoleFileSelector    ==");
            Console.WriteLine("================================================");
            Console.WriteLine();
        }

    }
}
