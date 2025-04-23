namespace Lab1.Entities;

using SQLite;

public class Patient
{
    [PrimaryKey, AutoIncrement, Indexed]
    public int Id { get; set; }

    public string Name { get; set; }
    public DateTime BirthDate { get; set; }

    [Indexed]
    public int WardId { get; set; }
}