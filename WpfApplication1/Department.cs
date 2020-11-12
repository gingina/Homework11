using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WpfApplication1
{
    public class Department
    {

        #region Конструкторы

        public Department(List<Department> insertedDepartments, List<Employee> employees, string name, DateTime dateTime)
        {
            Employees = employees;
            Name = name;
            CreationDate = dateTime;
            EmployeesCount = 0;
            InsertedDepartments = insertedDepartments;
        }

        public Department(List<Department> insertedDepartments, string name) : this(insertedDepartments, new List<Employee>(), name, DateTime.Now)
        {
            Employees = null;
        }

        public Department(List<Employee> employees, string name) : this(new List<Department>(), employees, name, DateTime.Now)
        {
            InsertedDepartments = null;
        }

        
        #endregion

        #region Поля

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public List<Employee> Employees { get; set; }
        
        public Employee Manager { get; set; }
        
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
        /// Количество сотрудников
        /// </summary>
        public int EmployeesCount { get; set; }  

        #endregion

        #region Методы

        public void AddIntern(string firstName, int id, int salary)
        {
            Employees.Add(new Intern(firstName, id, salary));
        }

        public void AddEmployee(string firstName, int id, int salary)
        {
            Employees.Add(new Intern(firstName, id, salary));
        }

        public void ChangeName(string newName)
        {
            Name = newName;
        }

        public void DeleteEmployee(int id)
        {
            
        }
        #endregion
    }
}