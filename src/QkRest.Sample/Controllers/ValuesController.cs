using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace QkRest.Sample.Controllers
{
    /// <summary>
    /// Values API.
    /// </summary>
    public class ValuesController : Controller
    {
        /// <summary>
        /// Get list of values.
        /// </summary>
        /// <returns>List of available values.</returns>
        [HttpGet("values")]
        public QkResponse<List<string>> Get()
        {
            var values = new List<string> { "value1", "value2" };
            return new QkResponse<List<string>>(values);
        }

        /// <summary>
        /// Get value by id.
        /// </summary>
        /// <param name="id">Value id.</param>
        /// <returns>Value.</returns>
        [HttpGet("values/{id}")]
        public QkResponse<string> Get(int id)
        {
            return new QkResponse<string>("value");
        }
    }
}
