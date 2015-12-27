using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public static class Constants
    {
        // Definition Paths
        public static string MenuItemsPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\definitions\LeftMenu.txt"); } }

        public static string MultiLingualJSPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\client\lang\"); } }

        public static string GuestLoginInfoPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\definitions\guest.txt"); } }

        public static string LanguageListPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\definitions\language.txt"); } }

        public static string TransactionTypePath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\definitions\transaction_type.txt"); } }

        public static string ScenarioOperationPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\definitions\scenario_operations.txt"); } }

        public static string ScenarioFunctionPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\definitions\scenario_functions.txt"); } }

        public static string DefaultCulture { get { return "tr-TR"; } }

        public static int MaxUiRecordCount { get { return 100; } }

        public static int MaxFileUploadBytes { get { return MaxFileUploadMBytes * 1024 * 2014; } }

        public static int MaxFileUploadMBytes { get { return 100; } }

        public static int MaxUiNetrowkCount { get { return 1000; } }
    }
}