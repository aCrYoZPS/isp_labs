using Lab1.Entities;

namespace Lab1.Services;

public interface IRateService
{
    IEnumerable<Rate> GetRates(DateTime date);
}