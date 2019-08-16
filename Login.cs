using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Automation.PageObjects;
using OpenQA.Selenium;

namespace Automation
{
    public class Login
    {
        private IWebDriver _driver;
        private readonly string _loginpage;
        public Login(IWebDriver driver, string LoginPage)
        {
            _driver = driver;
            _loginpage = LoginPage;
        }
            public LoginCommand LoginAs(string username)
            {
                return new LoginCommand(username, _driver, _loginpage);
                //    return LoginCommand;
            }

        public void GoToHome()
        {
            //var homePage = Driver.Instance; 
            //homePage.Navigate().GoToUrl("https://localhost::44352/");
            //Driver.Instance.Manage().Window.Maximize();

            //  var homePage = Driver.Instance;
            _driver.Navigate().GoToUrl("https://localhost::44352/");
            _driver.Manage().Window.Maximize();
        }


        public void LoginAsDeutscheBank1()
        {
            GoToHome();
            LoginAs("DeutscheBank1@cube.global").WithPassword("C7FSy1p$gG").LoginToCube();

        }

        public void LoginAsGbGAdmin()
        {
            GoToHome();
            LoginAs("gbgadmin@cube.global").WithPassword("Xez4r^JC3_").LoginToCube();

        }
        public class LoginCommand
        {
            private readonly IWebDriver _driver;
            private readonly string _userName;
            private string _password;
            private readonly string _loginpage;


            public LoginCommand(IWebDriver driver,string loginPage)
            {

                _driver = driver;
                _loginpage = loginPage;
            }


            public LoginCommand(string userName, IWebDriver driver, string loginPage)
            {
                _driver = driver;
                _userName = userName;
                _loginpage =  loginPage;
            }

            public LoginCommand WithPassword(string password)
            {
                _password = password;
                return this;
            }

            public void LoginToCube()
            {


                Thread.Sleep(1000);
                _loginpage.EnterUserNameAndPassword(_userName, _password);
                Thread.Sleep(1000);
                _loginpage.ClickLogin();

            }


        }
    }
}