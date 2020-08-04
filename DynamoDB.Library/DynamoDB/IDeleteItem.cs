using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public interface IDeleteItem
    {
        Task<Item> DeleteItems(int id);
    }
}
