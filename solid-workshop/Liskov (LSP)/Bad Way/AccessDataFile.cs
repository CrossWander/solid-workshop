namespace solid_workshop.Liskov__LSP_.Bad_Way
{
    public class AccessDataFile
    {
        public string FilePath { get; set; }

        public virtual void ReadFile()
        {
            // Read File logic  
            Console.WriteLine($"Base File {FilePath} has been read");
        }

        public virtual void WriteFile()
        {
            //Write File Logic  
            Console.WriteLine($"Base File {FilePath} has been written");
        }
    }


    public class AdminDataFileUser : AccessDataFile
    {
        public override void ReadFile()
        {
            // Read File logic  
            Console.WriteLine($"File {FilePath} has been read");
        }

        public override void WriteFile()
        {
            //Write File Logic  
            Console.WriteLine($"File {FilePath} has been written");
        }
    }

    public class RegularDataFileUser : AccessDataFile
    {
        public override void ReadFile()
        {
            // Read File logic  
            Console.WriteLine($"File {FilePath} has been read");
        }
        
        public override void WriteFile()
        {
            throw new NotImplementedException();
        }
    }
}
