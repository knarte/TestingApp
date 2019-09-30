using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Interfaces
{
    public interface ICalculationService
    {
        decimal TipAmount(decimal subTotal, double generosity);
    }

}
