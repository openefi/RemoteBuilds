using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static OpenEFI_RemoteBuild.Workers.ProcessWorkers;
using static OpenEFI_RemoteBuild.Utils.RequestUtils;
using static OpenEFI_RemoteBuild.DB.DBController;
using OpenEFI_RemoteBuild.Utils;

namespace OpenEFI_RemoteBuild.Controllers
{
    [Route("[controller]")]
    public class BuildController : ControllerBase
    {

        private readonly ILogger<BuildController> _logger;

        public BuildController(ILogger<BuildController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id:required}")]
        public IActionResult Get(string id)
        {
            if (id.Length == 0) return BadRequest(new { errorCode = 0, errorTXT = "dame el ID no sea tarado mijo" });
            IBuildStatus data = BuildStatus(id);

            if (data != null)
            {
                return Ok(data);
            }

            return NotFound(
                new
                {
                    errorCode = 1,
                    errorMSG = $"no se encuentra build para el ID {id}"
                });
        }

        [HttpPost()]
        public IActionResult Post([FromBody] BuildRequest req_in)
        {
            if (IsAnyNullOrEmpty(req_in))
            {
                return BadRequest(new { errorCode = 1, errorTxt = "No me voy a procesar nada tu request!!! SON INMUNDAS TUS REQUEST, SON INSULSAS, NO SON NADA DELICIOSAS! PORQUE VOS COCINÁS HORRIBLE!!" });
            }
            string _build = MakeBuild(_logger, req_in);
            return Accepted(new { build_ID = _build });
        }
    }
}
