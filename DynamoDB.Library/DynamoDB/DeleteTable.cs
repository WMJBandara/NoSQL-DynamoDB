using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public class DeleteTable : IDeleteTable
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public DeleteTable(IAmazonDynamoDB amazonDynamoDB)
        {
            _amazonDynamoDB = amazonDynamoDB;
        }

        public async Task<DeleteTableResponse> DeleteTableExecution(string tableName)
        {
            var request = new DeleteTableRequest
            {
                TableName = tableName
            };

            var response = await _amazonDynamoDB.DeleteTableAsync(request);
            return response;
        }
    }
}
