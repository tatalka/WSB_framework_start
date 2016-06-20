using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace UnitTestProject1
{
    [TestClass]
    public class TestySelenium
    {
        [ClassInitialize()]
        public static void WystartujPrzedTestami(TestContext test)
        {
            // if the result file is present - remove it.
            string ResultFilename = "result.html";

            if (File.Exists(ResultFilename))
            {
                File.Delete(ResultFilename);
            }

        }
        ZbiorczeWynikiTestów ZW  = new ZbiorczeWynikiTestów();

        [TestMethod]
        public void Logowanie_poprawne_Chrome()
        {
            // Creating page object 
            LoginPage stronaLogowania = new LoginPage("Chrome", @"http://localhost/bugzilla");
            
            // calling object's login method
            stronaLogowania.LoginIntoBugzilla("olew@poczta.onet.pl", "poziomka");

            // verification if the login went smoothly - parameters test name / expected page title / actual title / 
            Weryfikator.WeryfikujTytul("Logowanie poprawne Chrome", 
                "Welcome to Bugzilla",
                stronaLogowania.pageTitle,
                ZW);

            // Wynik asercji zostaje zapisany
            ZW.Zapisz();
            // czyszczenie po teście
            stronaLogowania.CleanUp();
        }

        [TestMethod]
        public void Logowanie_poprawne_Firefox()
        {
            LoginPage stronaLogowania = new LoginPage("Firefox", @"http://localhost/bugzilla");
            stronaLogowania.LoginIntoBugzilla("olew@poczta.onet.pl", "poziomka");
            Weryfikator.WeryfikujTytul("Logowanie poprawne Firefox",
                "Welcome to Bugzilla",
                stronaLogowania.pageTitle,
                ZW);

            ZW.Zapisz();
            // czyszczenie po teście
            stronaLogowania.CleanUp();
        }
        [TestMethod]
        public void PrzejdzDoAdministracji_Chrome()
        {
            LoginPage stronaLogowania = new LoginPage("Chrome", @"http://localhost/bugzilla");
            stronaLogowania.LoginIntoBugzilla("olew@poczta.onet.pl", "poziomka");

            AdministrationPage administracja = new AdministrationPage(stronaLogowania.Driver);
            Weryfikator.WeryfikujTytul("AdministrationPageOn_Chrome",
                "Administer your installation (Bugzilla 5.0.2)",
                administracja.pageTitle,
                ZW);
            
            ZW.Zapisz();
            // killing driver here
            administracja.CleanUp();
        }
        [TestMethod]
        public void PrzejdzDoUserow_Chrome()
        {
            LoginPage stronaLogowania = new LoginPage("Chrome", @"http://localhost/bugzilla");
            stronaLogowania.LoginIntoBugzilla("olew@poczta.onet.pl", "poziomka");

            AdministrationPage administracja = new AdministrationPage(stronaLogowania.Driver);

            UsersPage userzy = new UsersPage(administracja.Driver);
            Weryfikator.WeryfikujTytul("Przejdz do userow_Chrome",
               "Search users",
               userzy.pageTitle,
               ZW);
            ZW.Zapisz();

            // killing driver here
            userzy.CleanUp();
        }
        [TestMethod]
        public void Logowanie_niepoprawne_Chrome()
        {
            // test fails on purpose - to show that the framework really works
            LoginPage stronaLogowania = new LoginPage("Chrome", @"http://localhost/bugzilla");
            stronaLogowania.LoginIntoBugzilla("olew@poczta.onet.pl", "zlehaslo");

            Weryfikator.WeryfikujTytul("Logowanie Niepoprawne Chrome",
                "Welcome to Bugzilla",
                stronaLogowania.pageTitle,
                ZW);

            ZW.Zapisz();
            // killing driver here
            stronaLogowania.CleanUp();
        }

        [ClassCleanup]
        public static void OdpalPoWszystkichTestach()
        {
            string poczatekHtmla = "<!DOCTYPE html><html><head><meta charset = \"UTF-8\"></head><body><h1>Wyniki testów z użyciem Selenium dla aplikacji Bugzilla</h1><table border=\"1\"><tr><th>Nazwa testu</th><th>Wynik</th><th>Wiadomość</th></tr>";
            string koniecHtmla = @"</table></body></html>";
            StringBuilder sb = new StringBuilder();

            if (File.Exists("rezultat.html"))
            {
                sb.Append(poczatekHtmla);
                sb.Append(File.ReadAllText("rezultat.html"));
                sb.Append(koniecHtmla);
                File.Delete("rezultat.html");
                File.WriteAllText("rezultat.html", sb.ToString());
            }
            else
            {
            }
        }
    }
}
