using PeopleDepartment.CommonLibrary;

namespace PeopleDepartment.ReportConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "REPORT CONSOLE FOR EMPLOYEES";
            if (args.Length == 0)
            {
                Console.WriteLine("Nezadali ste dostatok argumentov.\nExiting program...");
                return;
            }

            string? inputFile = null;
            string? templateFile = null;
            string? outputFile = null;
            PersonCollection pc = [];
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("--input"))
                {
                    inputFile = args[i + 1];
                }
                if (args[i].Equals("--template"))
                {
                    templateFile = args[i + 1];
                }
                if (args[i].Equals("--output"))
                {
                    outputFile = args[i + 1];
                }
            }

            if (string.IsNullOrEmpty(inputFile) || string.IsNullOrEmpty(templateFile))
            {
                Console.WriteLine("Nezadali ste povinne argumenty!\n--input and --template cannot be null.\nExiting program...");
                return;
            }

            if (File.Exists(inputFile))
            {
                pc.LoadFromCsv(new FileInfo(inputFile));
            }

            string template = "";
            if (File.Exists(templateFile))
            {
                template = File.ReadAllText(templateFile);
            }
            var reports = pc.GenerateDepartmentReports();
            foreach (var report in reports)
            {
                string reportText = template
                    .Replace("[[Department]]", report.Department)
                    .Replace("[[Head]]", report.Head?.DisplayName ?? "N/A")
                    .Replace("[[Deputy]]", report.Deputy?.DisplayName ?? "N/A")
                    .Replace("[[Secretary]]", report.Secretary?.DisplayName ?? "N/A")
                    .Replace("[[NumberOfEmployees]]", report.NumberOfEmployees.ToString())
                    .Replace("[[NumberOfProfessors]]", report.NumberOfProfessors.ToString())
                    .Replace("[[NumberOfAssociateProfessors]]", report.NumberOfAssociateProfessors.ToString())
                    .Replace("[[NumberOfPhDStudents]]", report.NumberOfPhDStudents.ToString())
                    .Replace("[[Employees]]",
                        string.Join(Environment.NewLine, report.Employees.Select(e => e.ToFormattedString())))
                    .Replace("[[PhDStudents]]",
                        string.Join(Environment.NewLine, report.PhDStudents.Select(e => e.ToFormattedString())));

                if (string.IsNullOrEmpty(outputFile))
                {
                    Console.WriteLine(reportText);
                    Console.WriteLine();
                }
                else
                {
                    if (!Directory.Exists(outputFile))
                    {
                        Directory.CreateDirectory(outputFile);
                    }
                    var repFilePath = Path.Combine(outputFile, $"{report.Department}.txt");
                    File.WriteAllText(repFilePath, reportText);
                }
            }
        }
    }
}