using App.Exceptions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Logger
{
    public class LoggerGloiath
    {
        private static readonly ILog _log = LogManager.GetLogger("ErrorBd");

        public static void SetLogError(object message,ExceptionOperation exception)
        {
            string loggerName = exception.Operation;

            _log.Error(message, exception);
            _log.Error(exception.StackTrace);

        }

        public static void SetLogError(object message, Exception exception)
        {

            _log.Error(message, exception);
            _log.Error(exception.StackTrace);

        }
    }
}
