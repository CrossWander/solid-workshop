namespace solid_workshop.Liskov__LSP_.Good_Way
{
    public class AdminDataFileUserFixed : IFileReader, IFileWriter
    {
        public void ReadFile(string filePath)
        {
            // Read File logic    
            Console.WriteLine($"File {filePath} has been read");
        }

        public void WriteFile(string filePath)
        {
            //Write File Logic    
            Console.WriteLine($"File {filePath} has been written");
        }
    }

    public class RegularDataFileUserFixed : IFileReader
    {
        public void ReadFile(string filePath)
        {
            // Read File logic    
            Console.WriteLine($"File {filePath} has been read");
        }
    }
}
