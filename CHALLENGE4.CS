using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityAdmissionSystem
{
    // 1. Subject Class
    class Subject
    {
        public string Code;
        public int CreditHours;
        public string SubjectType;

        public Subject(string code, int ch, string type)
        {
            Code = code;
            CreditHours = ch;
            SubjectType = type;
        }
    }

    // 2. Degree Program Class
    class DegreeProgram
    {
        public string Title;
        public int Duration;
        public int Seats;
        public List<Subject> OfferedSubjects = new List<Subject>();

        public DegreeProgram(string title, int duration, int seats)
        {
            Title = title;
            Duration = duration;
            Seats = seats;
        }

        public int GetTotalOfferedCreditHours() => OfferedSubjects.Sum(s => s.CreditHours);

        public bool AddSubject(Subject s)
        {
            if (GetTotalOfferedCreditHours() + s.CreditHours <= 20)
            {
                OfferedSubjects.Add(s);
                return true;
            }
            return false;
        }
    }

    // 3. Student Class
    class Student
    {
        public string Name;
        public int Age;
        public double FscMarks;
        public double EcatMarks;
        public double Merit;
        public List<string> Preferences = new List<string>();
        public DegreeProgram RegisteredProgram = null;
        public List<Subject> RegisteredSubjects = new List<Subject>();

        public Student(string name, int age, double fsc, double ecat, List<string> prefs)
        {
            Name = name;
            Age = age;
            FscMarks = fsc;
            EcatMarks = ecat;
            Preferences = prefs;
            // Basic Merit Formula: 60% FSC + 40% ECAT
            Merit = (fsc * 0.6) + (ecat * 0.4);
        }

        public int GetCurrentCreditHours() => RegisteredSubjects.Sum(s => s.CreditHours);

        public void ShowStudentDetails()
        {
            string status = (RegisteredProgram != null) ? RegisteredProgram.Title : "Not Admitted";
            Console.WriteLine($"Student: {Name} | Merit: {Merit:F2} | Status: {status}");
            if (RegisteredSubjects.Count > 0)
            {
                Console.WriteLine("Registered Subjects: " + string.Join(", ", RegisteredSubjects.Select(s => s.Code)));
            }
        }
    }

    class Program
    {
        static List<DegreeProgram> universityPrograms = new List<DegreeProgram>();
        static List<Student> applicants = new List<Student>();

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("==== UNIVERSITY ADMISSION SYSTEM ====");
                Console.WriteLine("1. Add Program (Academic Branch)");
                Console.WriteLine("2. Add Subject to Program");
                Console.WriteLine("3. Student Admission Application");
                Console.WriteLine("4. Generate Merit List & Admit (Admission Dept)");
                Console.WriteLine("5. Register Subjects for Student");
                Console.WriteLine("6. Generate Fees (Fee Dept)");
                Console.WriteLine("7. Exit");
                Console.Write("\nSelect Option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddProgram(); break;
                    case "2": AddSubjectToProgram(); break;
                    case "3": ApplyForAdmission(); break;
                    case "4": ProcessAdmissions(); break;
                    case "5": RegisterSubjects(); break;
                    case "6": GenerateFees(); break;
                    case "7": exit = true; break;
                }
            }
        }

        // --- Academic Branch Methods ---
        static void AddProgram()
        {
            Console.Write("Enter Degree Title: ");
            string title = Console.ReadLine();
            Console.Write("Enter Duration (Years): ");
            int dur = int.Parse(Console.ReadLine());
            Console.Write("Enter Available Seats: ");
            int seats = int.Parse(Console.ReadLine());
            universityPrograms.Add(new DegreeProgram(title, dur, seats));
            Console.WriteLine("Program added successfully!");
            Console.ReadKey();
        }

        static void AddSubjectToProgram()
        {
            Console.Write("Enter Program Title: ");
            string title = Console.ReadLine();
            var prog = universityPrograms.Find(p => p.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (prog != null)
            {
                Console.Write("Subject Code: ");
                string code = Console.ReadLine();
                Console.Write("Credit Hours: ");
                int ch = int.Parse(Console.ReadLine());
                Console.Write("Type (Core/Elective): ");
                string type = Console.ReadLine();

                if (prog.AddSubject(new Subject(code, ch, type)))
                    Console.WriteLine("Subject Added.");
                else
                    Console.WriteLine("Error: Total Program Credit Hours cannot exceed 20!");
            }
            else Console.WriteLine("Program not found.");
            Console.ReadKey();
        }

        // --- Admission Department Methods ---
        static void ApplyForAdmission()
        {
            Console.Write("Student Name: ");
            string name = Console.ReadLine();
            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("FSC Marks: ");
            double fsc = double.Parse(Console.ReadLine());
            Console.Write("Ecat Marks: ");
            double ecat = double.Parse(Console.ReadLine());
            Console.Write("Enter Preferences (comma separated): ");
            List<string> prefs = Console.ReadLine().Split(',').Select(p => p.Trim()).ToList();

            applicants.Add(new Student(name, age, fsc, ecat, prefs));
            Console.WriteLine("Application Submitted.");
            Console.ReadKey();
        }

        static void ProcessAdmissions()
        {
            // Sort students by Merit (Descending)
            var meritList = applicants.OrderByDescending(s => s.Merit).ToList();

            foreach (var student in meritList)
            {
                foreach (var pref in student.Preferences)
                {
                    var prog = universityPrograms.Find(p => p.Title.Equals(pref, StringComparison.OrdinalIgnoreCase));
                    if (prog != null && prog.Seats > 0)
                    {
                        student.RegisteredProgram = prog;
                        prog.Seats--;
                        break; // Student admitted to their highest possible preference
                    }
                }
            }
            Console.WriteLine("Merit List Processed and Students Registered.");
            Console.ReadKey();
        }

        // --- Registration Logic ---
        static void RegisterSubjects()
        {
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();
            var student = applicants.Find(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (student?.RegisteredProgram != null)
            {
                Console.WriteLine($"Available Subjects in {student.RegisteredProgram.Title}:");
                foreach (var sub in student.RegisteredProgram.OfferedSubjects)
                    Console.WriteLine($"- {sub.Code} ({sub.CreditHours} CH)");

                Console.Write("Enter Subject Code to Register: ");
                string code = Console.ReadLine();
                var subject = student.RegisteredProgram.OfferedSubjects.Find(s => s.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

                if (subject != null)
                {
                    if (student.GetCurrentCreditHours() + subject.CreditHours <= 9)
                    {
                        student.RegisteredSubjects.Add(subject);
                        Console.WriteLine("Subject Registered.");
                    }
                    else Console.WriteLine("Limit Exceeded! Student cannot take more than 9 credit hours.");
                }
            }
            else Console.WriteLine("Student not found or not admitted to any program.");
            Console.ReadKey();
        }

        // --- Fee Department ---
        static void GenerateFees()
        {
            foreach (var s in applicants.Where(s => s.RegisteredSubjects.Count > 0))
            {
                double fee = s.GetCurrentCreditHours() * 500; // Assuming $500 per credit hour
                Console.WriteLine($"Student: {s.Name} | Total Credit Hours: {s.GetCurrentCreditHours()} | Total Fee: ${fee}");
            }
            Console.ReadKey();
        }
    }
}
