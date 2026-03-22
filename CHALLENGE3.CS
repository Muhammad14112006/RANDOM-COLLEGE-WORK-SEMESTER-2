using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace C3L4
{

    //book class
    class Book
    {
        public string title;
        public string isbn;
        public string publisher;
        public int year;
        public double price;
        public int stock;
        public List<string> authors = new List<string>();

        //function to show all the details of book
        public void ShowBookInfo()
        {
            Console.WriteLine($"[{isbn}] {title} | Price: ${price} | Stock: {stock}");
            Console.WriteLine($"Publisher: {publisher} ({year}) | Authors: {string.Join(", ", authors)}");
            Console.ReadKey();
        }
    }

    //class member
    class Member
    {
        public string name;
        public int memberID;
        public int booksPurchased;
        public double totalAmountSpent;

        //parameterized constructors
        public Member(string name, int id)
        {
            this.name = name;
            this.memberID = id;
            this.booksPurchased = 0;
            this.totalAmountSpent = 0;
        }

        //function to show the details of the member
        public void ShowDetails()
        {
            string type = (memberID > 0) ? "Member" : "Occasional Buyer";
            Console.WriteLine($"Name: {name} | ID: {memberID} ({type})");
            Console.WriteLine($"Books Purchased: {booksPurchased} | Amount Spent towards next Discount: ${totalAmountSpent:F2}"); // :F2 gives us the output in 2 places after the decimal
            Console.ReadKey();
        }
    }
    internal class Program
    {
        static List<Book> inventory = new List<Book>();
        static List<Member> members = new List<Member>();
        static double totalSales = 0;
        static double totalMembershipFees = 0;
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("====BOOKSTORE MANAGEMENT SYSTEM====");
                Console.WriteLine("a. Add a Book");
                Console.WriteLine("b.Search Book (Title)");
                Console.WriteLine("c. Search Book (ISBN)");
                Console.WriteLine("d. Update Book Stock");
                Console.WriteLine("e. Add a member");
                Console.WriteLine("f. Search for Member");
                Console.WriteLine("g. Update Member Info");
                Console.WriteLine("h. Purchase a Book");
                Console.WriteLine("i. Display Sales Stats");
                Console.WriteLine("j. Exit");
                Console.Write("\nSelect an Option: ");
                string choice = Console.ReadLine()?.ToLower();

                switch (choice)
                {
                    case "a":
                        {
                            AddBook();
                            break;
                        }
                    case "b":
                        {
                            SearchBookTitle();
                            break;
                        }
                    case "c":
                        {
                            SearchBookISBN();
                            break;
                        }
                    case "d":
                        {
                            UpdateStock();
                            break;
                        }
                    case "e":
                        {
                            AddMember();
                            break;
                        }
                    case "f":
                        {
                            SearchMember();
                            break;
                        }
                    case "g":
                        {
                            UpdateMember();
                            break;
                        }
                    case "h":
                        {
                            PurchaseBook();
                            break;
                        }
                    case "i":
                        {
                            DisplayStats();
                            break;
                        }
                    case "j":
                        {
                            exit = true;
                            break;
                        }
                }
            }
        }
        static void AddBook()
        {
            Book b = new Book();
            Console.Write("Enter title: ");
            b.title = Console.ReadLine();
            Console.Write("Enter ISBN: ");
            b.isbn = Console.ReadLine();
            Console.Write("Enter Publisher: ");
            b.publisher = Console.ReadLine();
            Console.Write("Enter Year: ");
            b.year = int.Parse(Console.ReadLine());
            Console.Write("Enter Price: ");
            b.price = double.Parse(Console.ReadLine());
            Console.Write("Enter Stock: ");
            b.stock = int.Parse(Console.ReadLine());
            Console.Write("Enter Authors (Comma Seperated)");
            b.authors = Console.ReadLine().Split(',').ToList();

            inventory.Add(b);
            Console.WriteLine("\nBook Added Successfully.");
            Console.ReadKey();
        }
        static void AddMember()
        {
            Console.Clear();
            Console.WriteLine("====Add Member====");
            Console.WriteLine("Enter Naame: ");
            string n = Console.ReadLine();
            Console.WriteLine("Enter Member ID (0 for occasional customer): ");
            int id = int.Parse(Console.ReadLine());

            Member m = new Member(n, id);
            members.Add(m);

            if (m.memberID > 0)
            {
                totalMembershipFees += 10;
                Console.WriteLine("\nMember Registered Successfully.\n$10 Membership Fee Collected.");
            }
            Console.ReadKey();
        }
        static void PurchaseBook()
        {
            Console.Clear();
            Console.WriteLine("==== Book Purchase ====");
            Console.Write("Enter Customer Name or ID: ");
            string input = Console.ReadLine();
            Member m = members.Find(x => x.name.Equals(input, StringComparison.OrdinalIgnoreCase) || x.memberID.ToString() == input);

            if (m == null)
            {
                Console.WriteLine("Customer not Found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Book Title or ISBN: ");
            string bookInput = Console.ReadLine();
            Book b = inventory.Find(x => x.title.Equals(bookInput, StringComparison.OrdinalIgnoreCase) || x.isbn == bookInput);

            if (b == null || b.stock <= 0)
            {
                Console.WriteLine("Boook out of Stock or Not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Quantity: ");
            int qty = int.Parse(Console.ReadLine());

            if (qty > b.stock)
            {
                Console.WriteLine("Insufficient Stock...");
                Console.ReadKey();
                return;
            }

            double finalBill = 0;
            for (int i = 0; i < qty; i++)
            {
                m.booksPurchased++;
                double currentPrice = b.price;

                if (m.memberID > 0) // Member incentives 
                {
                    currentPrice *= 0.95; //5% discount
                    if (m.booksPurchased % 11 == 0)
                    {
                        double discount = m.totalAmountSpent / 10;
                        currentPrice -= discount;
                        if (currentPrice < 0)
                        {
                            currentPrice = 0;
                        }
                        m.totalAmountSpent = 0;
                        Console.WriteLine($"Bonus: 11th book discount of ${discount:F2} has been applied.");
                        Console.ReadKey();
                    }
                    else
                    {
                        m.totalAmountSpent += currentPrice;
                    }
                }
                finalBill += currentPrice;
            }
            b.stock -= qty;
            totalSales += finalBill;
            Console.WriteLine($"Total Bill: ${finalBill:F2}");
            Console.ReadKey();
        }
        static void SearchBookTitle()
        {
            Console.Clear();
            Console.WriteLine("==== Search By Book Title ====");
            Console.Write("Enter Title: ");
            string t = Console.ReadLine();
            Book b = inventory.Find(x => x.title.Equals(t, StringComparison.OrdinalIgnoreCase));

            if (b != null)
            {
                b.ShowBookInfo();
            }
            else
            {
                Console.WriteLine("Not Found.");
                Console.ReadKey();
            }
        }
        static void SearchBookISBN()
        {
            Console.Clear();
            Console.WriteLine("==== Search by Book ISBN ====");
            Console.Write("Enter ISBN: ");
            string code = Console.ReadLine();
            Book b = inventory.Find(x => x.isbn == code);

            if (b != null)
            {
                b.ShowBookInfo();
            }
            else
            {
                Console.WriteLine("Not Found.");
            }
            Console.ReadKey();
        }

        static void UpdateStock()
        {
            Console.Clear();
            Console.WriteLine("==== Update Stock ====");
            Console.Write("Enter ISBN: ");
            string code = Console.ReadLine();
            Book b = inventory.Find(x => x.isbn == code);

            if (b != null)
            {
                Console.Write("Current Stock: " + b.stock);
                Console.WriteLine("Add/Remove Amount: "); // -ve sign to decrement in the stock
                b.stock += int.Parse(Console.ReadLine());
                Console.WriteLine("Stock Updated.");
            }
            Console.ReadKey();
        }
        static void SearchMember()
        {
            Console.Clear();
            Console.Write("Search Member Name/ID: ");
            string input = Console.ReadLine();
            Member m = members.Find(x => x.name.Equals(input, StringComparison.OrdinalIgnoreCase) || x.memberID.ToString() == input);

            if (m != null)
            {
                m.ShowDetails();
            }
            else
            {
                Console.WriteLine("Not Found.");
            }
            Console.ReadKey();
        }
        static void UpdateMember()
        {
            Console.Clear();
            Console.Write("Enter Member Name/ID to Update: ");
            string input = Console.ReadLine();
            Member m = members.Find(x => x.name.Equals(input, StringComparison.OrdinalIgnoreCase) || x.memberID.ToString() == input);

            if (m != null)
            {
                Console.Write("New Name: ");
                m.name = Console.ReadLine();
                Console.WriteLine("Name Updated Successfully.");
            }
            Console.ReadKey();
        }
        static void DisplayStats()
        {
            Console.Clear();
            Console.WriteLine("===== SALES SUMMARY =====");
            Console.WriteLine($"Total Revenue:      ${totalSales:F2}");
            Console.WriteLine($"Membership Fees:     ${totalMembershipFees:F2}");
            Console.WriteLine($"Registered Members:  {members.Count(x => x.memberID > 0)}");
            Console.ReadKey();
        }
    }
}
