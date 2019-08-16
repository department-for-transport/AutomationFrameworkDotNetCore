using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Automation
{
    //
    public static class Driver
    {
        private static IWebDriver _webDriver;

        public static IWebDriver Instance
        {
            get
            {
                return _webDriver ?? Create();
            }
        }

        private static IWebDriver Create()
        {
            _webDriver = new ChromeDriver();
            return _webDriver;
        }

        public static void Initialise()
        {
            _webDriver = new ChromeDriver();
        }

        public static void Initialise(ChromeOptions opt)
        {
            _webDriver = new ChromeDriver(".",opt);
        }
        public static void Close()
        {
            if (_webDriver == null) return;
            _webDriver.Close();
            _webDriver.Quit();
        }

        public static IJavaScriptExecutor Scripts(this IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;
        }
    }
}