# Liskov Substitution Principle
This principle states that:

 >***A child/sub/derived class should be completely substitutable to its base or parent class.***

 This principle is very similar to the **Interface Segregation Principle**. This principal is also related to the `Polymorphism` which is one of the four pillars of OOP.
 
 ## Bad Design
 According to the **Liskov Substitution Principle**, the following code is a bad design.
 
 ```C#

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


// This breaks the Liskov substituion principle because if we call the WriteFile() method
// then it will throw NotImplementedException.
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

 ```

## Good Design
If we redesign the above code as follows, it will solve the above issue.

```C#
// Interfaces
internal interface IFileReader
{
    void ReadFile(string filePath);
}

internal interface IFileWriter
{
    void WriteFile(string filePath);
}


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
```