using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Intern : Employee
    {
        public Intern(string firstName, int id, int salary) : base(firstName, id, salary)
        {
            Position = "Стажер";
        }

        public override string Position { get; set; }
    }
}
