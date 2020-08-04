using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DynamoDB.Library.DynamoDB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamoDBController : ControllerBase
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private readonly IDynamoDBExamples _dynamoDBExamples;
        private readonly IDeleteTable _deleteTable;
        private readonly IAddItem _addItem;
        private readonly IGetItem _getItem;
        private readonly IUpdateItem _updateItem;
        private readonly IDeleteItem _deleteItem;

        public DynamoDBController(IDynamoDBExamples dynamoDBExamples, IDeleteTable deleteTable, IAddItem addItem, IGetItem getItem, IUpdateItem updateItem, IDeleteItem deleteItem)
        {
            _dynamoDBExamples = dynamoDBExamples;
            _deleteTable = deleteTable;
            _addItem = addItem;
            _getItem = getItem;
            _updateItem = updateItem;
            _deleteItem = deleteItem;
        }

        [Route("createtable")]
        public ActionResult CreateTable()
        {
            _dynamoDBExamples.CreateDynamoDBTable();
            return Ok();
        }

        [HttpDelete]
        [Route("delete-table")]
        public async Task<IActionResult> DeleteTable(string tableName)
        {
            var response = await _deleteTable.DeleteTableExecution(tableName);
            return Ok(response);
        }

        [Route("putitems")]
        public IActionResult AddItem([FromQuery] int id, string regdate, double price)
        {
            _addItem.AddItemToTable(id, regdate, price);
            return Ok();
        }

        [Route("getitems")]
        public async Task<IActionResult> GetItems([FromQuery] int? id)
        {
            var response = await _getItem.GetItems(id);
            return Ok(response);
        }

        [HttpPut]
        [Route("updateitems")]
        public async Task<IActionResult> UpdateItems([FromQuery] int id, double price)
        {
            var response = await _updateItem.UpdateItems(id, price);
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete-item")]
        public async Task<IActionResult> DeleteItems([FromQuery] int id)
        {
            var response = await _deleteItem.DeleteItems(id);
            return Ok(response);
        }

        private void TestCRUDOperations(DynamoDBContext context)
        {
            int bookID = 1001; // Some unique value.
            Book myBook = new Book
            {
                Id = bookID,
                Title = "object persistence-AWS SDK for.NET SDK-Book 1001",
                ISBN = "111-1111111001",
                BookAuthors = new List<string> { "Author 1", "Author 2" },
            };

            // Save the book.
            context.SaveAsync(myBook);
            // Retrieve the book.
            Task<Book> bookRetrieved = context.LoadAsync<Book>(bookID);

            // Update few properties.
            bookRetrieved.Result.ISBN = "222-2222221001";
            bookRetrieved.Result.BookAuthors = new List<string> { " Author 1", "Author x" }; // Replace existing authors list with this.
            context.SaveAsync(bookRetrieved);

            // Retrieve the updated book. This time add the optional ConsistentRead parameter using DynamoDBContextConfig object.
            Task<Book> updatedBook = context.LoadAsync<Book>(bookID, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });

            // Delete the book.
            context.DeleteAsync<Book>(bookID);
            // Try to retrieve deleted book. It should return null.
            Task<Book> deletedBook = context.LoadAsync<Book>(bookID, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            if (deletedBook == null)
                Console.WriteLine("Book is deleted");
        }
    }
}