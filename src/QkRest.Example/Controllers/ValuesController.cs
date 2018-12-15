using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace QkRest.Example.Controllers
{
    /// <summary>
    /// Values APIs
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public QkResponse<List<string>> Get()
        {
            var list = new List<string> { "value1", "value2" };
            return new QkResponse<List<string>>(list);
        }

        /// <summary>
        /// Get value by id
        /// </summary>
        /// <param name="id">Value by id</param>
        /// <returns>List of values</returns>
        [HttpGet("{id}")]
        public QkResponse<string> Get(int id)
        {
            return new QkResponse<string>(id.ToString());
        }
    }
}
