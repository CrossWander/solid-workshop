# Interface Segregation Principle
This principle states that:

 >***No client should be forced to implement a method that it does not use or not relevant to it.***
 
 ## Bad Design
 According to the **Interface Segregation Principle**, the following code is a bad design.
 
 ```C#
public interface IEmployeeTasks
{
    void CreateTask();
    void AssginTask();
    void WorkOnTask();
}

public class ProjectManager : IEmployeeTasks
{
    public void CreateTask()
    {
        Console.WriteLine("Task created.");
    }

    public void AssginTask()
    {
        Console.WriteLine("Task assigned.");
    }

    public void WorkOnTask()
    {
        throw new NotImplementedException();
    }
}

public class Programmer : IEmployeeTasks
{
    public void CreateTask()
    {
        throw new NotImplementedException();
    }

    public void AssginTask()
    {
        throw new NotImplementedException();
    }

    public void WorkOnTask()
    {
        Console.WriteLine("Started working on the task.");
    }
}
 ```
 The problems are:
 
  1. The **ProjectManager** client has been forced to implement `WorkOnTask()` method although **ProjectManager** does not work on task.
  2. The **Programmer** client has been forced to implement `CreateTask()` and `AssginTask()` methods although **Programmer** cannot create and assign task.

## Good Design
If we redesign the above the code as follows, it fixes all the three issues mentioned above:
```C#
// Interfaces
public interface IManager
{
    void CreateTask();
    void AssginTask();
}

public interface IProgrammer
{
    void WorkOnTask();
}

// Clients
public class Manager : IManager
{
    public void CreateTask()
    {
        Console.WriteLine("Task created.");
    }

    public void AssginTask()
    {
        Console.WriteLine("Task assigned.");
    }
}

public class TeamLead : IManager, IProgrammer
{
    public void CreateTask()
    {
        Console.WriteLine("Task created.");
    }

    public void AssginTask()
    {
        Console.WriteLine("Task assigned.");
    }

    public void WorkOnTask()
    {
        Console.WriteLine("Started working on the task.");
    }
}

public class Programmer : IProgrammer
{
    public void WorkOnTask()
    {
        Console.WriteLine("Started working on the task.");
    }
}
```