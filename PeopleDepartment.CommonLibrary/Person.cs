namespace PeopleDepartment.CommonLibrary
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string? TitleBefore { get; }
        public string? TitleAfter { get; }
        public string? Position { get; set; }
        public string Email { get; set; }
        public List<string> Departments { get; }

        public Person(string firstName, string lastName, string displayName, string? position,
            string email, List<string> departments)
        {
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            Position = position;
            Email = email;
            Departments = departments;
            TitleBefore = DisplayName?.Split(FirstName + " " + LastName).FirstOrDefault();
            TitleAfter = DisplayName?.Split(FirstName + " " + LastName).LastOrDefault().Replace(",", "");
        }

        public string ToFormattedString()
        {
            return $"{DisplayName,-40} {Email}";
        }
    }

   }

