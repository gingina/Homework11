using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Manager : Employee
    {
        public Manager(string firstName, int id) : base(firstName, id, 0)
        {
            Position = "Руководитель";
        }

        /// <summary>
        /// Должность
        /// </summary>
        public override string Position { get; set; }

        /// <summary>
        /// Департамент, которым заведует начальник
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// Зарплата
        /// </summary>
        public new int Salary
        {
            get
            {
                int sum = 0;
                if (Department.Employees != null)
                {
                    foreach (var emp in Department.Employees) sum += emp.Salary;
                    return sum;
                }
                else if (Department.InsertedDepartments != null)
                {
                    foreach (var dep in Department.InsertedDepartments) sum += dep.Manager.Salary;
                    return sum;
                }
                else return 0;
            }
        }

    }
}
