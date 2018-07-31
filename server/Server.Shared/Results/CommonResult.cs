using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Shared.Results
{
    public enum CommonResult
    {
        Ok,
        ParamsIsEmpty,
        TokenExpired,
        NotAllowed
    }
}
