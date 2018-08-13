using Microsoft.AspNetCore.Mvc;
using Server.Shared.Core.Services;
using Server.Shared.Models.Whut;

namespace Server.Host.Controllers
{
    public class WhutController : ControllerBase
    {
        private readonly IWhutService<WhutStudent> _whut;

        public WhutController(IWhutService<WhutStudent> whut)
        {
            _whut = whut;
        }
    }
}