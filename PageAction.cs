using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
namespace Automation
{
    public class PageAction
    {

        private readonly IWebDriver _driver;
        private readonly Actions _action;
        private readonly Check _check;
        public PageAction(IWebDriver driver, Check check)
        {
            _driver = driver;
            _check = check;
            _action = new Actions(_driver);
        }

        public void ClickXpath(string El)
        {
            _driver.FindElement(By.XPath(El)).Click();
        }

        public void MoveToXpath(string obj)
        {
            //WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            //////must first wait for the call to complete
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath(obj)));

            //try
            //{
            //    IWebElement element = _driver.FindElement(By.XPath(obj));
            //    _action.MoveToElement(_driver.FindElement
            //        (By.XPath(obj))).Build().Perform();
            //    Thread.Sleep(250);
            //}
            //catch (StaleElementReferenceException e)
            //{
            //    Console.WriteLine(e);

            //}
            Actions Obj = new Actions(Driver.Instance);
            Obj.MoveToElement(Driver.Instance.FindElement
                (By.XPath(obj))).Build().Perform();
            Thread.Sleep(250);

        }

        public void ClickElementForXpathRowWithText(string textXPath, string text, string elemXPath)
        {
            var row = _driver.FindElements(By.XPath(textXPath));
            var button = _driver.FindElements(By.XPath(elemXPath));
            int rowCount = row.Count();

            for (int i = 1; i <= rowCount; i++)
            {
                //Xpath must be wildcarded using * at Row Number for this method to function
                string newRow = textXPath;
                newRow = newRow.Replace("[*]", "[" + i.ToString() + "]");
                Console.WriteLine(newRow);
                var rowToCheck = _driver.FindElement(By.XPath(newRow)).Text;
                Console.WriteLine(rowToCheck);

                if (rowToCheck.Contains(text))
                {
                    Console.WriteLine("Found");
                    Thread.Sleep(5000);
                    var pathToClick = _driver.FindElements(By.XPath(elemXPath)).ElementAt(i - 1);
                    pathToClick.Click();
                    break;
                }
            }
        }
        public void TypeToXPath(string obj, string keys)
        {
            _driver.FindElement
                (By.XPath(obj)).SendKeys(keys);
        }

        public void SendKeysToXpath(string obj, string keys)
        {
            _driver.FindElement
                (By.XPath(obj)).SendKeys(keys);
        }

        public void SendKeysWithAction(string obj, string keys)
        {
            // Actions Obj = new Actions(Driver.Instance);
            _action.Click();
            _action.SendKeys(keys).Build().Perform();
            Thread.Sleep(250);
        }

        public void SelectValueFromXPathDropdown(string XPath, string Value)
        {
            var dropDown = _driver.FindElement(By.XPath(XPath));
            var selectValue = new SelectElement(dropDown);
            selectValue.SelectByText(Value);
        }

        public void ClickTableLink(string rowValueToFind, string table, string aHefTextToFind)
        {
            var foundTable = _driver.FindElement(By.XPath(table));
            var rows = foundTable.FindElements(By.TagName("tr"));

            foreach (var row in rows)
            {
                if (row.Text.Contains(rowValueToFind))
                {
                    var tds = row.FindElements(By.TagName("a"));

                    foreach (var entry in tds)
                    {
                        if (entry.Text.Trim().Contains(aHefTextToFind))
                        {
                            Console.WriteLine(entry.Text);
                            entry.Click();
                            return;
                        }

                    }
                }
            }
        }

        public void ClearInputBox(string XPath)
        {
            var inputBox = _driver.FindElement(By.XPath(XPath));
            inputBox.Clear();
        }

        //This method will look for the items in a in the Ul for a href 
        //The xpath will require the path to a href 
        public void ClickAnItemInaDropDownInputListBox(string xPathToInputBox, string xPathListItem, string valueToFind)
        {
            ClickXpath(xPathToInputBox);

            var xPath = string.Format(xPathListItem + "[text()[contains(., '{0}')]]", valueToFind);

            ClickXpath(xPath);

        }


        public void ScrollElementIntoView(string obj)
        {
            var elem = _driver.FindElement(By.XPath(obj));
            IJavaScriptExecutor jsExec = (IJavaScriptExecutor)_driver;

            jsExec.ExecuteAsyncScript("arguments[0].scrollIntoView(true);", elem);
        }




        public void ClickButtonForXPathRowWithText(string TextXPath, string Text, string BtnXPath)
        {
            var row = Driver.Instance.FindElements(By.XPath(TextXPath));
            var button = Driver.Instance.FindElements(By.XPath(BtnXPath));
            int rowCount = row.Count();
            if (rowCount is (1))
            {
                //Xpath must have Row Number wildcarded using * at Row Number e.g. //*[@id=\"ListName\"]/tbody/tr[*]/td[3] where td is the Column to be used
                var pathToClick = Driver.Instance.FindElements(By.XPath(BtnXPath)).ElementAt(0);
                pathToClick.Click();
            }

            else
            {
                for (int i = 1; i <= rowCount; i++)
                {
                    //Xpath must be wildcarded using * at Row Number for this method to function
                    string newRow = TextXPath;
                    newRow = newRow.Replace("[*]", "[" + i.ToString() + "]");
                    var rowText = Driver.Instance.FindElement(By.XPath(newRow)).Text;
                    if (rowText.Contains(Text))
                    {
                        //this path must also have the Row Number wildcarded to allow elementAt to function
                        var pathToClick = Driver.Instance.FindElements(By.XPath(BtnXPath)).ElementAt(i - 1);
                        pathToClick.Click();
                        break;
                    }

                    Console.WriteLine(newRow);
                }
            }
        }


        public void ClickSpecificButtonForXPathRowWithText(string TextXPath, string Text, string BtnXPath, string BtnText)
        {
            var row = Driver.Instance.FindElements(By.XPath(TextXPath));
            var button = Driver.Instance.FindElements(By.XPath(BtnXPath));
            int rowCount = row.Count();

            if (rowCount is (1))
            {
                //Xpath must have Row Number wildcarded using * at Row Number e.g. //*[@id=\"ListName\"]/tbody/tr[*]/td[3] where td is the Column to be used
                var pathToClick = Driver.Instance.FindElements(By.XPath(BtnXPath)).ElementAt(0);
                pathToClick.Click();
            }

            else
            {
                for (int i = 1; i <= rowCount; i++)
                {
                    //Xpath must be wildcarded using * at Row Number for this method to function
                    string newRow = TextXPath;
                    string newRow2 = BtnXPath;
                    newRow = newRow.Replace("[*]", "[" + i.ToString() + "]");
                    newRow2 = newRow2.Replace("[ROW]", "[" + i.ToString() + "]").Replace("OPTION", BtnText);
                    var rowText = Driver.Instance.FindElement(By.XPath(newRow)).Text;

                    if (rowText.Contains(Text))
                    {
                        //this path must also have the Row Number wildcarded to allow elementAt to function
                        var pathToClick = Driver.Instance.FindElement(By.XPath(newRow2));
                        pathToClick.Click();
                        break;
                    }

                    Console.WriteLine(newRow);
                }
            }
        }
    }
}
