using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class Menu
{
    private readonly HabitRepository _repository;

    public Menu(HabitRepository repository)
    {
        _repository = repository;
    }

    public void Run()
    {
        bool CloseApp = false;

        while (!CloseApp)
        {
            Console.Clear();

            Console.WriteLine("""
            Main Menu

            What would you like to do ?

            0.Close Application
            1.View All Records
            2.Insert Record
            3.Delete Record
            4.Update Record
            --------------------------------
            """);
            string? input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    CloseApp = true;
                    System.Console.WriteLine("Goodbye ");
                    break;

                case "1":
                    ViewRecords();
                    break;

                case "2":
                    InsertRecord();
                    break;

                case "3":
                    DeleteRecord();
                    break;

                case "4":
                    UpdateRecord();
                    break;

                default:
                    System.Console.WriteLine("Invalid option. Please Type a number from 0 to 4");
                    Pause();
                    break;
            }
        }

    }

    private void ViewRecords()
    {
        ShowRecords();
        Pause();
    }

    private bool ShowRecords()
    {
        List<DrinkingWater> records = _repository.GetAll();

        if (records.Count == 0)
        {
            System.Console.WriteLine("\nNo records found.");
            return false;
        }

        System.Console.WriteLine("\n----------------------------------");
        foreach (var record in records)
        {
            System.Console.WriteLine(@$"
            {record.Id} | {record.Date:dd-MMM-yyyy} | Quantity: {record.Quantity}");
        }
        System.Console.WriteLine("\n----------------------------------");

        return true;
    }

    private void InsertRecord()
    {
        DateTime date = GetDateInput();
        int quantity = GetNumberInput("Enter the quantity (whole number, e.g. 5):" );

        var record = new DrinkingWater
        {
            Date = date,
            Quantity = quantity
        };

        _repository.Insert(record);

        Console.WriteLine("\nRecord added ✅");
        Pause();
    }

    private DateTime GetDateInput()
    {
        Console.Write("Enter The Date (dd-MM-yy), Or type 't for today :");

        string? input = Console.ReadLine();
        if (input?.ToLower() == "t")
            return DateTime.Today;

        DateTime date;

        while (!DateTime.TryParseExact(input, "d-M-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            Console.Write("Invalid date. Use dd-MM-yy (e.g. 15-11-25), or 't' for today: ");
            input = Console.ReadLine();
            if (input?.ToLower() == "t")
                return DateTime.Today;

        }
        return date;

    }

    private int GetNumberInput(string message)
    {
        System.Console.Write($"\n{message}");
        string? input = Console.ReadLine();

        int quantity;
        while (!int.TryParse(input, out quantity) || quantity <= 0)
        {
            System.Console.Write($"\nInvalid number. {message} ");
            input = Console.ReadLine();
        }

        return quantity;
    }

    private void DeleteRecord()
    {
        bool HasRecords=ShowRecords();

        if(!HasRecords)
        {
            Pause();
            return;
        }
       int id =GetNumberInput("Enter the Id of the record you want to delete: ");

        if (!_repository.Exists(id))
        {
            Console.WriteLine($"\nRecord {id} doesn't exist ❌");
            Pause();
            return;
        }

       Console.Write($"Are you sure you want to delete record {id}? (y/n): ");
       string? input=Console.ReadLine();

       if(input?.ToLower() !="y")
        {
             Console.WriteLine("\nDelete cancelled.");
                Pause();
                return;
        }
       
       _repository.Delete(id);
       System.Console.WriteLine($"Record {id} delete");

        Pause();

    }

    private void UpdateRecord()
    {
        if(!ShowRecords())
        {
            Pause();
            return;
        }

        int id=GetNumberInput("\nEnter The id of the record you want to update: ");

        if(! _repository.Exists(id))
        {
            System.Console.WriteLine($"\nRecord {id} doesn't exist");
            Pause();
            return;
        }

        DateTime date =GetDateInput();
        int quantity=GetNumberInput("\nEnter the new quantity :");

        var record=new DrinkingWater
        {
            Id=id,
            Date=date,
            Quantity=quantity
        };

        _repository.Update(record);
        Pause();
    }

    

    private void Pause()
    {
        System.Console.WriteLine("Please press any Key to continue....");
        Console.ReadKey();
    }
}