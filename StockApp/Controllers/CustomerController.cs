using App.Authorization;
using AutoMapper;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockApp.Helpers;
using StockApp.Models;
using StockApp.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Controllers
{
    [Route("api/Customers")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        #region Injection & Properties
        private IMapper _mapper;
        private IUnitOfWork _uow;
        private readonly string sharedKey;

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IUnitOfWork uow, IMapper mapper,
                ILoggerFactory loggerFactory, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _uow = uow;

            _logger = loggerFactory.CreateLogger<CustomerController>();
            sharedKey = accessor?.HttpContext?.User?.FindFirst("Key")?.Value;

        }
        #endregion

        [HttpGet("")]
        public IActionResult Index([FromBody] CustomerResourceParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(sharedKey))
                return BadRequest();

            var result = new Result() { Success = true };
            try
            {
                if (parameters == null)
                    parameters = new CustomerResourceParameters();

                parameters.Key = sharedKey;
                var customers = _uow.Customers.GetAllCustomers(parameters);

                var customerList = _mapper.Map<IEnumerable<CustomerModel>>(customers).AsQueryable();
                result.Data = customerList.GetPaged(parameters.PageNo, parameters.PageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.AddError(ex.Message);
            }

            return BadRequest(result);
        }

    }
}
