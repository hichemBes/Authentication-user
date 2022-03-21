using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apii.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ValueController : ControllerBase
	{


		public ValueController()
		{

		}
		
		[HttpGet,Authorize]
		public ActionResult<IEnumerable<string>> Get()
		{
			return new string[] { "value1", "value2" };
		}


	}
}
