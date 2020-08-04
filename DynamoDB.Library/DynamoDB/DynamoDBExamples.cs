using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DynamoDB.Library.DynamoDB
{
    public class DynamoDBExamples : IDynamoDBExamples
    {
        private readonly IAmazonDynamoDB amazonDynamoDB;
        private static readonly string tableName = "MyTest";

        public DynamoDBExamples(IAmazonDynamoDB _amazonDynamoDB)
        {
            this.amazonDynamoDB = _amazonDynamoDB;
        }

        public void CreateDynamoDBTable()
        {
            try
            {
                CreateTempTable();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void CreateTempTable()
        {
            Console.WriteLine($"Table - {tableName} is creating");
            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                         AttributeName = "Id",
                         AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "RegisterDateTime",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "RegisterDateTime",
                        KeyType = "Range"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = tableName
            };
            var response = amazonDynamoDB.CreateTableAsync(request);
            WaitUntilTableReady(tableName);
        }

        private void WaitUntilTableReady(string tableName)
        {
            string status = null;
            do
            {
                try
                {
                    Thread.Sleep(5000);
                    var res = amazonDynamoDB.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });
                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {

                }
            } while (status != "ACTIVE");
            {
                Console.WriteLine($"Table {tableName} created successfully");
            }

        }
    }
}
