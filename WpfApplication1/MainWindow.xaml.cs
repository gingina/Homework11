using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Configuration;
using System.Windows;
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

            for (int i = 0; i < 5; i++) employees1.Add(new Worker($"1Worker {i + 1}", i, (i + 1) * 1000));
            for (int i = 0; i < 5; i++) employees2.Add(new Worker($"2Worker {i + 1}", i, (i + 1) * 1000));

            var dep1 = new Department(employees1, "Dep1");
            var dep2 = new Department(employees2, "Dep2");

            var deps = new List<Department> { dep1, dep2 };

            company = new Department(deps, "MainDep");

            var manager1 = new Manager(company, "manager1", 0, 10000);

            company.Manager = manager1;

            SerializeJson("Data.json");
        }

        public void SerializeJson(string path)
        {
            var serialized = JsonConvert.SerializeObject(company, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            });

            File.WriteAllText(path, serialized);
        }
    }
}