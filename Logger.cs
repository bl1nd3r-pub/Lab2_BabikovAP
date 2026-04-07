using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_FIO
{
    public static class Logger
    {
        private static readonly string LogFilePath = $"triangle_lab_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";

        public static void LogSuccess(string requestParams, string triangleType, string coordinates)
        {
            string entry = $"[SUCCESS] {DateTime.Now:yyyy-MM-dd HH:mm:ss}\nПараметры: {requestParams}\nРезультат: Тип={triangleType}, Координаты={coordinates}";
            WriteLog(entry);
        }

        public static void LogFailure(string requestParams, string resultOrError, string stackTrace = null)
        {
            string entry = $"[FAILURE] {DateTime.Now:yyyy-MM-dd HH:mm:ss}\nПараметры: {requestParams}\nРезультат/Ошибка: {resultOrError}";
            if (!string.IsNullOrEmpty(stackTrace))
                entry += $"\nStackTrace: {stackTrace}";
            WriteLog(entry);
        }

        private static void WriteLog(string message)
        {
            Console.WriteLine(message);
            File.AppendAllText(LogFilePath, message + Environment.NewLine);
        }
    }
}
