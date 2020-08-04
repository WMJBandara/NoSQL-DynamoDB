using Amazon.DynamoDBv2;

namespace DynamoDB.Library.DynamoDB
{
    public class PutItem<T> where T : class
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public PutItem(IAmazonDynamoDB amazonDynamoDB)
        {
            _amazonDynamoDB = amazonDynamoDB;
        }

        //public async Task<T> GetItems(PutItemRequest putItemRequest)
        //{
        //    var response = await _amazonDynamoDB.PutItemAsync(putItemRequest);
        //    //return response.Attributes
        //}


    }
}
