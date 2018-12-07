using App.Common;
using App.Common.Business;
using App.Data.Model.Base;
using App.Data.Model.Model;
using App.Integration;
using App.Utility;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Transactions;
using App.Entity.Dto;
using App.Entity.Common;
using System.Data.SqlClient;

namespace App.Business
{
    /// <summary>
    /// Clase que maneja toda la lógica del negocio de transacciones
    /// </summary>
    /// <remarks> Autor: Gabriel Herrera</remarks>
    public class TransactionBusiness: ITransaction
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        #region Dispose
        /// <summary>
        /// Método público que permite liberar recursos no administrados de la clase TransactionBusiness.
        /// </summary>
        /// <remarks> Autor: Gabriel Herrera</remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        #region Métodos
        /// <summary>
        /// Método que inserta las transacciones en basa de datos
        /// </summary>
        /// <remarks> Autor: Gabriel Herrera</remarks>
        public void SetTransaction()
        {
            int code = 0;
            TransactionOptions transactionOption = new TransactionOptions()
            {
                Timeout = TransactionManager.DefaultTimeout,
                IsolationLevel = IsolationLevel.Serializable
            };
            
            ServiceIntegration Service = new ServiceIntegration();
            IEnumerable<ProductTransaction> objeto = Service.ResponseTransaction<ProductTransaction>(CommonConstants.UrlTransaction,ref code);

            if (code == 200)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOption))
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.TransactionRepository.AllDelete();
                        unitOfWork.TransactionRepository.BulkInsert(objeto.ToList());
                    }

                    scope.Complete();
                }
            }
        }
        /// <summary>
        /// Método que retorna todas las transacciones
        /// </summary>
        /// <remarks> Autor: Gabriel Herrera</remarks>
        public List<ProductTransaction> GetTransaction()
        {
            List<ProductTransaction> objeto = new List<ProductTransaction>();
            using (unitOfWork = new UnitOfWork())
            {
                objeto = unitOfWork.TransactionRepository.Get().ToList();
                return objeto;
            }
        }

        /// <summary>
        /// Método que retorna el consolidado de las transacciones
        /// </summary>
        /// <remarks> Autor: Gabriel Herrera</remarks>
        public TotalTransaction[] GetTotalTransaction()
        {
            string procedureName = "SearchTransaction";

            using (unitOfWork = new UnitOfWork())
            {
                TotalTransaction[] total = unitOfWork.TransactionRepository.ExecuteProcedure<TotalTransaction>(procedureName);
                return total;

            }

        }

        /// <summary>
        /// Método que retorna el valor en euros de un sku
        /// </summary>
        /// <remarks> Autor: Gabriel Herrera</remarks>
        public TotalTransaction[] GetSkuValue(string sku)
        {
            string procedureName = "CalculateSku";
            using(unitOfWork = new UnitOfWork())
            {
                SqlProcedure procedure = new SqlProcedure(procedureName, new SqlParameter { ParameterName ="@Sku",Value = sku});
                TotalTransaction[] skuTotal = unitOfWork.TransactionRepository.ExecuteProcedure<TotalTransaction>(procedure);
                return skuTotal;
            }
        }
        #endregion


    }
}
