using System.Threading;
using OpenQA.Selenium;

namespace Automation
{

    public class Home
    {
        private IWebDriver _driver;
        private string _url;


        public Home(IWebDriver driver,string url)
        {
            _driver = driver;
            _url = url;
       
        }

        public void GoToHome()
        {

            
            _driver.Navigate().GoToUrl(_url);
            _driver.Manage().Window.Maximize();
        }
    }
}
