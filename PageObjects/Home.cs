
using OpenQA.Selenium;

namespace Automation.PageObjects
{
    public class Home
    {
        private IWebDriver _driver;

        public Home(IWebDriver driver)
        {
            _driver = driver;
        }

        public class PageLabels
        {

        }
        public class PageLinks
        {

        }

        public class PageTextBox
        {
        }

        public class PageTitle
        {
            public static string SubmitMaritimeStatisticsTitle = "/html/head/title";
        }
        public class PageButton
        {
            public static string StartNow = "//*[@id='start-btn']";
            public static string Login = "";

        }
        public class PageCheckBox
        {

        }

        public class PageImage
        {

        }
    }
}
