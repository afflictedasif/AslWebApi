using AslWebApi.DAL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AslWebApi.Services
{
    public interface IJsonDBService
    {
        public List<string> GetColumnNames<T>();
        public string GetKeyValuePairOfData<T>(int id) where T : class;
        public Task<string> GetKeyValuePairOfDataAsync<T>(int id) where T : class;
        public string GetKeyValuePairOfData<T>(string id) where T : class;
        public Task<string> GetKeyValuePairOfDataAsync<T>(string id) where T : class;
        public string GetKeyValuePairOfDataWithSQL<T>(string query) where T : class;
        public Task<string> GetKeyValuePairOfDataWithSQLAsync<T>(string query) where T : class;

    }

    public class JsonDBService : IJsonDBService
    {
        private DatabaseContext context;

        public JsonDBService(DatabaseContext dc)
        {
            context = dc;
        }

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

        public string GetKeyValuePairOfData<T>(int id) where T : class
        {
            var a = context.Set<T>().Find(id);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        public async Task<string> GetKeyValuePairOfDataAsync<T>(int id) where T : class
        {
            var a = await context.Set<T>().FindAsync(id);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        public string GetKeyValuePairOfData<T>(string id) where T : class
        {
            var a = context.Set<T>().Find(id);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        public async Task<string> GetKeyValuePairOfDataAsync<T>(string id) where T : class
        {
            var a = await context.Set<T>().FindAsync(id);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }

        public string GetKeyValuePairOfDataWithSQL<T>(string query) where T : class
        {
            var a = context.Set<T>().FromSqlRaw(query);
            var b = JsonConvert.SerializeObject(a);
            return b;
        }
        public async Task<string> GetKeyValuePairOfDataWithSQLAsync<T>(string query) where T : class
        {
            var a = await context.Set<T>().FromSqlRaw(query).FirstOrDefaultAsync();
            var b = JsonConvert.SerializeObject(a);
            return b;
        }

    }
}
