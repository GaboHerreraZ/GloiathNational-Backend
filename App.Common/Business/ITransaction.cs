using App.Data.Model.Model;
using App.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Business
{
    public interface ITransaction
    {
        List<ProductTransaction> GetTransaction();
        void SetTransaction();
        TotalTransaction[] GetSkuValue(string sku);
        TotalTransaction[] GetTotalTransaction();
        void Dispose();
    }
}
