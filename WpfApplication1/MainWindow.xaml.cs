using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Configuration;
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

        public MainWindow()
        {
            var employees1 = new List<Employee>();
            var employees2 = new List<Employee>();
            var employees3 = new List<Employee>();

            for (int i = 0; i < 5; i++) employees1.Add(new Worker($"1Worker {i + 1}", i, (i + 1) * 1000));
            for (int i = 0; i < 5; i++) employees2.Add(new Worker($"2Worker {i + 1}", i, (i + 1) * 1000));
            for (int i = 0; i < 5; i++) employees3.Add(new Worker($"3Worker {i + 1}", i, (i + 1) * 1000));

            var dep1 = new Department(employees1, "Dep1");
            var dep2 = new Department(employees2, "Dep2");
            var dep3 = new Department(employees3, "Dep3");

            var dep4 = new Department(new List<Department> { dep2, dep3 }, "dep4");

            var deps = new List<Department> { dep1, dep4 };

            company = new Department(deps, "MainDep");

            var manager1 = new Manager(company, "manager1", 0, 10000);

            company.Manager = manager1;

            //SerializeJson("Data.json");
        }

        public void SerializeJson(string path)
        {
            var serialized = JsonConvert.SerializeObject(company, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            });

            File.WriteAllText(path, serialized);
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

                Tree.Items.Add(item);
            }
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
                }
            }
        }
    }
}