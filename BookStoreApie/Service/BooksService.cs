using BookStoreApie.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApie.Service
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync() => 
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string Id) =>
            await _booksCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book newBook) => 
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string Id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == Id, updatedBook);

        public async Task RemoveAsync(string Id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == Id);
    }
}
