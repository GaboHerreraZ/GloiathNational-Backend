using App.Data.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Business
{
    public interface IRate
    {
        List<Rate> GetRate();
        void SetRate();
        void CalculateConversion(string target);
        void Dispose();
    }
}
