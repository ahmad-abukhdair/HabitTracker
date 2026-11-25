
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class HabitRepository
{
    private readonly string _connectionString;

    public HabitRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var TableCmd = connection.CreateCommand();

            TableCmd.CommandText =
            @"Create Table IF NOT Exists DRINKING_WATER(
            
            Id Integer Primary Key AutoIncrement,

            Date Text,
            Quantity Integer
            )";

            TableCmd.ExecuteNonQuery();
        }
    }

    public void Insert(DrinkingWater record)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var InsertCmd = connection.CreateCommand();

            InsertCmd.CommandText = @"Insert Into Drinking_Water(Date,Quantity)
                                 Values(@date,@quantity)";

            InsertCmd.Parameters.AddWithValue("@date", record.Date.ToString("dd-MM-yy"));
            InsertCmd.Parameters.AddWithValue("@quantity", record.Quantity);
            InsertCmd.ExecuteNonQuery();
        }
    }

    public List<DrinkingWater> GetAll()
    {
        var records = new List<DrinkingWater>();
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var SelectCmd = connection.CreateCommand();
            SelectCmd.CommandText = @"Select Id,Date,Quantity
                                 From DRINKING_WATER";

            using (var reader = SelectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var record = new DrinkingWater

                    {

                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", CultureInfo.InvariantCulture),
                        Quantity = reader.GetInt32(2)
                    };
                    records.Add(record);
                }
            }
        }
        return records;
    }


    public bool Delete(int id)
    {
        using (var connection=new SqliteConnection(_connectionString))
        {
            connection.Open();

            var DeleteCmd=connection.CreateCommand();

            DeleteCmd.CommandText=@"Delete From DRINKING_WATER
                                    WHERE Id =@id";
            DeleteCmd.Parameters.AddWithValue("@id",id);

            int RowsAffected=DeleteCmd.ExecuteNonQuery();

            return RowsAffected>0;
        }
    }

    public bool Exists (int id)
    {
        using (var connection=new SqliteConnection(_connectionString))
        {
            connection.Open();
            var CheckCmd=connection.CreateCommand();

            CheckCmd.CommandText= @"Select Count(*) From DRINKING_WATER WHERE Id=@id";
            CheckCmd.Parameters.AddWithValue("@id",id);

            int Count=Convert.ToInt32(CheckCmd.ExecuteScalar());

            return Count>0;
        }
    }

    public void Update(DrinkingWater record)
    {
        using (var connection =new SqliteConnection(_connectionString))
        {
            connection.Open();
            var UpdateCmd=connection.CreateCommand();

            UpdateCmd.CommandText=@"Update DRINKING_WATER Set
                                  Date = @date , 
                                  Quantity= @quantity Where id = @id ";
        UpdateCmd.Parameters.AddWithValue("@date",record.Date.ToString("dd-MM-yy"));
        UpdateCmd.Parameters.AddWithValue("@quantity",record.Quantity);
        UpdateCmd.Parameters.AddWithValue("@id",record.Id);

        UpdateCmd.ExecuteNonQuery();

        }
    }

}

