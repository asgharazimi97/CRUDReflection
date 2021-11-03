using CRUD.Entities.Entity;
using CRUD.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Entities.Store.Interfaces
{
    public interface IDynamicStore
    {
        public Task<object> AddAsync(string tableName, string model);
        public Task<object> GetAsync(string tableName, int page = 1, int count = 10);
        public Task<object> GetByIdAsync(string tableName, int id);
        public Task<bool> RemoveAsync(string tableName, string model);
        public Task<object> UpdateAsync(string tableName, string model);
        Task<List<TableKey>> FindKeys(string entityName);
    }
}
