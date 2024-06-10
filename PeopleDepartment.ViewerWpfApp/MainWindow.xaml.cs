using Microsoft.Win32;
using PeopleDepartment.CommonLibrary;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace PeopleDepartment.ViewerWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string SelectedItem = "";
        private PersonCollection pc = [];
        private DepartmentReport[] report;

        public MainWindow()
        {
            InitializeComponent();
            emp.Text = "0";
            phd.Text = "0";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new()
            {
                Filter = "CSV Files (*.csv) | *.csv | All Files (*.*) | *.*",
                FilterIndex = 1,
                InitialDirectory = @"C:\Desktop",
                RestoreDirectory = true
            };
            bool? result = open.ShowDialog();

            if (result == true)
            {
                string FilePath = open.FileName;
                pc.LoadFromCsv(new FileInfo(FilePath));
            }

            report = pc.GenerateDepartmentReports();
            for (int i = 0; i < report.Length; i++)
            {
                ComboBox.Items.Add(report[i].Department);
            }
            ComboBox.SelectedIndex = 0;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBox.SelectedItem != null)
            {
                SelectedItem = ComboBox.SelectedItem.ToString();
            }
            NaplnZamestnancov();
            
        }
        private void NaplnZamestnancov()
        {
            EmployeesListView.Items.Clear();
            PhDListView.Items.Clear();

            foreach(var rep in report)
            {
                if(rep.Department == SelectedItem)
                {
                    var employees = rep.Employees;

                    foreach(var emp in  employees)
                    {
                        EmployeesListView.Items.Add(emp);
                    }
                    t1.Text = rep?.Head?.DisplayName;
                    t2.Text = rep?.Deputy?.DisplayName;
                    t3.Text = rep?.Secretary?.DisplayName;

                    emp.Text = rep?.NumberOfEmployees.ToString();
                    
                }
            }

            foreach(var rep in report)
            {
                if(rep.Department == SelectedItem)
                {
                    var phdStudents = rep.PhDStudents;
                    
                    foreach(var phd in  phdStudents)
                    {
                        PhDListView.Items.Add(phd);
                    }
                    phd.Text = rep.NumberOfPhDStudents.ToString();
                }
            }
        }
    }
}