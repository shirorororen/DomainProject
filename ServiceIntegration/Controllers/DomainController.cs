using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceIntegration.Logic.Interface;

namespace ServiceIntegration.API.Controllers
{
    [Route("api/service/domain")]
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IDomainService domainService;

        public DomainController(IDomainService domainService)
        {
            this.domainService = domainService;
        }

        /// <summary>
        /// Get all services result for a single domain
        /// </summary>
        /// <param name="domain">Domain</param>
        /// <returns>All services result</returns>
        [HttpGet("{domain}")]
        public async Task<IActionResult> Get(string domain)
        {
            var response = await domainService.ProcessServices(domain);
            return Ok(response);
        }

        /// <summary>
        /// Get all services result for multiple domain
        /// </summary>
        /// <param name="domains">List of domains</param>
        /// <returns>All services for list of domains</returns>
        [HttpGet("domains")]
        public async Task<IActionResult> GetAll([FromBody] string[] domains)
        {
            var response = await domainService.ProcessServices(domains);
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpGet("{domain}/{service}")]
        public IActionResult GetService(string domain, string service)
        {
            throw new NotImplementedException();
        }
    }
}