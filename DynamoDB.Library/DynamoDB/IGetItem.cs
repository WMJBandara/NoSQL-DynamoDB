using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public interface IGetItem
    {
        Task<DynamoDBTableItem> GetItems(int? id);
    }
}
