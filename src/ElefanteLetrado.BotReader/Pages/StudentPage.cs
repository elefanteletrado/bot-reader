using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace ElefanteLetrado.BotReader.Pages
{
    public class StudentPage
    {
        private readonly IWebDriver _browser;

        public StudentPage(IWebDriver browser)
        {
            _browser = browser;
        }

        public string[] GetAllActiveLevels()
        {
            return _browser.FindElements(By.CssSelector(".el-level > span:not(.disable)"))
                .Select(t => t.Text)
                .ToArray();
        }

        public void ClickLevelByName(string name)
        {
            _browser.FindElements(By.CssSelector(".el-level > span"))
                .First(t => t.Text.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Click();
        }

        public string[] GetAllBookNames()
        {
            return _browser.FindElements(By.CssSelector("data-el-bookshelf-book > div > a"))
                .Select(t => t.GetAttribute("title"))
                .ToArray();
        }

        public void ClickBookByName(string name)
        {
            _browser.FindElement(By.CssSelector("data-el-bookshelf-book > div > a[title='" + name + "']"))
                .Click();
        }

        public bool HasBooks()
        {
            try
            {
                var tags = _browser.FindElements(By.TagName("data-el-bookshelf-book"));
                return tags != null && tags.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool HasLevels()
        {
            try
            {
                var levels = _browser.FindElements(By.CssSelector(".el-level"));
                return levels != null && levels.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public void WaitShowingBookshelf()
        {
            while (!HasBooks())
            {
                Thread.Sleep(200);
            }
        }

        internal void WaitShowingLevels()
        {
            while (!HasLevels())
            {
                Thread.Sleep(200);
            }

            //// info: está ocorrendo erro na máquina do elefante ao clicar no nível porque o elemento de UI do nível está é clicável                       
            Thread.Sleep(500);
        }
    }
}