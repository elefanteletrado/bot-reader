using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace ElefanteLetrado.BotReader.Pages
{
    public class ReaderPage
    {
        private readonly IWebDriver _browser;

        public ReaderPage(IWebDriver browser)
        {
            _browser = browser;
        }

        public void LeaveRead()
        {
            _browser.FindElements(By.CssSelector(".el-reader-message-box .el-button[title='Biblioteca']"))
                .First()
                .Click();
        }

        public void WaitLoadingBook()
        {
            while (_browser.Url.Contains("/reader/load/read/book/"))
            {
                Thread.Sleep(500);
            }

            if (!_browser.Url.Contains("/reader/read/"))
            {
                Debug.WriteLine("Error! Deve abrir a tela do leitor.");
            }
        }

        public bool HasFinishedBook()
        {
            try
            {
                var button = _browser.FindElement(By.CssSelector(".el-reader-action-button.button-pink"));
                return button != null && button.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool HasReachLastPage()
        {
            var headerTag = _browser.FindElement(By.CssSelector(".reader-header"));

            if (headerTag.GetAttribute("class").Contains("closed"))
            {
                var executor = (IJavaScriptExecutor)_browser;
                executor.ExecuteScript("document.querySelectorAll('.reader-header')[0].classList.remove('closed')");
            }

            var pageMark = _browser.FindElement(By.CssSelector(".reader-header > h1 > div.pull-right > span")).Text;

            if (string.IsNullOrEmpty(pageMark))
            {
                //// info: significa que está na primeira página
                return false;
            }

            var pageParts = pageMark.Split('/');
            var firstPage = Convert.ToInt32(pageParts[0]);
            var lastPage = Convert.ToInt32(pageParts[1]);

            return firstPage == lastPage;
        }

        public void GoToNextPage()
        {
            try
            {
                //// TODO: por vezes, não está conseguindo encontrar o botão de avançar a página
                _browser.FindElement(By.CssSelector(".el-reader-viewer-nav-btn.nav-btn-next"))
                    .Click();
            }
            catch
            {

            }
        }

        public void GoToPreviousPage()
        {
            _browser.FindElement(By.CssSelector(".el-reader-viewer-nav-btn.nav-btn-prev"))
                .Click();
        }

        public void ClickToFinishBook()
        {
            _browser.FindElement(By.CssSelector(".el-reader-action-button.button-pink"))
                .Click();
        }

        public bool HasOpenModalFinishBook()
        {
            try
            {
                var backgroundOverlay = _browser.FindElement(By.CssSelector(".el-reader-message-overlay"));
                return backgroundOverlay != null && backgroundOverlay.Enabled;
            }
            catch
            {
                return false;
            }
        }

        public void GoToLibrary()
        {
            var buttons = _browser.FindElements(By.CssSelector(".el-reader-message-overlay .button.button-pink"));
            buttons.First(b => b.GetAttribute("ng-click") == "goBackToLibrary()")
                .Click();
        }

        public bool IsOpenModalContinue()
        {
            try
            {
                var continueButton = _browser.FindElement(By.CssSelector(".el-reader-message-overlay .button-green"));
                var restartButton = _browser.FindElement(By.CssSelector(".el-reader-message-overlay .button-red"));

                return continueButton != null && restartButton != null && continueButton.Displayed && restartButton.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void ClickContinueRead()
        {
            var continueButton = _browser.FindElement(By.CssSelector(".el-reader-message-overlay .button-green"));
            continueButton.Click();
            Thread.Sleep(500); //// info: deixar finalizar a animação de fadeout da modal
        }

        public void WaitFinishBook()
        {
            while (!HasFinishedBook())
            {
                Thread.Sleep(500);
            }
        }
    }
}