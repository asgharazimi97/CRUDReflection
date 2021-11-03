using CRUD.Entities.Entity;
using CRUD.Entities.Models;
using CRUD.Entities.Repository;
using CRUD.Entities.Store.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Entities.Store
{
    public class DynamicStore:IDynamicStore
    {
        private readonly CRUDContext _dbContext;
        public DynamicStore( CRUDContext dbContext)
        {
            _dbContext = dbContext;
        }


        private InstanceModel GenerateInstance(string tableName)
        {
            tableName = tableName[0].ToString().ToUpper() + tableName.Substring(1);
            var entityType = Assembly.GetAssembly(typeof(BaseEntity)).GetType("CRUD.Entities.Entity." + tableName);
            object instance = Activator.CreateInstance(entityType);
            Type genericType = typeof(Repository<>);
            Type repositoryType = genericType.MakeGenericType(instance.GetType());
            object repository = Activator.CreateInstance(repositoryType, _dbContext);
            return new InstanceModel { RepositoryType = repositoryType, Repository = repository };
        }

        private object Deserialize(string tableName, string model)
        {
            tableName = tableName[0].ToString().ToUpper();
            var entityType = Assembly.GetAssembly(typeof(BaseEntity)).GetType("CRUD.Entities.Entity." + tableName);
            var result = JsonConvert.DeserializeObject(model, entityType);
            return result;
        }


        public async Task<object> AddAsync(string tableName, string model)
        {
            dynamic values = Newtonsoft.Json.Linq.JObject.Parse(model);
            var jsonModel = Deserialize(tableName, model);
            var instance = GenerateInstance(tableName);
            MethodInfo genericMethod = instance.RepositoryType.GetMethod("AddAsync");
            var task = (Task)genericMethod.Invoke(instance.Repository, new[] { jsonModel });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }

        public async Task<object> GetAsync(string tableName, int page = 1, int count = 10)
        {
            var instance = GenerateInstance(tableName);

            MethodInfo genericMethod = instance.RepositoryType.GetMethod("GetAsync");
            var task = (Task)genericMethod.Invoke(instance.Repository, new[] { page.ToString(), count.ToString() });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }

        public async Task<object> GetByIdAsync(string tableName, int id)
        {
            var instance = GenerateInstance(tableName);
            MethodInfo genericMethod = instance.RepositoryType.GetMethod("GetByIdAsync");
            var task = (Task)genericMethod.Invoke(instance.Repository, new[] { id.ToString() });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }


        public async Task<bool> RemoveAsync(string tableName, string model)
        {
            var jsonModel = Deserialize(tableName, model);

            var instance = GenerateInstance(tableName);
            MethodInfo genericMethod = instance.RepositoryType.GetMethod("RemoveAsync");
            var task = (Task)genericMethod.Invoke(instance.Repository, new[] { jsonModel });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return Convert.ToBoolean(resultProperty.GetValue(task));
        }

        public async Task<object> UpdateAsync(string tableName, string model)
        {
            var jsonModel = Deserialize(tableName, model);
            var instance = GenerateInstance(tableName);
            MethodInfo genericMethod = instance.RepositoryType.GetMethod("UpdateAsync");
            var task = (Task)genericMethod.Invoke(instance.Repository, new[] { jsonModel });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }

        public async Task<List<TableKey>> FindKeys(string entityName)
        {
            var entity = Assembly.GetAssembly(typeof(BaseEntity)).GetType("CRUD.Entities.Entity." + entityName);
            var entityType = _dbContext.Model.FindEntityType(entity);
            var tableName = entityType.GetTableName();
            string query = $@"SELECT cp.name'ColumnName',tr.name 'RefrencedTable',cr.name'RefrencedColumnName',tp.name 'CurrentTable'
FROM
    sys.foreign_keys fk
INNER JOIN
    sys.tables tp ON fk.parent_object_id = tp.object_id
INNER JOIN
    sys.tables tr ON fk.referenced_object_id = tr.object_id
INNER JOIN
    sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
INNER JOIN
    sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
INNER JOIN
    sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
WHERE tp.name ='{tableName}'";
            return await _dbContext.Set<TableKey>().FromSqlRaw(query).ToListAsync();
        }

    }
}
