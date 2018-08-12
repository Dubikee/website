using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Service.Whut;
using Server.Shared.Core;

namespace Server.Host.Controllers
{
    public class WhutController : ControllerBase
    {
        private readonly IWhutService _whut;

        public WhutController(IWhutService whut)
        {
            _whut = whut;
        }
    }
}