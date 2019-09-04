using System;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace Logeo
{
    public class JobbLogger
    {
        public static bool _logToFile;
        public static bool _logToConsole;
        public static bool _logMessage;
        public static bool _logWarning;
        public static bool _logError;
        public static bool LogToDatabase;
        public static bool _initialized;




        public JobbLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage,
            bool logWarning, bool logError)
        {
            _logError = logError;
            _logMessage = logMessage;
            _logWarning = logWarning;
            LogToDatabase = logToDatabase;
            _logToFile = logToFile;
            _logToConsole = logToConsole;
        }

        public static void LogMessage(string message1, bool message, bool warning, bool error)
        {
            try
            {
                message1.Trim(); //elimina los espacios de inicio y fin que pueda tener el mensaje

                if (message1 == null || message1.Length == 0) //acá verifica que haya algún mensaje, de lo contrario devuelve vacío
                {
                    return;
                }

                if (ControlarInicio(_logToConsole, _logToFile, LogToDatabase))//modifiqué el código con este método para verificar, a través de prueba unitaria, que siempre sea falso, es decir, que si o si se cumpla una de las 3 condiciones o 2
                {
                    throw new Exception("Invalidconfiguration");
                }

                if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning && !error))
                {
                    throw new Exception("Error or Warning or Message must be specified");
                }
                                

                LogueoBase(message, error, warning, _logMessage, _logError, _logWarning); //creé un método para el logueo a la base

                
                //Logeo a archivo de texto

                LogueoArchivo(message, error, warning, _logMessage, _logError, _logWarning); //creé un método para el logueo a archivo
                

                //Logueo de consola

                LogueoConsola(message, error, warning, _logMessage, _logError, _logWarning); //creé un método para el logueo a archivo

                
            }catch(System.Data.SqlClient.SqlException e) { }
        }


        public static bool ControlarInicio(bool _logToConsole,bool _logToFile,bool LogToDatabase)
        {
            bool resultado = !_logToConsole && !_logToFile && !LogToDatabase;

            return resultado;
        }


        public static void LogueoBase(bool message, bool error, bool warning, bool _logMessage, bool _logError, bool _logWarning)
        {
            try
            {
                System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
                connection.Open();

                int t = 0;

                if (message && _logMessage)
                {
                    t = 1;
                }
                else if (error && _logError)
                {
                    t = 2;
                }
                else if (warning && _logWarning) //acá debería haber otro else if porq si entró en uno de los de arriba, no necesita entrar en otro
                {
                    t = 3;
                }
                //Logeo a Base de datos
                SqlCommand command = new SqlCommand("Insert into Log Values('" + message + "', " + t.ToString() + ")");
                command.ExecuteNonQuery();


            }
            catch (System.Data.SqlClient.SqlException e) { }
        }

        public static void LogueoArchivo(bool message, bool error, bool warning, bool _logMessage, bool _logError, bool _logWarning)
        {
            try
            {
                string l = string.Empty;
                if
                (!System.IO.File.Exists(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
                {
                    l = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
                }
                if (error && _logError)
                {
                    l = l + DateTime.Now.ToShortDateString() + message;
                }

                if (warning && _logWarning)
                {
                    l = l + DateTime.Now.ToShortDateString() + message;
                }
                if (message && _logMessage)
                {
                    l = l + DateTime.Now.ToShortDateString() + message;
                }

                System.IO.File.WriteAllText(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", l);
            }
            catch(System.IO.DirectoryNotFoundException e) { }
        }

        public static void LogueoConsola(bool message, bool error, bool warning, bool _logMessage, bool _logError, bool _logWarning)
        {
            if (error && _logError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if (warning && _logWarning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (message && _logMessage)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
        }
    }





     
}