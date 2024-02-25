namespace Basm;

public class Program
{
    public static void Main(string[] args)
    {
        string filename = string.Empty;
        string fileContents = string.Empty;

#if !DEBUG
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: basm <file>");
            return;
        }

        filename = args[0];
        try
        {
            // Read the contents of the file into a string
            fileContents = File.ReadAllText(filename);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error reading the file: {ex.Message}");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return;
        }
#else
        // For debugging, fill the fileContents
        fileContents = @"
LET a = ""Lollo""
label:
PRINT ""Hello World!""
; GOTO label
";

#endif

        // Create a parser instance and parse the read file contents
        var parser = new Parser();
        parser.ParseProgram(fileContents);
    }
}
