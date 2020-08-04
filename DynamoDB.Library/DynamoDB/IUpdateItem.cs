using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public interface IUpdateItem
    {
        Task<Item> UpdateItems(int id, double price);
    }
}
