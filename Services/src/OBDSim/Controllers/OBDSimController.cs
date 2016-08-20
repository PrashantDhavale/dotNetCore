namespace OBDSim.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Providers;
    using Serilog;
    using System;
    using System.Collections.Generic;

    [Route("api")]
    [Produces("application/json")]
    public class OBDSimController : Controller
    {
        #region Members

        private readonly IOBDSimProvider _obdSimProvider;
        private readonly ILogger<OBDSimController> _logger;

        #endregion

        #region Constructor

        public OBDSimController(IOBDSimProvider obdSimProvider, ILogger<OBDSimController> logger)
        {
            _obdSimProvider = obdSimProvider;
            _logger = logger;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("obdsim/{customerId}", Name = "getOBDSims")]
        [ProducesResponseType(typeof(List<OBDSimModel>), 200)]
        public ActionResult GetOBDSims(int customerId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Invoking Provider GetOBDSims method with {0}", customerId);
                    var OBDSimModel = _obdSimProvider.GetOBDSims(customerId);
                    return Ok(OBDSimModel);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get OBDSim details", ex);
                return BadRequest(new { Reason = "Error Occurred" });
            }
        }

        #endregion
    }
}
