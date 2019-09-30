using BeSafe.Core.Interfaces;
using BeSafe.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

public class CalculationService : ICalculationService
{
    public decimal TipAmount(decimal subTotal, double generosity)
    {
        return subTotal * (decimal)(generosity / 100);
    }
}

