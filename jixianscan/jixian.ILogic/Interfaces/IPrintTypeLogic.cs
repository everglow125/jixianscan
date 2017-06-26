using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jixian.Logic.Inte
{
    public interface IPrintTypeLogic : IBaseLogic<PrintType>
    {
        Dictionary<string, string> GetPrintTypeMatchs();
    }
}
