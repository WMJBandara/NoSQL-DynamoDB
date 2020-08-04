using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public class DeleteItem : IDeleteItem
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;
        private readonly IGetItem _getItem;

        public DeleteItem(IAmazonDynamoDB amazonDynamoDB, IGetItem getItem)
        {
            _amazonDynamoDB = amazonDynamoDB;
            _getItem = getItem;
        }

        public async Task<Item> DeleteItems(int id)
        {
            var deleteItem = _getItem.GetItems(id);
            var registerDateTime = deleteItem.Result.Items.Select(x => x.RegisterDateTime).FirstOrDefault();
            var deleteItemRequest = DeleteItemRequest(id, registerDateTime);
            var response = await _amazonDynamoDB.DeleteItemAsync(deleteItemRequest);
            return new Item
            {
                Id = Convert.ToInt32(response.Attributes["Id"].N),
                Price = Convert.ToDouble(response.Attributes["Price"].N),
                RegisterDateTime = response.Attributes["RegisterDateTime"].N
            };
        }

        private DeleteItemRequest DeleteItemRequest(int id, string registeredDate)
        {
            var request = new DeleteItemRequest()
            {
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue{ N = id.ToString()}},
                    {"RegisterDateTime", new AttributeValue{ N = registeredDate}}
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#id", "Id"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":newid", new AttributeValue{N = id.ToString() }}
                },
                TableName = "MyTest",
                ReturnValues = "ALL_OLD",
                ConditionExpression = "#id = :newid"
            };
            return request;
        }
    }
}
