using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Automation.Utils;

using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow.Assist;
using SeleniumExtras.WaitHelpers;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using TechTalk.SpecFlow;

namespace Automation
{
    public class Check
    {
        private readonly IWebDriver _driver;
       

        public Check(IWebDriver driver)
        {
            _driver = driver;
        }

        //Wait for Angular Load
        public void waitForAngularLoad()
        {
            IJavaScriptExecutor jsExec = (IJavaScriptExecutor)_driver;

            String angularReadyScript = @"var callback = arguments[arguments.length - 1];
            var el = document.querySelector('html');
            if (!window.angular) {
                callback('False')
            }
            if (angular.getTestability) {
                angular.getTestability(el).whenStable(function(){callback('True')});
            } else {
                if (!angular.element(el).injector()) {
                    callback('False')
                }
                var browser = angular.element(el).injector().get('$browser');
                browser.notifyWhenNoOutstandingRequests(function(){callback('True')});
            }";

            //Wait for ANGULAR to load
            Boolean angularLoad = Convert.ToBoolean(jsExec.ExecuteAsyncScript(angularReadyScript));
        }

        public static void sleep(int miliseconds)
        {

            try
            {
                Thread.Sleep(miliseconds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public bool WaitForObject(string obj, int timeoutInSeconds)
        {
            //    IWebDriver driver = Driver.Instance;
            for (int i = 0; i < timeoutInSeconds; i++)
            {
                if (IsElementPresentByXPath(obj) is true)
                    if (_driver.FindElement(By.XPath(obj)).Displayed
                        && _driver.FindElement(By.XPath(obj)).Enabled)
                    {
                        return true;
                    }

                Thread.Sleep(1000);
            }

            return false;
        }

        public bool FluentWait(string obj, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                var test = wait.Until(ExpectedConditions.ElementExists(By.XPath(obj)));

                // if (wait.Until(drv => drv.FindElement(By.XPath(obj))) != null)
                if (test != null)
                {
                    return true;
                }
            }

            return true; ;
        }

        public bool WaitForClass(string obj, int timeoutInSeconds)
        {
          
            for (int i = 0; i < timeoutInSeconds; i++)
            {
                if (IsElementPresentByClass(obj) is true)
                {
                    return true;
                }

                Thread.Sleep(1000);
            }

            return false;
        }


        public bool WaitForCss(string obj, int timeoutInSeconds)
        {
            //    IWebDriver driver = Driver.Instance;
            for (int i = 0; i < timeoutInSeconds; i++)
            {
                if (IsElementPresentByCss(obj) is true)
                {
                    return true;
                }

                Thread.Sleep(1000);
            }

            return false;
        }

        //Will need to find a better wait mechanism
        public void WaitForElementsAndLogo(string elementToWaitFor)
        {

            WaitForElements(elementToWaitFor);
        }

        public void WaitForElements(string elementToWaitFor)
        {
            FluentWait(elementToWaitFor, 20);
            WaitForObject(elementToWaitFor, 20);
        }

        public bool WaitForObjectToDisappear(string obj, int timeoutInSeconds)
        {
            IWebDriver driver = Driver.Instance;
            for (int i = 0; i < timeoutInSeconds; i++)
            {
                if (driver.FindElement(By.XPath(obj)).Displayed is false)
                {
                    return true;
                }

                Thread.Sleep(1000);
            }

            return false;
        }

        public void IfXPathElementIsThere(string obj, string args)
        {
            Assert.IsTrue(IsElementPresentByXPath(obj), args);
            IWebElement element = _driver.FindElement(By.XPath(obj));
            Assert.IsTrue(element.Displayed && element.Enabled, args);
        }

        public void IfXPathElementIsNotThere(string obj, string args)
        {
            if (IsElementPresentByXPath(obj) is true)
            {
                IWebElement element = _driver.FindElement(By.XPath(obj));
                Assert.IsFalse(element.Displayed && element.Enabled, args);
            }
            else
            {
                Assert.IsFalse(IsElementPresentByXPath(obj), args);
            }
        }

        public bool IsElementPresentByCss(string El)
        {
            try
            {
                _driver.FindElement(By.CssSelector(El));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void IfCssElementIsThere(string obj, string args)
        {
            Assert.IsTrue(IsElementPresentByCss(obj), args);
            IWebElement element = _driver.FindElement(By.CssSelector(obj));
            Assert.IsTrue(element.Displayed && element.Enabled, args);
        }

        public void DbCheck(string DbValue, string value)
        {
            Assert.IsTrue(DbValue == value);
        }

        public void IfCssElementIsNotThere(string obj, string args)
        {
            if (IsElementPresentByCss(obj) is true)
            {
                IWebElement element = _driver.FindElement(By.CssSelector(obj));
                Assert.IsFalse(element.Displayed && element.Enabled, args);
            }
            else
            {
                Assert.IsFalse(IsElementPresentByCss(obj), args);
            }
        }


        public void IfXPathContainsText(string obj, string text)
        {
            var Text = string.Empty;
            Text = _driver.FindElement(By.XPath(obj)).Text;
            Assert.IsTrue(Text.Contains(text), "Text: " + text + " not found or incorrect");
        }


        public void IfXPathDoesNotContainText(string obj, string text)
        {
            var Text = _driver.FindElement(By.XPath(obj)).Text;
            Assert.IsFalse(Text.Contains(text));
        }

        public void IfCssContainsText(string obj, string text)
        {
            var Text = _driver.FindElement(By.CssSelector(obj)).Text;
            Assert.IsTrue(Text.Contains(text), "Text: " + text + " not found or incorrect");
        }

        public void IfCssAttributeContainsText(string obj, string attribute, string text)
        {
            String Text = _driver.FindElement(By.CssSelector(obj)).GetAttribute(attribute);
            Assert.IsTrue(Text.Contains(text), "Text: " + text + " not found or incorrect");
        }

        public void IfClassContainsText(string obj, string text)
        {
            var Text = _driver.FindElement(By.CssSelector(obj)).Text;
            Assert.IsTrue(Text.Contains(text), "Text: " + text + " not found or incorrect");
        }



        public void IfCssDoesNotContainText(string obj, string text)
        {
            var Text = _driver.FindElement(By.CssSelector(obj)).Text;
            Assert.IsFalse(Text.Contains(text));
        }

        public void CssIsSelected(string obj)
        {
            IWebElement checkbox = _driver.FindElement(By.CssSelector(obj));
            Assert.IsTrue(checkbox.Selected);
        }

        public void CssIsNotSelected(string obj)
        {
            IWebElement checkbox = _driver.FindElement(By.CssSelector(obj));
            Assert.IsFalse(checkbox.Selected);
        }

        public void XPathIsSelected(string obj)
        {
            IWebElement checkbox = _driver.FindElement(By.XPath(obj));
            Assert.IsTrue(checkbox.Selected);
        }

        public void XPathIsNotSelected(string obj)
        {
            IWebElement checkbox = _driver.FindElement(By.XPath(obj));
            Assert.IsFalse(checkbox.Selected);
        }

        public bool IsElementPresentByXPath(string El)
        {
            try
            {
                _driver.FindElement(By.XPath(El));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsElementPresentByClass(string El)
        {
            try
            {
                var e = _driver.FindElement(By.ClassName(El));
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public void IfPageTitleContainText(string title)
        {

            Assert.IsTrue(_driver.Title.Contains(title));
        }

        public bool IsElementVisableByXPath(string El)
        {
            try
            {
                var i = _driver.FindElement(By.XPath(El));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void IsElementAttributeEqual(string checkValue, string attrib, string path)
        {
            Assert.IsTrue(_driver.FindElement(By.XPath(path)).GetAttribute(attrib).Equals(checkValue));
        }

        public void IfXPathContainsValue(string obj, string value)
        {
            var val = Driver.Instance.FindElement(By.XPath(obj)).GetAttribute("value");
            if (val != null)
            {
                string newVal = val.ToString();
                Console.WriteLine("Expected value is: " + value + " and the value found was: " + newVal);
                Assert.IsTrue(newVal == value, "Fail: Value incorrect");
                Console.WriteLine("Success");
            }
            else
            {
                Assert.Fail("Test Fail: Value not found for Object: " + obj);
            }
        }

        public string GetInventoryLocationIdFromTheUrl()
        {
            string url = _driver.Url;
            string[] test = url.Split(new[] { "location/", "/detail" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> s = StringHelper.ExtractFromString(url, "location/", "/detail");
            return s[0].ToString();
            //https://localhost:44300/main/#/task/inventory/settings/location/31033/detail
        }



        private void GetGridDate(string GridName, string ExcelFilename)
        {
            var elemTable = _driver.FindElement(By.XPath(GridName));

            // Get all Row of the table
            List<IWebElement> lstTrElem = new List<IWebElement>(elemTable.FindElements(By.TagName("tr")));
            DataTable dt = new DataTable();
            String strRowData = "";
            String strHeadData = "";
            int r = 0;

            // Go through each Row
            foreach (var elemTr in lstTrElem)
            {
                dt.Rows.Add();
                // Get columns or Headers from Rows
                List<IWebElement> lstTdElem = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
                List<IWebElement> lstThElem = new List<IWebElement>(elemTr.FindElements(By.TagName("th")));
                if (lstTdElem.Count > 0)
                {
                    int c = 0;
                    // Get each Column value
                    foreach (var elemTd in lstTdElem)
                    {
                        strRowData = strRowData + elemTd.Text + "\t";
                        dt.Rows[r][c] = elemTd.Text;
                        c = c + 1;
                    }
                }
                else
                {
                    // Get each Header
                    foreach (var elemHead in lstThElem)
                    {
                        strHeadData = strHeadData + elemHead.Text + "\t";
                        dt.Columns.Add(elemHead.Text.Replace("\r\n", "")
                            .Replace(" ", "")
                            .Replace("%", "")
                            .Replace("=", ""));
                    }
                }

                //Output Data to Console
                Console.Write(strHeadData.Replace("\r\n", ""));
                strHeadData = String.Empty;
                Console.WriteLine(strRowData.Replace("\r\n", ""));
                strRowData = String.Empty;
                r = r + 1;
            }

            //Output DataTable to Console
            foreach (DataColumn col in dt.Columns)
            {
                Console.Write(col.ToString() + "\t");
            }

            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine();
                for (int x = 0; x < dt.Columns.Count; x++)
                {
                    Console.Write(row[x].ToString() + "\t");
                }
            }

            //  DataRow[] rows = dt.Select(ColumnName + "= '" + Check +"'");

            WriteGridToExcel(dt, ExcelFilename);
        }

        public void CheckTableValue(string gridName, string columnName, String check)
        {
            bool found = false;
            //  WaitForObject(MainBody.General.Loading, 5);
            var table = _driver.FindElement(By.XPath(gridName));
            foreach (var tr in table.FindElements(By.TagName("tr")))
            {
                var tds = tr.FindElements(By.TagName("td"));
                for (var i = 0; i < tds.Count; i++)
                {

                    if (tds[i].Text.Trim().Contains(check))
                    {
                        Assert.IsTrue(true);
                        found = true;
                        break;
                    }

                }
            }
            if (found == false)
            {
                Assert.True(false);
            }
        }
        public static void WriteGridToExcel(DataTable dt, string SheetName)
        {
            var ExcelPath = Path.Combine(TestContext.CurrentContext.TestDirectory, ConfigurationManager.AppSettings["ExcelSheetsPath"] + "\\" + SheetName);
            StreamWriter wr = new StreamWriter(ExcelPath);

            try
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }

                wr.WriteLine();

                //write rows to excel file
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
                        }
                        else
                        {
                            wr.Write("\t");
                        }
                    }
                    //go to next line
                    wr.WriteLine();
                }
                //close file
                wr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void XPathColumnText(string xPath, int num, string text)
        {
            //XPath is the column
            //Num is how many you want to find
            //Get the XPath and replace TEXT with input from Specflow for search
            // Note: the Xpath must be in the following format to search in a particular column: -
            //*[@id=\"ctl00_MainContent_datafileViewer_gvMirrorErrorListView\"]/tbody/tr[*]/td[contains(text(),\"TEXT\")]
            string column = xPath;
            column = column.Replace("TEXT", (text));
            Console.WriteLine(column);
            var count = _driver.FindElements(By.XPath(column)).Count;
            Console.WriteLine("row count of Status " + (text) + " is " + (count));
            Assert.AreEqual(num, count, "There are incorrect values and/or number of values found in the list");
        }

        /// <summary>
        /// Based on the table that is passed in it will search for the identifying value
        /// passed in against the column name provided, this identifies the row to search against.
        /// Then it will look for the column name that holds the cell for the value to check against and use the
        /// row combined with these values to find the actual cell value and check this with the check value that is passed in
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="colNameForId"></param>
        /// <param name="identifingValue"></param>
        /// <param name="columnNameHoldingValueToVerify"></param>
        /// <param name="valueTocheck"></param>
        public void XpathCheckTableCellWithRowElement(string gridName, string colNameForId, string identifingValue, string columnNameHoldingValueToVerify, string valueTocheck)
        {
            //  WaitForObject(MainBody.General.Loading, 5);
            IWebElement table = _driver.FindElement(By.XPath(gridName));

            var row = (from e in TableHelper.ReadTable(table)
                       where e.ColumnName == colNameForId && e.ColumnValue == identifingValue
                       select e.RowNumber).SingleOrDefault();

            string cellValue = TableHelper.ReadCell(columnNameHoldingValueToVerify, row);

            Console.WriteLine(identifingValue + " " + cellValue);
            Assert.IsTrue(valueTocheck == cellValue, "Fail: Value incorrect");
            Console.WriteLine("Success");

        }

    //Commented out as the models have moved out of the project have to find a way to pass in the models to create and 
    //make this method generic

        //public void CheckTableWithPaginationContainsValue<TypeOfValue>(Table table, string pagination, string tableRowCellForTheId, 
        //    string tableRowCellForTheValueToCheck, string nextPage, string pageOne, TypeOfValue Model)
        //{
        //    int pageWeAreOn = 1;
        //    Table singleRowTable = null;

        //    var paginationCount = Driver.Instance.FindElements(By.XPath(pagination)).Count;
        //    Console.WriteLine("Total pages = " + paginationCount.ToString());

        //    //as we have >> at the start we have to start the pagination with + 1 , so page 1 is now (2) page 2 is (3) etc 

        //    //You can use the below in google developer console to validate your xpath
        //    //"$x("//ul[@class='pagination ng-table-pagination']//following-sibling::a[not(contains(.,'»') or contains(.,'«'))]")"
        //    foreach (var row in table.Rows)
        //    {
        //        waitForAngularLoad();
        //        int rowCount = Driver.Instance.FindElements(
        //            By.XPath(tableRowCellForTheId)).Count;
        //        Console.WriteLine("Starting checks on row = " + rowCount);

        //        singleRowTable = new Table(table.Header.ToArray());
        //        singleRowTable.AddRow(row);

        //        var testData = singleRowTable.CreateInstance<Automation.Models.Inventory>();

        //        for (int rowWeAreOn = 1; rowWeAreOn <= rowCount; rowWeAreOn++)
        //        {
        //            string numPath = tableRowCellForTheId;
        //            numPath = numPath.Replace("*", rowWeAreOn.ToString());

        //            //get the path to the id for the text we want to find for the row
        //            var numPathText = Driver.Instance.FindElement(By.XPath(numPath)).Text;
        //            Console.WriteLine(numPathText);

        //            //get the cell for the value we are checking for
        //            string valuePath = tableRowCellForTheValueToCheck;
        //            valuePath = valuePath.Replace("*", rowWeAreOn.ToString());

        //            var pathText = _driver.FindElement(By.XPath(valuePath)).Text;

        //            Console.WriteLine("The  path text = " + pathText);
        //            Console.WriteLine("rowWeAreOn = " + rowWeAreOn.ToString());
        //            Console.WriteLine(" The row path = " + numPath + " The date path =" + valuePath);
        //            Console.WriteLine(" Checking if id we are passing in to find " + testData.Task.ToString() +
        //                              " matches the row  text = " + numPathText.ToString());

        //            if (testData.Task == numPathText)
        //            {
        //                Console.WriteLine(
        //                    "We the match wahoo! now we check to see if the value you want to check matches what is displayed ");
        //                Console.WriteLine("testData.DateDue = " + testData.DateDue.ToString());
        //                Console.WriteLine("datePathText.ToString() = " + pathText.ToString());

        //                Assert.IsTrue(testData.DateDue.ToString() == pathText.ToString(),
        //                    "The values do not match looking for =" + testData.DateDue.ToString() + " found = " +
        //                    pathText.ToString());

        //                break;
        //            }
        //            else if (rowWeAreOn == (rowCount))
        //            {

        //                Console.WriteLine("Id we want to find not found check we are not on the last page");

        //                Console.WriteLine("Page we are = " + (pageWeAreOn - 1).ToString());
        //                Console.WriteLine("Pagination count = " + (paginationCount).ToString());

        //                //check we have not come to the end of the pages
        //                if ((pageWeAreOn - 1) != paginationCount)
        //                {
        //                    Console.WriteLine("Moving on to the next page");
        //                    //click the next the page
        //                    _driver.FindElement(By.XPath(nextPage)).Click();

        //                    waitForAngularLoad();
        //                    pageWeAreOn = pageWeAreOn + 1;

        //                    //We are moving on to the next page so we have to reset the counter of the row we are on to 1
        //                    rowWeAreOn = 1;

        //                    // As we don't know how the amount of rows are on the page we have to get  these each time
        //                    rowCount = _driver.FindElements(
        //                        By.XPath(tableRowCellForTheId)).Count;
        //                    Console.WriteLine("Page number " + (paginationCount).ToString() + " row count = " + rowCount);
        //                }
        //                else
        //                {
        //                    //we are on the last page so fail just printing out for a sanity check
        //                    Console.WriteLine("we are on the last page failed to find the id nothing Page number = " + (paginationCount).ToString());

        //                    Assert.IsTrue(testData.Task == numPathText, " Did not find ID = " + testData.Task.ToString());

        //                }
        //            }
        //        }
        //        //now we have to reset the counts as we are going back to the start on page 1
        //        _driver.FindElement(By.XPath(pageOne)).Click();
        //        waitForAngularLoad();
        //        pageWeAreOn = 1;
        //    }
        //}
    }
}
