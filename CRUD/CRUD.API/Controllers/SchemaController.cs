using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD.Entities.Models;
using CRUD.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly IDynamicService _dynamicService;

        public SchemaController(IDynamicService dynamicService)
        {
            _dynamicService = dynamicService;
        }

        [HttpGet("Tables")]
        public async Task<List<TableSchemaModel>> GetTablesAsync()
        {
            return await _dynamicService.GetAllTablesAsync();
        }

        [HttpGet("{tableName}")]
        public async Task<List<FieldSchemaModel>> GetTableSchema(string tableName)
        {
            return await _dynamicService.GetFieldSchemaAsync(tableName);
        }
    }
}