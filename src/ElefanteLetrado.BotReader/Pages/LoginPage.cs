using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace ElefanteLetrado.BotReader.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _browser;

        public LoginPage(IWebDriver browser)
        {
            _browser = browser;
        }

        public void submitUserLogin(string login)
        {
            var txtUsername = _browser.FindElement(By.Name("username"));
            txtUsername.Clear();
            txtUsername.SendKeys(login);
            _browser.FindElement(By.CssSelector("input[type=submit]")).Click();
            WaitLoadCourses();
        }

        public void submitStudent(string courseName, string studentName, string studentPassword)
        {
            var cmbCourseFormat = "//select/option[contains(text(),'{0}')]";
            _browser.FindElement(By.XPath(String.Format(cmbCourseFormat, courseName)))
                .Click();
            Thread.Sleep(500);

            var studentAvatar = _browser.FindElements(By.CssSelector(".students-list-itens li > a > div.student-fullname"))
                .FirstOrDefault(tag => tag.Text == studentName);
            studentAvatar.Click();

            var txtPassword = _browser.FindElement(By.Name("password"));
            txtPassword.Clear();
            txtPassword.SendKeys(studentPassword);

            _browser.FindElement(By.CssSelector("input[type=submit]")).Click();
        }

        public bool HasFilledSelectCourse()
        {
            try
            {
                var selectTag = _browser.FindElement(By.TagName("select"));
                return selectTag != null && selectTag.Enabled && Convert.ToInt16(selectTag.GetAttribute("length")) > 1;
            }
            catch
            {
                return false;
            }
        }

        public void WaitLoadCourses()
        {
            while (!HasFilledSelectCourse())
            {
                Thread.Sleep(200);
            }
        }
    }
}