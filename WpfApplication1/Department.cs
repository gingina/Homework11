using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WpfApplication1
{
    public class Department
    {

        #region Конструкторы

        [JsonConstructor]
        public Department(Manager manager, List<Department> insertedDepartments, List<Employee> employees, string name, DateTime dateTime)
        {
            Manager = manager;
            Employees = employees;
            Name = name;
            CreationDate = dateTime;
            InsertedDepartments = insertedDepartments;

            Manager.Department = this;
        }

        public Department(Manager manager, List<Department> insertedDepartments, string name) : this(manager, insertedDepartments, new List<Employee>(), name, DateTime.Now)
        {
            Employees = null;
        }

        public Department(Manager manager, List<Employee> employees, string name) : this(manager, new List<Department>(), employees, name, DateTime.Now)
        {
            InsertedDepartments = null;
        }

        
        #endregion

        #region Поля

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public List<Employee> Employees { get; set; }
        
        public Manager Manager { get; set; }
        
        /// <summary>
        /// Вложенные департаменты
        /// </summary>
        public List<Department> InsertedDepartments { get; set; }
        
        /// <summary>
        /// Дата создания департамента
        /// </summary>
        public DateTime CreationDate { get; set; }
        
        /// <summary>
        /// Наименование департамента 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество сотрудников в этом департаменте
        /// </summary> 
        public int EmployeesCount 
        { 
            get
            {
                if (Employees == null) return 0;
                return Employees.Count;
            }
        }  

        /// <summary>
        /// Количество всех сотрудников под этим департаментом
        /// </summary>
        public int AllEmployeesCount
        {
            get
            {
                if (EmployeesCount != 0)
                {
                    return EmployeesCount;
                }
                else
                {
                    var sum = 0;
                    foreach (var dep in InsertedDepartments)
                    {
                        sum += dep.AllEmployeesCount;
                    }

                    return sum;
                }
            }
        }

        /// <summary>
        /// Количество вложенных департаментов
        /// </summary>
        public int InsertedDepartmentsCount
        {
            get
            {
                if (InsertedDepartments == null) return 0;
                return InsertedDepartments.Count;
            }
        }


        #endregion

        #region Методы

        public void AddIntern(string firstName, int id, int salary)
        {
            Employees.Add(new Intern(firstName, id, salary));
        }

        public void AddWorker(string firstName, int id, int salary)
        {
            Employees.Add(new Worker(firstName, id, salary));
        }

        public void RemoveEmployee(Employee employee)
        {
            Employees.Remove(employee);
        }

        public void RemoveDepartment(Department department)
        {
            InsertedDepartments.Remove(department);
        }

        public void AddDepartment(Department department)
        {
            InsertedDepartments.Add(department);
        }
        #endregion
    }
}