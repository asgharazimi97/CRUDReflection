using CRUD.Services.Interfaces;
using System;
using System.Threading.Tasks;
using CRUD.Entities;
using CRUD.Entities.Store.Interfaces;
using CRUD.Entities.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CRUD.Services
{
    public class DynamicService: IDynamicService
    {
        private readonly IDynamicStore _basisDynamicStore;
        //private readonly IMapper _mapper;

        public DynamicService(IDynamicStore dynamicBasisStore)
        {
            _basisDynamicStore = dynamicBasisStore;
        }

        public async Task<object> AddAsync(string tableName, string model)
        {
            return await _basisDynamicStore.AddAsync(tableName, model);
        }

        public async Task<object> GetAsync(string tableName, int page = 1, int count = 10)
        {
            return await _basisDynamicStore.GetAsync(tableName, page, count);
        }

        public async Task<object> GetByIdAsync(string tableName, int id)
        {
            return await _basisDynamicStore.GetByIdAsync(tableName, id);
        }

        public async Task<bool> RemoveAsync(string tableName, int id)
        {
            var checkRecord = await GetByIdAsync(tableName, id);
            if (checkRecord == null)
                return false;

            string model = "{\"id\":" + id + "}";
            return await _basisDynamicStore.RemoveAsync(tableName, model);
        }

        public async Task<object> UpdateAsync(string tableName, int id, string model)
        {
            var checkRecord = await GetByIdAsync(tableName, id);
            if (checkRecord == null)
                return null;

            return await _basisDynamicStore.UpdateAsync(tableName, model);
        }

        public async Task<List<TableSchemaModel>> GetAllTablesAsync()
        {
            var entities=AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(t => t.IsClass && t.Namespace == "CRUD.Entities.Entity").ToList();
            var result = new List<TableSchemaModel>();
            foreach (var item in entities)
            {
                result.Add(new TableSchemaModel { EnglishNmae=item.Name});
            }
            return result;
        }

        //public async Task<List<TableSchemaModel>> GetAllTablesAsync()
        //{
        //    var entityType = Assembly.GetAssembly(typeof(BaseEntity)).GetType("CRUD.Entities.Entity");
        //    return null;
        //    //return await _basisDynamicDataStore.GetAllTablesAsync();
        //}

        public async Task<List<FieldSchemaModel>> GetFieldSchemaAsync(string tableName)
        {
            Type myType = Assembly.GetAssembly(typeof(BaseEntity)).GetType("CRUD.Entities.Entity." + tableName);

            // Get the fields of the specified class.
            var fields = myType.GetProperties().ToList();

            if (fields == null && !fields.Any())
                return null;

            fields = fields.Where(x => x.PropertyType.Namespace == "System" || x.PropertyType.IsEnum).ToList();
            var getKeys =await _basisDynamicStore.FindKeys(tableName);

            var output = new List<FieldSchemaModel>();

            foreach (var item in fields)
            {
                var oneModel = new FieldSchemaModel { EnglishName = item.Name, DataType = item.PropertyType.Name };
                var checkIsKey = getKeys.Where(x => x.ColumnName == item.Name).FirstOrDefault();
                if (checkIsKey != null)
                {
                    var entity = Assembly.GetAssembly(typeof(BaseEntity)).GetType("CRUD.Entities.CRUDContext");
                    string refrenceTableName = entity.GetProperties().FirstOrDefault(x=>x.Name==checkIsKey.RefrencedTable).PropertyType.GenericTypeArguments.FirstOrDefault().Name;
                    oneModel.ForeignKeyTable = refrenceTableName;
                    oneModel.IsForeignKey = true;
                }
                Type checkEnum = Assembly.GetAssembly(typeof(BaseEntity)).GetType(item.PropertyType.FullName);
                if (checkEnum != null && checkEnum.GetFields().Any())
                {
                    oneModel.IsEnum = true;
                    oneModel.EnumInfo = new List<EnumModel>();
                    foreach (var enu in checkEnum.GetFields())
                    {
                        if (enu.FieldType.IsEnum)
                        {
                            var enumaData = new EnumModel
                            {
                                Name = enu.Name,
                                Value = enu.GetRawConstantValue()
                            };
                            oneModel.EnumInfo.Add(enumaData);
                        }
                    }
                }
                output.Add(oneModel);
            }

            return output;
        }
    }
}
