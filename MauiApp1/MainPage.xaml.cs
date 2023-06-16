using SQLite;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    public static SampleDemoDatabase database;
    public static SampleDemoDatabase Database
    {
        get
        {
            if (database == null)
                database = new SampleDemoDatabase();
            return database;
        }
    }
    public MainPage()
	{
		InitializeComponent();
        DatePicker dp1 = new DatePicker();
        var items = Database.GetItems();
        foreach (var item in items)
        {
            dp1.Date = item.BillDate;
        }
        this.Content = dp1;
    }
}
public class OrderItem
{
    public DateTime BillDate { get; set; }

}
public class SampleDemoDatabase
{
    static object locker = new object();
    SQLiteConnection database;
    public SampleDemoDatabase()
    {
        string dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "mydatabase3.db");
        SQLiteConnection connection = new SQLiteConnection(dbPath);
        database = connection;
        // Creating the table
        database.CreateTable<OrderItem>();

        // Inserting items into table
        database.Query<OrderItem>("INSERT INTO OrderItem(BillDate) values('2023-06-12')");

    }

    public IEnumerable<OrderItem> GetItems()
    {
        lock (locker)
        {
            var table = from i in database.Table<OrderItem>() select i;
            return table;
        }
    }



}

