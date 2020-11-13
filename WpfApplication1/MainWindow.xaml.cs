using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Configuration;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Department company;
        public string dataBasePath = "Data.json";

        private Department selectedDepartment;
        private Employee selectedEmployee;

        public MainWindow()
        {
            company = CreateDepartment();
            //company = DeserializeJson(dataBasePath);

            SerializeJson(dataBasePath);
        }

        public void SerializeJson(string path)
        {
            var serialized = JsonConvert.SerializeObject(company, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            File.WriteAllText(path, serialized);
        }

        public Department DeserializeJson(string path)
        {
            var text = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<Department>(text, new JsonSerializerSettings 
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        private void Tree_Initialized(object sender, EventArgs e)
        {
            foreach (var dep in company.InsertedDepartments)
            {
                var item = new TreeViewItem()
                {
                    Header = dep.Name,
                    Tag = dep
                };

                item.Items.Add(null);

                item.Expanded += Folder_Expanded;
                item.Selected += Item_Selected;
                

                Tree.Items.Add(item);
            }
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)(Tree.SelectedItem);

            if (item.Tag is Department)
            {
                var department = (Department)(item.Tag);
                SetDepartmentValues(department);

                selectedDepartment = department;
            }
            else if (item.Tag is Employee)
            {
                var employee = (Employee)(item.Tag);
                var parent = (Department)(((TreeViewItem)(item.Parent)).Tag);

                selectedDepartment = parent;
                selectedEmployee = employee;

                SetEmployeeValues(employee);
                SetDepartmentValues(parent);
            }
        }

        private void SetDepartmentValues(Department department)
        {
            DepartmentNameBox.Text = department.Name;
            InsertedDepartmentsCountBox.Text = department.InsertedDepartmentsCount.ToString();
            EmloyeesCountBox.Text = department.EmployeesCount.ToString();
            DepartmentCreationDateBox.Text = department.CreationDate.ToString();
            ManagerNameBox.Text = department.Manager.FirstName;
            ManagerSalaryBox.Text = department.Manager.Salary.ToString();
        }

        private void SetEmployeeValues(Employee employee)
        {
            EmployeeNameBox.Text = employee.FirstName;
            EmployeePositionBox.Text = employee.Position;
            EmployeeSalaryBox.Text = employee.Salary.ToString();
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            var obj = (Department)item.Tag;

            if (item.Items.Count != 1 || item.Items[0] != null) return;

            if (obj.InsertedDepartments != null)
            {
                var list = obj.InsertedDepartments;
                foreach (var dep in list)
                {
                    var subItem = new TreeViewItem()
                    {
                        Header = dep.Name,
                        Tag = dep
                    };

                    subItem.Items.Add(null);

                    subItem.Expanded += Folder_Expanded;
                    subItem.Selected += Item_Selected;

                    item.Items.Add(subItem);
                }
            }
            if (obj.Employees != null)
            {
                var list = obj.Employees;
                foreach (var emp in list)
                {
                    var subItem = new TreeViewItem()
                    {
                        Header = emp.FirstName,
                        Tag = emp
                    };

                    item.Items.Add(subItem);
                    subItem.Selected += Item_Selected;
                }
            }
        }

        private Department CreateDepartment()
        {
            var employees1 = new List<Employee>();
            var employees2 = new List<Employee>();
            var employees3 = new List<Employee>();

            for (int i = 0; i < 5; i++) employees1.Add(new Worker($"1Worker {i + 1}", i, (i + 1) * 1000));
            for (int i = 0; i < 5; i++) employees2.Add(new Worker($"2Worker {i + 1}", i, (i + 1) * 1000));
            for (int i = 0; i < 5; i++) employees3.Add(new Worker($"3Worker {i + 1}", i, (i + 1) * 1000));

            var manager1 = new Manager("Boss1", 1);
            var manager2 = new Manager("Boss2", 2);
            var manager3 = new Manager("Boss3", 3);
            var manager4 = new Manager("Boss4", 4);
            var ceo = new Manager("CEO", 0);

            var dep1 = new Department(manager1, employees1, "Dep1");
            var dep2 = new Department(manager2, employees2, "Dep2");
            var dep3 = new Department(manager3, employees3, "Dep3");

            var dep4 = new Department(ceo, new List<Department> { dep2, dep3 }, "dep4");

            var deps = new List<Department> { dep1, dep4 };

            return new Department(manager4, deps, "MainDep");
        }
    }
}