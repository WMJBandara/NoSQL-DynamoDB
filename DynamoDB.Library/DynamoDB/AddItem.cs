using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public class AddItem : IAddItem
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public AddItem(IAmazonDynamoDB amazonDynamoDB)
        {
            _amazonDynamoDB = amazonDynamoDB;
        }

        public async Task AddItemToTable(int id, string registerDateTime, double price)
        {
            var itemRequest = PutItemRequest(id, registerDateTime, price);
            await PutItem(itemRequest);
        }

        private PutItemRequest PutItemRequest(int id, string regDate, double price)
        {
            var request = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue{ N = id.ToString() }},
                {"RegisterDateTime", new AttributeValue{N = regDate} },
                {"Price", new AttributeValue{N = price.ToString()} }
            };

            return new PutItemRequest
            {
                TableName = "MyTest",
                Item = request
            };
        }

        private async Task PutItem(PutItemRequest itemRequest)
        {
            await _amazonDynamoDB.PutItemAsync(itemRequest);
        }
    }
}
