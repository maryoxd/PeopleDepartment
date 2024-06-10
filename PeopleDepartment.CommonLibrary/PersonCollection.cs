using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleDepartment.CommonLibrary
{
    public class PersonCollection : IEnumerable<Person>
    {
        private List<Person> persons = [];

        public IEnumerator<Person> GetEnumerator()
        {
            return persons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Person person)
        {
            persons.Add(person);
        }

        public bool Remove(Person person)
        {
            return persons.Remove(person);
        }

        public void LoadFromCsv(FileInfo csvFile)
        {
            if(!File.Exists(csvFile.FullName))
            {
                Console.WriteLine("csvFile doesn't exists! Cannot load data.");
                return;
            }
            StreamReader sr = new(csvFile.FullName);
            sr.ReadLine();

            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parts = line.Split(";");
                Person person = new(
                    parts[0],
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    [.. parts[5].Split(",")]
                    );
            Add(person);
            }
            persons.Sort((x,y) => string.Compare(x.FirstName, y.FirstName, StringComparison.OrdinalIgnoreCase));
             
        }

        public DepartmentReport[] GenerateDepartmentReports()
        {
            var reports = new List<DepartmentReport>();
            var departments = persons.SelectMany(p => p.Departments).Distinct();

            foreach(var department in departments)
            {
                var employees = persons.Where(p => p.Departments.Contains(department) &&
                !p.Position.Contains("doktorand"))
                .OrderBy(p => p.FirstName)
                .ToList();

                var phdStudents = persons.Where(p => p.Departments.Contains(department) &&
                p.Position.Contains("doktorand"))
                .OrderBy(p => p.FirstName)
                .ToList();

                var Head = persons.FirstOrDefault(p => p.Departments.Contains(department) &&
                p.Position.Contains("vedúci"));

                var Deputy = persons.FirstOrDefault(p => p.Departments.Contains(department) &&
                p.Position.Contains("zástupca vedúceho"));

                var Secretary = persons.FirstOrDefault(p => p.Departments.Contains(department) &&
                p.Position.Contains("sekretárka"));

                int NumberOfProfessors = persons.Count(p => p.DisplayName.Contains("prof.") &&
                p.Departments.Contains(department));
                int NumberOfAssociateProfessors = persons.Count(p => p.DisplayName.Contains("doc.") &&
                p.Departments.Contains(department));

                var report = new DepartmentReport(
                    department,
                    Head,
                    Deputy,
                    Secretary,
                    NumberOfProfessors,
                    NumberOfAssociateProfessors,
                    employees,
                    phdStudents);
                reports.Add(report);
            }
            reports.Sort((x, y) => string.Compare(x.Department, y.Department, StringComparison.Ordinal));
            return [.. reports];
        }
    }
}
