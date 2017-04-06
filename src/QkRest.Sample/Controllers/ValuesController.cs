using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace QkRest.Sample.Controllers
{
    public class ValuesController : Controller
    {
        [HttpGet, Route("values")]
        public QkResponse<List<string>> Get()
        {
            var values = new List<string> { "value1", "value2" };
            return new QkResponse<List<string>>(values);
        }

        [HttpGet("values/{id}")]
        public QkResponse<string> Get(int id)
        {
            return new QkResponse<string>("value");
        }
    }
}
