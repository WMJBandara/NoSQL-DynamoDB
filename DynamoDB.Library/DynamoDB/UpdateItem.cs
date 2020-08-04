using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDB.Library.DynamoDB
{
    public class UpdateItem : IUpdateItem
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;
        private readonly IGetItem _getItem;
        public UpdateItem(IAmazonDynamoDB amazonDynamoDB, IGetItem getItem)
        {
            _amazonDynamoDB = amazonDynamoDB;
            _getItem = getItem;
        }

        public async Task<Item> UpdateItems(int id, double price)
        {
            var response = _getItem.GetItems(id);
            var currentPrice = response.Result.Items.Select(z => z.Price).FirstOrDefault();
            var registerDateTime = response.Result.Items.Select(z => z.RegisterDateTime).FirstOrDefault();
            var request = GetUpdateItemRequest(id, currentPrice, registerDateTime, price);

            var client = new AmazonDynamoDBClient();
            var result = await UpdateItemAsyns(request);
            return new Item
            {
                Id = Convert.ToInt32(result.Attributes["Id"].N),
                Price = Convert.ToDouble(result.Attributes["Price"].N),
                RegisterDateTime = result.Attributes["RegisterDateTime"].N.ToString()
            };
        }

        private async Task<UpdateItemResponse> UpdateItemAsyns(UpdateItemRequest updateItemRequest)
        {
            try
            {
                var response = await _amazonDynamoDB.UpdateItemAsync(updateItemRequest);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private UpdateItemRequest GetUpdateItemRequest(int id, double currentPrice, string registerDateTime, double newPrice)
        {

            //var client = new AmazonDynamoDBClient();
            //          var request = new UpdateItemRequest
            //          {
            //              TableName = "ProductCatalog",
            //              Key = new Dictionary<string, AttributeValue>
            //{
            //   { "Id", new AttributeValue { N = "301" } }
            //},
            //              ExpressionAttributeNames = new Dictionary<string, string>
            //{
            //  { "#title", "Title" }
            //},
            //              ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            //{
            //  { ":newproduct", new AttributeValue { S = "18\" Girl's Bike" } }
            //},
            //              UpdateExpression = "SET #title = :newproduct"
            //          };
            //          client.UpdateItem(request);

            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { N = id.ToString() }},
                    { "RegisterDateTime", new AttributeValue { N = registerDateTime }}
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#price", "Price" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":newprice", new AttributeValue { N = newPrice.ToString() } }
                    //{ ":currentprice", new AttributeValue { N = currentPrice.ToString() } }
                },
                UpdateExpression = "SET #price = :newprice",
                //ConditionExpression = "#P = :currentprice",
                TableName = "MyTest",
                ReturnValues = "ALL_NEW"
            };

            return request;
        }
    }
}
