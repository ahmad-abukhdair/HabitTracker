using HabitTracker;
using Microsoft.Data.Sqlite;

var repository = new HabitRepository(@"Data Source=HabitTracker.db");

repository.CreateTable();

var menu = new Menu(repository);
menu.Run();