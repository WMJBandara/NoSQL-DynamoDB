using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public interface IDeleteTable
    {
        Task<DeleteTableResponse> DeleteTableExecution(string tableName);
    }
}