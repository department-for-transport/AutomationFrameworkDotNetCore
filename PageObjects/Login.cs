using OpenQA.Selenium;


namespace Automation.PageObjects
{

    public class Login
    {
        private IWebDriver _driver;


        public Login(IWebDriver driver)
        {
            _driver = driver;
        }

        IWebElement TxtUserName => _driver.FindElement(By.XPath(PageTextBox.User));
        IWebElement TxtPassword => _driver.FindElement(By.XPath(PageTextBox.Password));
        IWebElement BtnSignIn => _driver.FindElement(By.XPath(PageButton.SignIn));

        public void EnterUserNameAndPassword(string userName, string password)
        {
            TxtUserName.SendKeys(userName);
            TxtPassword.SendKeys(password);
        }

        public void ClickLogin()
        {
            BtnSignIn.Click();
        }


        public class PageLinks
        {

            public static string ForgotPassword = "//*[@id='lnkforgotpassword']";
        }

        public class PageLabels
        {
            public static string Login = "//*[@id='hdrlogin']";
            public static string Copyright = "//*[@id='lblcopyright']";
            public static string RememberMe = "//*[@id='lblrememberme']";

        }

        public class PageTextBox
        {
            public static string User = "//*[@id='username']";
            public static string Password = "//*[@id='password']";

        }

        public class PageButton
        {
            public static string SignIn = "//*[@id='btnsignin']";
        }
        public class PageCheckBox
        {
            public static string RememberMe = "//*[@id='chkrememberme']";
        }

        public class PageImage
        {
            public static string CubeLogo = "//*[@id='imgcubelogo']";
        }
    }
}
