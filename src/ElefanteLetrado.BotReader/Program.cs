using System;
using System.Configuration;
using System.IO;
using System.Threading;
using ElefanteLetrado.BotReader.Events;
using ElefanteLetrado.BotReader.Pages;
using ElefanteLetrado.BotReader.Scripts;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ElefanteLetrado.BotReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log("Start BotReader!");

            var continueLoopExecution = true;
            var checkpoint = new CheckpointEvent();
            string url = ConfigurationManager.AppSettings["urlSite"];

            checkpoint.LastBook = ConfigurationManager.AppSettings["lastBook"];
            checkpoint.LastLevel = ConfigurationManager.AppSettings["lastLevel"];

            while (continueLoopExecution)
            {
                try
                {
                    ExecuteScriptWithNewBrowser(url, checkpoint);
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
            }

            Log("Finish BotReader!");
            Log("Press any key to exit...");
            Console.ReadKey();
        }

        private static void ExecuteScriptWithNewBrowser(string url, CheckpointEvent checkpoint)
        {
            var student = new StudentLoginInfo();

            using (var browser = StartBrowserTo(url))
            {
                var script = new SequentialReadScript(
                    new LoginPage(browser),
                    new StudentPage(browser),
                    new ReaderPage(browser)
                )
                {
                    ContinueFromBook = checkpoint.LastBook,
                    ContinueFromLevel = checkpoint.LastLevel,
                    Events = new AggregateEvents()
                        .AddEvent(checkpoint)
                        .AddEvent(new LogEvent(Console.Out))
                };

                script.PlayWith(student);
            }
        }

        private static IWebDriver StartBrowserTo(string url)
        {
            var browser = new ChromeDriver();
            browser.Manage().Window.Maximize();
            browser.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
            return browser;
        }

        private static void Log(string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            var msg = string.Format("{0:dd/MM/yyyy hh:mm:ss.fff} {1}", DateTime.Now, message);
            Console.WriteLine(msg);
        }
    }
}