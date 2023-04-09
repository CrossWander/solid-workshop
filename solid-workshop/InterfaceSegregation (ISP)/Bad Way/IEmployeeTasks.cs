using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solid_workshop.InterfaceSegregation__ISP_.Bad_Way
{
    public interface IEmployeeTasks
    {
        void CreateTask();
        void AssginTask();
        void WorkOnTask();
    }
}
