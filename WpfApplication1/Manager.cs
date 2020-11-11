using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Manager : Employee
    {
        public Manager(Department department, string firstName, int id, int salary) : base(firstName, id, salary)
        {
            Position = "Руководитель";
            Department = department;
        }

        public override string Position { get; set; }

        public Department Department { get; set; }
    }
}
