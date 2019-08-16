using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Automation.PageObjects;

using NUnit.Framework.Internal;
using OpenQA.Selenium;

namespace Automation
{
    public class PageOps
    {

        private readonly PageAction _action;
        private readonly Login _login;
        private readonly IWebDriver _driver;
        private readonly Check _check;

        public PageOps(Login login, PageAction action, IWebDriver driver, Check check)
        {
            _check = check;
            _login = login;
            _action = action;
            _driver = driver;
        }
       

        

    }
}
