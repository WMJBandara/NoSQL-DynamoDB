using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public interface IAddItem
    {
        Task AddItemToTable(int id, string registerDateTime, double price);
    }
}
