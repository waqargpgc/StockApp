using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Models;

namespace StockApp.Controllers
{
    [Route("api/endpoints")]
    public class EndPointsInfoController : Controller
    {
        private IUnitOfWork _uow;

        public EndPointsInfoController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        //[Authorize]
        [HttpGet(""), AllowAnonymous]
        [HttpGet("info")]
        public IActionResult Info()
        {
            var resources = new List<ResourceEndpoint> {
                new ResourceEndpoint
                {
                    ResouceName = "Login",
                    Uri = "api/auth/login",
                    Params = new List<object>()
                    {
                       new
                       {
                           name = "UserName",
                           type= "string",
                           optional = false
                       },
                         new
                       {
                           name = "Password",
                           type= "string",
                           optional = false
                       }
                    },
                    SampleUri = new List<string>
                    {
                        "/api/auth/login"
                    },
                    Description = "Http post request, params are posted in the body of the request.",
                    SampleOutput = new List<object>
                    {
                        new {
                                token = "eyJhbGciOiJIUzI.1-D-EbXjUg8a64qxcSw5vAU...",
                                roles = new List<string>
                                {
                                    "abc-e1e3d5d6-75d",
                                    "fgh-e1e3d5d6-75d"
                                },
                                expiresIn = "2019-04-04T22:00:00.5238122+00:00",
                                requestAt = "2019-04-04T16:00:00.5238082+00:00",
                                token_type = "bearer"
                            }
                    }
                },
                new ResourceEndpoint
                {
                    ResouceName = "Create User",
                    Uri = "api/Account/createUser",
                    Params = new List<object>()
                    {
                       new
                       {
                           name = "UserName",
                           type= "string",
                           optional = true
                       }, new
                       {
                           name = "Password",
                           type= "string",
                           optional = false
                       }, new
                       {
                           name = "IsOwner",
                           type= "boolean",
                           optional = true
                       },
                         new
                       {
                           name = "Email",
                           type= "string",
                           optional = false
                       },
                         new
                       {
                           name = "FirstName",
                           type= "string",
                           optional = true
                       },
                         new
                       {
                           name = "LastName",
                           type= "string",
                           optional = true
                       },
                         new
                       {
                           name = "Designation",
                           type= "string",
                           optional = true
                       },
                         new
                       {
                           name = "PhoneNumber",
                           type= "string",
                           optional = true
                       },
                         new
                       {
                           name = "Roles",
                           type= "List<string> - Array of string",
                           optional = false
                       }
                    },
                    SampleUri = new List<string>
                    {
                        "/api/Account/createUser"
                    },
                    Description = "Http post request, params are posted in the body of the request.",
                    SampleOutput = new List<object>
                    {
                        new {
                                data = new {
                            user = new {
                                user = "..."
                            },
                            roles = new List<string>
                            {
                                "abc-34343-fd"
                            }
                        },
                        success= true,
                       }
                    }
                },

            };


            return Ok(resources);
        }


    }
}
