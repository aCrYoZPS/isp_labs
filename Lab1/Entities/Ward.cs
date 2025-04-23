using SQLite;

namespace Lab1.Entities;

[Table("Wards")]
public class Ward
{
    [PrimaryKey, AutoIncrement, Indexed]
    public int Id { get; set; }

    public int Number { get; set; }
    public int PatientCount { get; set; }
}