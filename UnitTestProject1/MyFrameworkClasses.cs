using System;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using System.IO;

namespace UnitTestProject1
{
    public class LoginPage:ICleanUp
    {
        public string StartingAddress { get; set; }
        public string pageTitle { get; set; }
        public IWebDriver Driver { get; set; }
        By logInLink = By.Id("login_link_top");
        By userNameTextBox = By.Id("Bugzilla_login_top");
        By passwordTextBox = By.Id("Bugzilla_password_top");
        By logInButton = By.Id("log_in_top");

        // page constructor
        public LoginPage(string whichBrowser, string startingAddress)
        {
            this.StartingAddress = startingAddress;
            switch (whichBrowser)
            {
                case "Firefox":
                    this.Driver = new FirefoxDriver();
                    break;
                case "Chrome":
                    this.Driver = new ChromeDriver();
                    break;
                case "Edge":
                    this.Driver = new EdgeDriver();
                    break;
                case "IE":
                    this.Driver = new InternetExplorerDriver();
                    break;
                default:
                    throw new Exception("unsupported!");
            }
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(StartingAddress);
        }
        // login method
        public void LoginIntoBugzilla(string username, string password)
        {
            Driver.FindElement(logInLink).Click();
            Driver.FindElement(userNameTextBox).SendKeys(username);
            Driver.FindElement(passwordTextBox).SendKeys(password);
            Driver.FindElement(logInButton).Click();
            pageTitle = Driver.Title;
        }
        // cleaning method
        public void CleanUp()
        {
            Driver.Quit();
        }
        
    }
    public class AdministrationPage:ICleanUp
    {
        public IWebDriver Driver { get; set; }
        public string pageTitle { get; set; }
        By administrationLink = By.XPath("//a[contains(.,'Administration')]");
        
        // Constructor gets driver from login page and switches to administration page
        public AdministrationPage(IWebDriver driver)
        {
            this.Driver = driver;
            Driver.FindElement(administrationLink).Click();
            pageTitle = Driver.Title;
        }

        public void CleanUp()
        {
            Driver.Quit();
        }
    }

    public class UsersPage:ICleanUp
    {
        public IWebDriver Driver { get; set; }
        public string pageTitle { get; set; }
        By usersLink = By.XPath("//a[contains(.,'Users')]"); 
        public UsersPage(IWebDriver driver)
        {
            this.Driver = driver;
            Driver.FindElement(usersLink).Click();
            pageTitle = Driver.Title;
        }

        public void CleanUp()
        {
            Driver.Quit();
        }
    }

    public class ZbiorczeWynikiTestów
    {
        public StringBuilder sb = new StringBuilder();

        public ZbiorczeWynikiTestów()
        {
            // Because the unit tests can't interact with each other (they have to be independent), they all write
            // to the same result file - to summarize the tests

            if (File.Exists("result.html"))
            {
                string poczatkowaWartosc = File.ReadAllText("result.html");
            }
        }

        public void dodajWynikiTestu(string wynikTestu)
        {
            sb.Append(wynikTestu);
        }
        public void Zapisz()
        {
            File.AppendAllText("result.html", sb.ToString()+ Environment.NewLine);
        }
    }

    public static class Weryfikator         // static verification method
    {
        public static void WeryfikujTytul(string nazwaTestu, string oczekiwanyTytul, string faktycznyTytul, ZbiorczeWynikiTestów tutajWpiszRezultat)
        {
            string rezultatTestu = "";
            string trescWiadomosci = "";
            if (oczekiwanyTytul == faktycznyTytul)
            {
                rezultatTestu = "<font color=\"green\">Pass</font>";
                trescWiadomosci = "Test zakończony sukcesem";
            }
            else
            {
                rezultatTestu = "<font color=\"red\">Fail</font>"; ;
                trescWiadomosci = "Test nie powiódł się, bo tytuł: " + faktycznyTytul + " zamiast: " + oczekiwanyTytul;
            }

            // jeden test = jedna linia w tabelce wyjsciowym htmlu
            tutajWpiszRezultat.dodajWynikiTestu("<tr><td>" + nazwaTestu + "</td>" + "<td>" + rezultatTestu + "</td><td>" + trescWiadomosci + "</td></tr>");
        }
    }
}