using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CofigurationApi.Controllers
{
    [ApiController]
    [Route("configuration/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly ConfigurationService _configurationService;

        public ConfigurationController(ILogger<ConfigurationController> logger, ConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Gets a configuration based on name and version
        /// </summary>
        /// 
        [HttpGet("/configurations/{name}/{version}")]
        public ActionResult<IEnumerable<string>> Get(string name, string version)
        {

            return null;

        }

    }
}
