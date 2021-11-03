using CRUD.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Services.Interfaces
{
    public interface IDynamicService
    {
        public Task<object> AddAsync(string tableName, string model);
        public Task<object> GetAsync(string tableName, int page = 1, int count = 10);
        public Task<object> GetByIdAsync(string tableName, int id);
        public Task<bool> RemoveAsync(string tableName, int id);
        public Task<object> UpdateAsync(string tableName, int id, string model);
        Task<List<TableSchemaModel>> GetAllTablesAsync();
        Task<List<FieldSchemaModel>> GetFieldSchemaAsync(string tableName);
    }
}
