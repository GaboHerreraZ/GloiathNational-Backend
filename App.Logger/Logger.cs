using App.Exceptions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Logger
{

    /// <summary>
    /// Clase creada para el manejo de errores, en caso de no usar Azure storage
    /// </summary>
    /// <returns></returns>
    /// <CreatedBy>Gabriel Herrera Z</CreatedBy>
    public class Logger
    {
        #region Variables

        private static readonly ILog _log = LogManager.GetLogger("ErrorLog");

        #endregion

        /// <summary>
        /// Método que inserta el log en el appender especificado a partir de una
        /// excepción no controlada
        /// </summary>
        /// <returns></returns>
        /// <CreatedBy>Gabriel Herrera Z</CreatedBy>
        public static void SetLog(Exception ex)
        {
            if (ex != null)
            {
                _log.Error(ex.Message + "|" + ex.InnerException + "|" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Método que inserta el log en el appender especificado a partir de una
        /// excepción controlada
        /// </summary>
        /// <returns></returns>
        /// <CreatedBy>Gabriel Herrera Z</CreatedBy>
        public static void SetLog(ExceptionOperation ex)
        {
            if (ex != null)
            {
                _log.Error(ex.Message + "|" + ex.InnerException + "|" + ex.StackTrace);
            }
        }
        /// <summary>
        /// Método que inserta el log tipo info en el appender especificado
        /// </summary>
        /// <returns></returns>
        /// <CreatedBy>Gabriel Herrera Z</CreatedBy>
        public static void WriteMessage(string text)
        {
            if (text != null)
            {
                _log.Info(text);
            }
        }

    }
}
