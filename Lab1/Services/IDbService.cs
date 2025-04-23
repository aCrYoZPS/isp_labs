using Lab1.Entities;

namespace Lab1.Services;

public interface IDbService
{
    IEnumerable<Ward> GetAllWards();
    IEnumerable<Patient> GetWardPatients(int wardId);
    void Init();
}