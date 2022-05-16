using AslWebApi.DAL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AslWebApi.Services
{
    public interface IJsonDBService
    {
        /// <summary>
        /// Get the column names of the given entity type from database
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>List of column name strings</returns>
        public List<string> GetColumnNames<T>();
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Primary Key ID</param>
        /// <returns>JSON string of the data</returns>
        public string GetKeyValuePairOfData<T>(int id) where T : class;
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Primary Key ID</param>
        /// <returns>JSON string of the data</returns>
        public Task<string> GetKeyValuePairOfDataAsync<T>(int id) where T : class;
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Primary Key ID</param>
        /// <returns>JSON string of the data</returns>
        public string GetKeyValuePairOfData<T>(string id) where T : class;
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Primary Key ID</param>
        /// <returns>JSON string of the data</returns>
        public Task<string> GetKeyValuePairOfDataAsync<T>(string id) where T : class;
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="query">sql query to find the data from the database</param>
        /// <returns></returns>
        public string GetKeyValuePairOfDataWithSQL<T>(string query) where T : class;
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="query">sql query to find the data from the database</param>
        /// <returns></returns>
        public Task<string> GetKeyValuePairOfDataWithSQLAsync<T>(string query) where T : class;

    }

    public class JsonDBService : IJsonDBService
    {
        private DatabaseContext context;

        public JsonDBService(DatabaseContext dc)
        {
            context = dc;
        }

        /// <summary>
        /// Get the column names of the given entity type from database
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>List of column name strings</returns>
        public List<string> GetColumnNames<T>()
        {
            List<string> columnNameList = new List<string>();
            var entityType = context.Model.FindEntityType(typeof(T));
            if (entityType == null) return columnNameList;
            // Table info 
            var tableName = entityType.GetTableName();
            var tableSchema = entityType.GetSchema();
            // Column info 
            foreach (var property in entityType.GetProperties())
            {
                var columnName = property!.GetColumnName();
                var columnType = property!.GetColumnType();
                columnNameList.Add(columnName); ;
            };

            return columnNameList;
        }

        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Primary Key ID</param>
        /// <returns>JSON string of the data</returns>
        public string GetKeyValuePairOfData<T>(int id) where T : class
        {
            var a = context.Set<T>().Find(id);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Primary Key ID</param>
        /// <returns>JSON string of the data</returns>
        public async Task<string> GetKeyValuePairOfDataAsync<T>(int id) where T : class
        {
            var a = await context.Set<T>().FindAsync(id);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Primary Key ID</param>
        /// <returns>JSON string of the data</returns>
        public string GetKeyValuePairOfData<T>(string id) where T : class
        {
            var a = context.Set<T>().Find(id);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Primary Key ID</param>
        /// <returns>JSON string of the data</returns>
        public async Task<string> GetKeyValuePairOfDataAsync<T>(string id) where T : class
        {
            var a = await context.Set<T>().FindAsync(id);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="query">sql query to find the data from the database</param>
        /// <returns></returns>
        public string GetKeyValuePairOfDataWithSQL<T>(string query) where T : class
        {
            var a = context.Set<T>().FromSqlRaw(query);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        /// <summary>
        /// Gets the data in JSON format from databaase.
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="query">sql query to find the data from the database</param>
        /// <returns></returns>
        public async Task<string> GetKeyValuePairOfDataWithSQLAsync<T>(string query) where T : class
        {
            var a = await context.Set<T>().FromSqlRaw(query).FirstOrDefaultAsync();
            var b = JsonConvert.SerializeObject(a);
            return b;
        }

    }
}
