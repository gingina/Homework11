using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Worker : Employee
    {
        public Worker(string firstName, int id, int salary) : base(firstName, id, salary)
        {
            Position = "Сотрудник";
        }

        public override string Position { get; set; }
    }
}
