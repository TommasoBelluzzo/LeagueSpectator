﻿#region Using Directives
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using LeagueSpectator.Properties;
#endregion

namespace LeagueSpectator
{
    public static class Program
    {
        #region Members
        private static Mutex s_Mutex;
        #endregion

        #region Properties
        public static ApplicationDialog Form { get; private set; }

        public static Icon Icon { get; private set; }

        public static String Version { get; private set; }

        public static UInt32 MutexMessage { get; private set; }
        #endregion

        #region Entry Point
        [STAThread]
        public static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Application.ThreadException += ThreadException;

            Assembly assembly = Assembly.GetExecutingAssembly();
            String assemblyGuid = ((GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            String mutexName = String.Format(CultureInfo.InvariantCulture, @"Local\{{{0}}}", assemblyGuid);

            MutexMessage = NativeMethods.RegisterMessage(assemblyGuid);
            s_Mutex = new Mutex(true, mutexName, out Boolean mutexCreated);

            if (!mutexCreated)
            {
                NativeMethods.BroadcastMessage(MutexMessage);
                return;
            }

            Stream stream = assembly.GetManifestResourceStream("LeagueSpectator.Properties.Application.ico");

            if (stream != null)
                Icon = new Icon(stream);

            Version programVersion = assembly.GetName().Version;
            Version = String.Concat("v", programVersion.Major, ".", programVersion.Minor);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Form = new ApplicationDialog());

            s_Mutex.ReleaseMutex();
        }
        #endregion

        #region Events
        private static void ThreadException(Object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }
        #endregion

        #region Methods
        private static void HandleException(Exception e)
        {
            if (e == null)
                e = new ApplicationException(Resources.TextUnknownException);

            using (ExceptionDialog dialog = new ExceptionDialog(e))
                dialog.Prompt();

            Application.Exit();
        }
        #endregion
    }
}