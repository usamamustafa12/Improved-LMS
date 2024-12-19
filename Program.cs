using System;
using Microsoft.Data.SqlClient;

namespace LibraryManagementSystem
{
    // Abstraction
    public abstract class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Contact { get; set; }

        // Polymorphism
        public virtual void DisplayDetails()
        {
            Console.WriteLine($"UserID: {UserID}, Name: {Name}, Role: {Role}, Contact: {Contact}");
        }
    }

    // Inheritance
    public class Borrower : User
    {
        public string Address { get; set; }

        // Polymorphism Overriding the base class
        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Address: {Address}");
        }
    }

    // Inheritance
    public class AdminStaff : User
    {
        public string Department { get; set; }

        //Polymorphism Overriding the base class
        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Department: {Department}");
        }
    }

    //Encapsulation Book class with private fields and public properties
    public class Book
    {
        private int bookID;
        private string title;
        private string author;
        private int quantity;

        public int BookID
        {
            get { return bookID; }
            set { bookID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
    }

    class Program
    {
        private static readonly string connectionString = "Server=localhost\\SQLEXPRESS;Database=hehe;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Library Management System");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. View Books");
                Console.WriteLine("3. Update Book");
                Console.WriteLine("4. Remove Book");
                Console.WriteLine("5. Add Borrower");
                Console.WriteLine("6. View Borrowers");
                Console.WriteLine("7. Update Borrower");
                Console.WriteLine("8. Remove Borrower");
                Console.WriteLine("9. Add Admin Staff");
                Console.WriteLine("10. View Admin Staff");
                Console.WriteLine("11. Update Admin Staff");
                Console.WriteLine("12. Remove Admin Staff");
                Console.WriteLine("13. Exit");

                Console.WriteLine("\nEnter your choice: ");
                string input = Console.ReadLine();
                int choice;
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddBook();
                            break;
                        case 2:
                            ViewBooks();
                            break;
                        case 3:
                            UpdateBook();
                            break;
                        case 4:
                            RemoveBook();
                            break;
                        case 5:
                            AddBorrower();
                            break;
                        case 6:
                            ViewBorrowers();
                            break;
                        case 7:
                            UpdateBorrower();
                            break;
                        case 8:
                            RemoveBorrower();
                            break;
                        case 9:
                            AddAdminStaff();
                            break;
                        case 10:
                            ViewAdminStaff();
                            break;
                        case 11:
                            UpdateAdminStaff();
                            break;
                        case 12:
                            RemoveAdminStaff();
                            break;
                        case 13:
                            Console.WriteLine("Exiting... Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Press any key to continue.");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice, please enter a number between 1 and 13.");
                    Console.ReadKey(); 
                }
            }
        }

        // Books
        static void AddBook()
        {
            Console.WriteLine("Enter Book Title:");
            string title = Console.ReadLine();
            Console.WriteLine("Enter Author:");
            string author = Console.ReadLine();
            Console.WriteLine("Enter Quantity:");
            int quantity = int.Parse(Console.ReadLine());

            string query = "INSERT INTO Books (Title, Author, Quantity) VALUES (@Title, @Author, @Quantity)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Author", author);
                command.Parameters.AddWithValue("@Quantity", quantity);
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Book added successfully!");
            }
        }

        static void ViewBooks()
        {
            string query = "SELECT * FROM Books";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) 
                {
                    Console.WriteLine("BookID\tTitle\tAuthor\tQuantity");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["BookID"]}\t{reader["Title"]}\t{reader["Author"]}\t{reader["Quantity"]}");
                    }
                }
                else
                {
                    Console.WriteLine("No books found.");
                }
                Console.WriteLine("\nPress any key to go back to the menu.");
                Console.ReadKey();
            }
        }

        static void UpdateBook()
        {
            Console.WriteLine("Enter the BookID of the book you want to update:");
            int bookID = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new Book Title:");
            string title = Console.ReadLine();
            Console.WriteLine("Enter new Author:");
            string author = Console.ReadLine();
            Console.WriteLine("Enter new Quantity:");
            int quantity = int.Parse(Console.ReadLine());

            string query = "UPDATE Books SET Title = @Title, Author = @Author, Quantity = @Quantity WHERE BookID = @BookID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookID", bookID);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Author", author);
                command.Parameters.AddWithValue("@Quantity", quantity);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Book updated successfully!");
                else
                    Console.WriteLine("No book found with that ID.");
            }
        }

        static void RemoveBook()
        {
            Console.WriteLine("Enter the BookID of the book you want to remove:");
            int bookID = int.Parse(Console.ReadLine());

            string query = "DELETE FROM Books WHERE BookID = @BookID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookID", bookID);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Book removed successfully!");
                else
                    Console.WriteLine("No book found with that ID.");
            }
        }

        //Borrowers
        static void AddBorrower()
        {
            Console.WriteLine("Enter Borrower Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Borrower Contact:");
            string contact = Console.ReadLine();
            Console.WriteLine("Enter Borrower Address:");
            string address = Console.ReadLine();

            string query = "INSERT INTO Borrowers (Name, Contact, Address) VALUES (@Name, @Contact, @Address)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Contact", contact);
                command.Parameters.AddWithValue("@Address", address);
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Borrower added successfully!");
            }
        }

        static void ViewBorrowers()
        {
            string query = "SELECT * FROM Borrowers";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) 
                {
                    Console.WriteLine("UserID\tName\tContact\tAddress");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["UserID"]}\t{reader["Name"]}\t{reader["Contact"]}\t{reader["Address"]}");
                    }
                }
                else
                {
                    Console.WriteLine("No borrowers found.");
                }
                Console.WriteLine("\nPress any key to go back to the menu.");
                Console.ReadKey();  
            }
        }

        static void UpdateBorrower()
        {
            Console.WriteLine("Enter the UserID of the borrower you want to update:");
            int userID = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new Borrower Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter new Borrower Contact:");
            string contact = Console.ReadLine();
            Console.WriteLine("Enter new Borrower Address:");
            string address = Console.ReadLine();

            string query = "UPDATE Borrowers SET Name = @Name, Contact = @Contact, Address = @Address WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Contact", contact);
                command.Parameters.AddWithValue("@Address", address);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Borrower updated successfully!");
                else
                    Console.WriteLine("No borrower found with that ID.");
            }
        }

        static void RemoveBorrower()
        {
            Console.WriteLine("Enter the UserID of the borrower you want to remove:");
            int userID = int.Parse(Console.ReadLine());

            string query = "DELETE FROM Borrowers WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Borrower removed successfully!");
                else
                    Console.WriteLine("No borrower found with that ID.");
            }
        }

        //Admin Staff 
        static void AddAdminStaff()
        {
            Console.WriteLine("Enter Admin Staff Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Admin Staff Contact:");
            string contact = Console.ReadLine();
            Console.WriteLine("Enter Admin Staff Department:");
            string department = Console.ReadLine();

            string query = "INSERT INTO AdminStaff (Name, Contact, Department) VALUES (@Name, @Contact, @Department)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Contact", contact);
                command.Parameters.AddWithValue("@Department", department);
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Admin staff added successfully!");
            }
        }

        static void ViewAdminStaff()
        {
            string query = "SELECT * FROM AdminStaff";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)  
                {
                    Console.WriteLine("UserID\tName\tContact\tDepartment");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["UserID"]}\t{reader["Name"]}\t{reader["Contact"]}\t{reader["Department"]}");
                    }
                }
                else
                {
                    Console.WriteLine("No admin staff found.");
                }
                Console.WriteLine("\nPress any key to go back to the menu.");
                Console.ReadKey(); 
            }
        }

        static void UpdateAdminStaff()
        {
            Console.WriteLine("Enter the UserID of the admin staff you want to update:");
            int userID = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new Admin Staff Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter new Admin Staff Contact:");
            string contact = Console.ReadLine();
            Console.WriteLine("Enter new Admin Staff Department:");
            string department = Console.ReadLine();

            string query = "UPDATE AdminStaff SET Name = @Name, Contact = @Contact, Department = @Department WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Contact", contact);
                command.Parameters.AddWithValue("@Department", department);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Admin staff updated successfully!");
                else
                    Console.WriteLine("No admin staff found with that ID.");
            }
        }

        static void RemoveAdminStaff()
        {
            Console.WriteLine("Enter the UserID of the admin staff you want to remove:");
            int userID = int.Parse(Console.ReadLine());

            string query = "DELETE FROM AdminStaff WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Admin staff removed successfully!");
                else
                    Console.WriteLine("No admin staff found with that ID.");
            }
        }
    }
}
