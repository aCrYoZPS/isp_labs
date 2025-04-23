using Lab1.Entities;
using SQLite;

namespace Lab1.Services;

public class SQLiteService : IDbService
{
    private SQLiteConnection? db;

    public SQLiteService()
    {
        Init();
    }

    public IEnumerable<Ward> GetAllWards()
    {
        if (db == null) return [];

        return db.Table<Ward>();
    }

    public IEnumerable<Patient> GetWardPatients(int wardId)
    {
        if (db == null) return [];

        return db.Table<Patient>().Where(p => p.WardId == wardId);
    }

    public void Init()
    {
        var connectionString = Path.Combine(Globals.WorkingDirectory, Globals.DbName);
        if (File.Exists(connectionString))
        {
            File.Delete(connectionString);
        }

        db = new SQLiteConnection(connectionString);

        db.CreateTable<Ward>();
        db.CreateTable<Patient>();

        try
        {
            db.InsertAll(new List<Ward>
            {
                new Ward
                {
                    Id = 1,
                    Number = 104,
                    PatientCount = 3,
                },
                new Ward
                {
                    Id = 2,
                    Number = 101,
                    PatientCount = 1,
                }
            });
            db.InsertAll(new List<Patient>
            {
                new Patient
                {
                    Id = 1,
                    Name = "Ivan",
                    BirthDate = DateTime.Now.AddYears(-20),
                    WardId = 1,
                },
                new Patient
                {
                    Id = 2,
                    Name = "Peter",
                    BirthDate = DateTime.Now.AddYears(-25).AddDays(-5),
                    WardId = 1,
                },
                new Patient
                {
                    Id = 3,
                    Name = "Sophie",
                    BirthDate = DateTime.Now.AddYears(-25).AddMonths(-2).AddDays(-10),
                    WardId = 2,
                },
                new Patient
                {
                    Id = 4,
                    Name = "Andrew",
                    BirthDate = DateTime.Now.AddYears(-25).AddMonths(-2).AddDays(-10),
                    WardId = 1,
                },
            });
        }
        catch (SQLiteException)
        {
        }
    }
}