using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CRUD.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using CRUD.Entities;
using System.Linq;

namespace CRUD.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CRUDContext dbContext;

        public Repository(CRUDContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAsync(string page = "1", string count = "10")
        {
            int p = Convert.ToInt32(page);
            int c = Convert.ToInt32(count);
            if (p <= 0 || c <= 0)
                return null;

            var query = dbContext.Set<T>().AsQueryable();
            var data = await query.Skip((p - 1) * c).Take(c).ToListAsync();
            return data;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var result = await dbContext.Set<T>().FindAsync(Convert.ToInt32(id));
            if (result != null)
                dbContext.Entry(result).State = EntityState.Detached;
            return result;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            try
            {
                dbContext.Remove(entity);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        //public async Task<List<TableSchema>> GetAllTablesAsync()
        //{
        //    return await dbContext.TableSchemas.ToListAsync();
        //}
        //public async Task<List<FieldSchema>> GetFieldSchemaAsync(string tableName)
        //{
        //    return await dbContext.FieldSchemas.Where(x => x.TableSchema.EnglishNmae == tableName).ToListAsync();
        //}
    }
}
