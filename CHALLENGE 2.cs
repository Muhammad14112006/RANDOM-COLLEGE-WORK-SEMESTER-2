using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2L4
{
    class Member
    {
        //data members
        public string name;
        public int memberID;
        public string[] books = new string[10];
        public int numBooks;
        public double moneyInBank;
        public double amountSpent;

        //default constructor
        public Member()
        {
            this.name = "";
            this.memberID = 0;
            this.numBooks = 0;
            this.moneyInBank = 0;
            this.amountSpent = 0;
        }

        //parameterized constructor
        public Member(string name, int memberID, double moneyInBank)
        {
            this.name = name;
            this.memberID = memberID;
            this.moneyInBank = moneyInBank;
            this.numBooks = 0;
            this.amountSpent = 0;
        }

        //buy book function
        public void BuyBook(string bookName, double price)
        {
            if (this.numBooks < this.books.Length && this.moneyInBank >= price) // to check if the array books has an empty slot or not to enter new books...... and also if the money in the bank is sufficient enough to buy new books
            {
                this.books[this.numBooks] = bookName;
                this.numBooks++;

                this.moneyInBank -= price;
                this.amountSpent += price;

                Console.WriteLine("Book bought successfully !");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Unable to buy the book.");
            }
        }
        //function to show the names of the books bought by the "member"
        public void  ShowBooks()
        {
            Console.Clear();
            Console.WriteLine("----Books Bought----");
            for (int i = 0; i < this.numBooks; i++)
            {
                Console.WriteLine(this.books[i]);
            }
            if (this.numBooks == 0)
            {
                Console.WriteLine("No books bought yet.");
            }

            Console.ReadKey();
        }
        // function to show details of the "member" who bought the books
        public void ShowDetails()
        {
            Console.Clear();
            Console.WriteLine("----Member Details----");
            Console.WriteLine("Name: " + this.name);
            Console.WriteLine("Member ID: " + this.memberID);
            Console.WriteLine("Book Bought: " + this.numBooks);
            Console.WriteLine("Money Left: " + this.moneyInBank);
            Console.WriteLine("Amount Spent: " + this.amountSpent);
            Console.ReadKey();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Member m = new Member(); // the main member object
            int choice;
            Console.Clear();
            do
            {
                Console.WriteLine("----Menu----");
                Console.WriteLine("1. Set Member Info");
                Console.WriteLine("2. Buy Book");
                Console.WriteLine("3. Show Books");
                Console.WriteLine("4. Show Details");
                Console.WriteLine("5. Exit");
                Console.Write("\nEnter your choice: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        {
                            Console.Clear();
                            Console.WriteLine("----Set Member Info----");
                            Console.Write("Enter Name: ");
                            m.name = Console.ReadLine();
                            Console.Write("Enter Member ID: ");
                            m.memberID = int.Parse(Console.ReadLine());
                            Console.Write("Enter the money in Bank: ");
                            m.moneyInBank = double.Parse(Console.ReadLine());
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            Console.WriteLine("----Buy Books----");
                            Console.Write("Enter Book Name: ");
                            string book = Console.ReadLine();
                            Console.Write("Enter Book Price: ");
                            double price = double.Parse(Console.ReadLine());
                            m.BuyBook(book, price);
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            m.ShowBooks();
                            break;
                        }
                    case 4:
                        {
                            m.ShowDetails();
                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("Enter any key to exit the program...");
                            Console.ReadKey();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Invalid Choice! Please Enter 1-5.");
                            Console.ReadKey();
                            break;
                        }
                }
            }
            while (choice != 5);
        }
    }
}
