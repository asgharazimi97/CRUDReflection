using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<T> AddAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task<T> GetByIdAsync(string id);
        public Task<List<T>> GetAsync(string page = "1", string count = "10");
        public Task<bool> RemoveAsync(T entity);
    }
}
