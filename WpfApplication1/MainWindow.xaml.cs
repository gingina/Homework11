using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Configuration;
using System.Security.Cryptography;
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
                    Tag = new EmployeesAndDepartments(dep.InsertedDepartments, dep.Employees)
                };

                item.Items.Add(null);

                item.Expanded += Folder_Expanded;
                item.Selected += Item_Selected;
                

                Tree.Items.Add(item);
            }
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            //var info = (EmployeesAndDepartments)(item.Tag);

            //DepartmentNameBox.Text = item.Header.ToString();

            //if (item.Parent != null)
            //{
            //    if (item.Parent is TreeView) return;
            //    else if (item.Parent is TreeViewItem) ((TreeViewItem)item.Parent).IsSelected = false;
            //} 

            DepartmentNameBox.Text = ((TreeViewItem)Tree.SelectedItem).Header.ToString();

        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            var obj = (EmployeesAndDepartments)item.Tag;

            if (item.Items.Count != 1 || item.Items[0] != null) return;

            if (obj.Departments != null)
            {
                var list = obj.Departments;
                foreach (var dep in list)
                {
                    var subItem = new TreeViewItem()
                    {
                        Header = dep.Name,
                        Tag = new EmployeesAndDepartments(dep.InsertedDepartments, dep.Employees)
                    };

                    subItem.Items.Add(null);

                    subItem.Expanded += Folder_Expanded;
                    subItem.Selected += Item_Selected;
                    //subItem.MouseDoubleClick += Item_Selected;

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

            var manager1 = new Manager("Boss1", 1, 10000);
            var manager2 = new Manager("Boss2", 2, 100000);
            var manager3 = new Manager("Boss3", 3, 100000);
            var manager4 = new Manager("Boss4", 4, 100000);
            var ceo = new Manager("CEO", 0, 1000000);

            var dep1 = new Department(manager1, employees1, "Dep1");
            var dep2 = new Department(manager2, employees2, "Dep2");
            var dep3 = new Department(manager3, employees3, "Dep3");

            var dep4 = new Department(ceo, new List<Department> { dep2, dep3 }, "dep4");

            var deps = new List<Department> { dep1, dep4 };

            return new Department(manager4, deps, "MainDep");
        }
    }
}