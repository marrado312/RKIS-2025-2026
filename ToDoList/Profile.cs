using System;

namespace TodoList
{
    class Profile
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int BirthYear { get; private set; }
        public Profile(string firstName, string lastName, int birthYear)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthYear = birthYear;
        }

        public string GetInfo()
        {
            int age = DateTime.Now.Year - BirthYear;
            return $"{FirstName} {LastName}, возраст {age}";
        }
    }
}