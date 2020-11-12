using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class EmployeesAndDepartments
    {
        public EmployeesAndDepartments(List<Department> deps, List<Employee> emps)
        {
            Departments = deps;
            Employees = emps;
        }

        public List<Department> Departments { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
