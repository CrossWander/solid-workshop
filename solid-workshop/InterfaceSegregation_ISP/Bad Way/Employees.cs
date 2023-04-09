namespace solid_workshop.InterfaceSegregation__ISP_.Bad_Way
{
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
}
