using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public class GetItem : IGetItem
    {
        private readonly string tableName = "MyTest";
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public GetItem(IAmazonDynamoDB amazonDynamoDB)
        {
            _amazonDynamoDB = amazonDynamoDB;
        }

        public async Task<DynamoDBTableItem> GetItems(int? id)
        {
            var scanRequest = BuildRequest(id);

            var response = await ScanResponse(scanRequest);

            return new DynamoDBTableItem
            {
                Items = response.Items.Select(MapItem).ToList()
            };
        }

        private Item MapItem(Dictionary<string, AttributeValue> response)
        {
            return new Item
            {
                Id = Convert.ToInt32(response["Id"].N),
                RegisterDateTime = response["RegisterDateTime"].N
            };
        }

        private async Task<ScanResponse> ScanResponse(ScanRequest scanRequest)
        {
            var response = await _amazonDynamoDB.ScanAsync(scanRequest);
            return response;
        }

        public ScanRequest BuildRequest(int? id)
        {
            if (!id.HasValue)
            {
                return new ScanRequest
                {
                    TableName = tableName
                };
            }

            return new ScanRequest
            {
                TableName = tableName,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":v_Id", new AttributeValue{N = id.ToString()}}
                },
                FilterExpression = "Id = :v_Id",
                ProjectionExpression = "Id, RegisterDateTime"
            };
        }
    }
}
