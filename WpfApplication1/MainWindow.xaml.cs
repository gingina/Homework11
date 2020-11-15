using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
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

        private TreeViewItem selectedDepartmentItem;
        private TreeViewItem selectedEmployeeItem;

        public MainWindow()
        {
            company = DeserializeJson(dataBasePath);
        }

        public void SerializeJson(string path)
        {
            var serialized = JsonConvert.SerializeObject(company, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
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

        private void DrawTree()
        {
            Tree.Items.Clear();
            var item = new TreeViewItem()
            {
                Header = company.Name,
                Tag = company
            };

            item.Items.Add(null);

            item.Expanded += Folder_Expanded;
            item.Selected += Item_Selected;


            Tree.Items.Add(item);
        }

        private void Tree_Initialized(object sender, EventArgs e)
        {
            //foreach (var dep in company.InsertedDepartments)
            //{
            //    var item = new TreeViewItem()
            //    {
            //        Header = dep.Name,
            //        Tag = dep
            //    };

            //    item.Items.Add(null);

            //    item.Expanded += Folder_Expanded;
            //    item.Selected += Item_Selected;


            //    Tree.Items.Add(item);
            //}
            
            DrawTree();
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)(Tree.SelectedItem);

            if (item.Tag is Department)
            {
                var department = (Department)(item.Tag);
                SetDepartmentValues(department);

                selectedDepartment = department;
                selectedDepartmentItem = item;
            }
            else if (item.Tag is Employee)
            {
                var employee = (Employee)(item.Tag);
                var parent = (Department)(((TreeViewItem)(item.Parent)).Tag);

                selectedDepartment = parent;
                selectedEmployee = employee;

                SetEmployeeValues(employee);
                SetDepartmentValues(parent);
                selectedEmployeeItem = item;
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

        private void DepNameButton_Click(object sender, RoutedEventArgs e)
        {
            selectedDepartment.Name = DepartmentNameBox.Text;
            DrawTree();
        }
        private void ManagerNameButton_Click(object sender, RoutedEventArgs e)
        {
            selectedDepartment.Manager.FirstName = ManagerNameBox.Text;
            DrawTree();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SerializeJson(dataBasePath);
        }

        private void RevertButton_Click(object sender, RoutedEventArgs e)
        {
            company = DeserializeJson(dataBasePath);
            DrawTree();
        }

        private void DeleteDepartment_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (selectedDepartmentItem.Parent is TreeView) MessageBox.Show("Нельзя удалить компанию");
            else
            {
                var parent = (TreeViewItem)(selectedDepartmentItem.Parent);
                var parentDep = (Department)(parent.Tag);
                parentDep.RemoveDepartment(selectedDepartment);
                DrawTree();
            }
        }

        private void DeleteEmployee_ButtonClick(object sender, RoutedEventArgs e)
        {
            var parent = (TreeViewItem)(selectedEmployeeItem.Parent);
            var parentDep = (Department)(parent.Tag);
            parentDep.RemoveEmployee(selectedEmployee);
            DrawTree();
        }

        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            var depName = CreateDepartmentNameBox.Text;
            var managerName = CreateManagerNameBox.Text;

            var selectedItem = Tree.SelectedItem;

            if (String.IsNullOrWhiteSpace(depName) || String.IsNullOrWhiteSpace(managerName))
            {
                MessageBox.Show("Заполни все поля");
            }
            else if (selectedItem == null)
            {
                MessageBox.Show("Выбери департамент, в котором нет сотрудников");
            }
            else
            {
                var tag = ((TreeViewItem)selectedItem).Tag;
                if (tag is Employee || ((Department)tag).EmployeesCount != 0)
                {
                    MessageBox.Show("Выбери департамент, в котором нет сотрудников");
                }
                else
                {
                    var newManager = new Manager(CreateManagerNameBox.Text, company.AllEmployeesCount + 1);
                    var newDep = new Department(newManager, new List<Department>(), new List<Employee>(), CreateDepartmentNameBox.Text, DateTime.Now);
                    ((Department)tag).AddDepartment(newDep);
                    DrawTree();
                }
            }
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {

            var empName = CreateEmployeeNameBox.Text;
            var empSalary = CreateEmployeeSalaryBox.Text;

            var selectedItem = Tree.SelectedItem;

            if (String.IsNullOrWhiteSpace(empName) || String.IsNullOrWhiteSpace(empSalary) || PositionComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Заполни все поля");
            }
            else if (!int.TryParse(empSalary, out int _))
            {
                MessageBox.Show("Зарплата должна быть целочисленной");
            }
            else if (selectedItem == null)
            {
                MessageBox.Show("Выбери департамент, в котором нет внутренних департаментов");
            }
            else
            {
                var tag = ((TreeViewItem)selectedItem).Tag;
                if (tag is Employee || ((Department)tag).InsertedDepartmentsCount != 0)
                {
                    MessageBox.Show("Выбери департамент, в котором нет внутренних департаментов");
                }
                else
                {
                    Employee newEmp;
                    if (PositionComboBox.SelectedIndex == 0)
                    {
                        newEmp = new Worker(empName, company.AllEmployeesCount, int.Parse(empSalary));
                    }
                    else
                    {
                        newEmp = new Intern(empName, company.AllEmployeesCount, int.Parse(empSalary));
                    }
                    ((Department)tag).Employees.Add(newEmp);
                    DrawTree();
                }
            }
        }
    }
}