using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD.Services;
using CRUD.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicController : ControllerBase
    {
        private readonly IDynamicService _dynamicService;

        public DynamicController(IDynamicService dynamicService)
        {
            _dynamicService = dynamicService;
        }

        [HttpGet("{tableName}")]
        public async Task<object> GetAsync([FromRoute]string tableName, int page, int count)
        {
            return await _dynamicService.GetAsync(tableName, page, count);
        }

        [HttpGet("{tableName}/{id}")]
        public async Task<object> GetByIdAsync([FromRoute]string tableName, [FromRoute]int id)
        {
            return await _dynamicService.GetByIdAsync(tableName, id);
        }

        [HttpPut("{tableName}/{id}")]
        public async Task UpdateAsync([FromRoute]string tableName, [FromRoute] int id, [FromBody] object model)
        {
            await _dynamicService.UpdateAsync(tableName, id, model.ToString());
        }

        [HttpDelete("{tableName}/{id}")]
        public async Task RemoveAsync([FromRoute]string tableName, [FromRoute] int id)
        {
            await _dynamicService.RemoveAsync(tableName, id);
        }

        [HttpPost("{tableName}")]
        public async Task<object> AddAsync([FromRoute]string tableName, [FromBody]object model)
        {
            return await _dynamicService.AddAsync(tableName, model.ToString());
        }
    }
}