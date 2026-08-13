using DataBase;

namespace Services
{
    public interface IDataBaseService : IService
    {
        SpreadsheetContent Content { get; }
    }
}