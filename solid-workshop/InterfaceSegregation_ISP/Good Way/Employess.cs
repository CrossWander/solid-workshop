namespace solid_workshop.InterfaceSegregation__ISP_.Good_Way
{
    public class ProjectManager : IManager
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
}