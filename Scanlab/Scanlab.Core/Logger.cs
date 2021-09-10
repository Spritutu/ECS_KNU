using NLog;
using System;
using System.Net;
using System.Text;

namespace Scanlab
{
    public sealed class Logger
    {
        internal static ILogger logger = (ILogger)LogManager.GetCurrentClassLogger();

        /// <summary>로그 메시지 이벤트 핸들러</summary>
        public static event LoggerMessage OnLogged;

        /// <summary>로그 메시지 기록</summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void Log(Logger.Type type, object message)
        {
            switch (type)
            {
                case Logger.Type.Trace:
                    Logger.logger.Trace(message);
                    break;
                case Logger.Type.Debug:
                    Logger.logger.Debug(message);
                    break;
                case Logger.Type.Info:
                    Logger.logger.Info(message);
                    break;
                case Logger.Type.Warn:
                    Logger.logger.Warn(message);
                    break;
                case Logger.Type.Error:
                    Logger.logger.Error(message);
                    break;
                case Logger.Type.Fatal:
                    Logger.logger.Fatal(message);
                    break;
            }
            Delegate[] invocationList = Logger.OnLogged?.GetInvocationList();
            if (invocationList == null)
                return;
            foreach (LoggerMessage loggerMessage in invocationList)
                loggerMessage(type, (string)message);
        }

        /// <summary>로그 메시지 기록</summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        public static void Log(Logger.Type type, string format, object arg0)
        {
            switch (type)
            {
                case Logger.Type.Trace:
                    Logger.logger.Trace(format, arg0);
                    break;
                case Logger.Type.Debug:
                    Logger.logger.Debug(format, arg0);
                    break;
                case Logger.Type.Info:
                    Logger.logger.Info(format, arg0);
                    break;
                case Logger.Type.Warn:
                    Logger.logger.Warn(format, arg0);
                    break;
                case Logger.Type.Error:
                    Logger.logger.Error(format, arg0);
                    break;
                case Logger.Type.Fatal:
                    Logger.logger.Fatal(format, arg0);
                    break;
            }
            Delegate[] invocationList = Logger.OnLogged?.GetInvocationList();
            if (invocationList == null)
                return;
            foreach (LoggerMessage loggerMessage in invocationList)
                loggerMessage(type, string.Format(format, arg0));
        }

        /// <summary>로그 메시지 기록</summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public static void Log(Logger.Type type, string format, object arg0, object arg1)
        {
            switch (type)
            {
                case Logger.Type.Trace:
                    Logger.logger.Trace(format, arg0, arg1);
                    break;
                case Logger.Type.Debug:
                    Logger.logger.Debug(format, arg0, arg1);
                    break;
                case Logger.Type.Info:
                    Logger.logger.Info(format, arg0, arg1);
                    break;
                case Logger.Type.Warn:
                    Logger.logger.Warn(format, arg0, arg1);
                    break;
                case Logger.Type.Error:
                    Logger.logger.Error(format, arg0, arg1);
                    break;
                case Logger.Type.Fatal:
                    Logger.logger.Fatal(format, arg0, arg1);
                    break;
            }
            Delegate[] invocationList = Logger.OnLogged?.GetInvocationList();
            if (invocationList == null)
                return;
            foreach (LoggerMessage loggerMessage in invocationList)
                loggerMessage(type, string.Format(format, arg0, arg1));
        }

        /// <summary>로그 메시지 기록</summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Log(
          Logger.Type type,
          string format,
          object arg0,
          object arg1,
          object arg2)
        {
            switch (type)
            {
                case Logger.Type.Trace:
                    Logger.logger.Trace(format, arg0, arg1, arg2);
                    break;
                case Logger.Type.Debug:
                    Logger.logger.Debug(format, arg0, arg1, arg2);
                    break;
                case Logger.Type.Info:
                    Logger.logger.Info(format, arg0, arg1, arg2);
                    break;
                case Logger.Type.Warn:
                    Logger.logger.Warn(format, arg0, arg1, arg2);
                    break;
                case Logger.Type.Error:
                    Logger.logger.Error(format, arg0, arg1, arg2);
                    break;
                case Logger.Type.Fatal:
                    Logger.logger.Fatal(format, arg0, arg1, arg2);
                    break;
            }
            Delegate[] invocationList = Logger.OnLogged?.GetInvocationList();
            if (invocationList == null)
                return;
            foreach (LoggerMessage loggerMessage in invocationList)
                loggerMessage(type, string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>로그 메시지 기록</summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Log(Logger.Type type, string format, params object[] args)
        {
            switch (type)
            {
                case Logger.Type.Trace:
                    Logger.logger.Trace(format, args);
                    break;
                case Logger.Type.Debug:
                    Logger.logger.Debug(format, args);
                    break;
                case Logger.Type.Info:
                    Logger.logger.Info(format, args);
                    break;
                case Logger.Type.Warn:
                    Logger.logger.Warn(format, args);
                    break;
                case Logger.Type.Error:
                    Logger.logger.Error(format, args);
                    break;
                case Logger.Type.Fatal:
                    Logger.logger.Fatal(format, args);
                    break;
            }
            Delegate[] invocationList = Logger.OnLogged?.GetInvocationList();
            if (invocationList == null)
                return;
            foreach (LoggerMessage loggerMessage in invocationList)
                loggerMessage(type, string.Format(format, args));
        }

        /// <summary>로그 메시지 기록</summary>
        /// <param name="type"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public static void Log(Logger.Type type, Exception ex, string message)
        {
            switch (type)
            {
                case Logger.Type.Trace:
                    Logger.logger.Trace(ex, message);
                    break;
                case Logger.Type.Debug:
                    Logger.logger.Debug(ex, message);
                    break;
                case Logger.Type.Info:
                    Logger.logger.Info(ex, message);
                    break;
                case Logger.Type.Warn:
                    Logger.logger.Warn(ex, message);
                    break;
                case Logger.Type.Error:
                    Logger.logger.Error(ex, message);
                    break;
                case Logger.Type.Fatal:
                    Logger.logger.Fatal(ex, message);
                    break;
            }
            Delegate[] invocationList = Logger.OnLogged?.GetInvocationList();
            if (invocationList == null)
                return;
            foreach (LoggerMessage loggerMessage in invocationList)
                loggerMessage(type, message + ": " + Logger.ExceptionToString(ex));
        }

        /// <summary>예외 상황 호출스택등 부가 정보 얻기</summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        internal static string ExceptionToString(Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Date: " + DateTime.Now.ToString("u") + Environment.NewLine);
            stringBuilder.AppendFormat("Computer: " + Dns.GetHostName() + Environment.NewLine);
            stringBuilder.AppendFormat("Source: " + ex.Source.Trim() + Environment.NewLine);
            stringBuilder.AppendFormat("Method: " + ex.TargetSite.Name + Environment.NewLine);
            stringBuilder.AppendFormat("Message: " + ex.Message + Environment.NewLine);
            stringBuilder.AppendFormat("Stack Trace: " + ex.StackTrace + Environment.NewLine);
            return stringBuilder.ToString();
        }

        /// <summary>로그 타입</summary>
        public enum Type
        {
            /// <summary>추적</summary>
            Trace,
            /// <summary>디버그</summary>
            Debug,
            /// <summary>정보 (기본)</summary>
            Info,
            /// <summary>경고</summary>
            Warn,
            /// <summary>에러</summary>
            Error,
            /// <summary>치명</summary>
            Fatal,
        }
    }
}
