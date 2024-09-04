using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Threading;

public class HandleChrome
{
    private IWebDriver driver;
    private IWebElement selectElement;
    private IList<IWebElement> options;
    private static HandleChrome instance;

    public static HandleChrome Instance()
    {
        if (instance == null) instance = new HandleChrome();
        return instance;
    }

    public void OpenChrome(string url)
    {
        var driverService = ChromeDriverService.CreateDefaultService();
        driverService.HideCommandPromptWindow = true;
        ChromeOptions options = new ChromeOptions();
        options.AddArguments("--disable-notifications", "headless");
        driver = new ChromeDriver(driverService, options);
        driver.Navigate().GoToUrl(url);
    }

    public void WaitForJavascript()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
    }

    public void SleepRandom(int timeMin, int timeMax)
    {
        Random rnd = new Random();
        int sleepTime = rnd.Next(timeMin, timeMax);
        Thread.Sleep(sleepTime);
    }

    public void CloseChrome(bool quit = false)
    {
        if (quit)
        {
            driver.Quit();
        }
        else
        {
            driver.Close();
        }
    }

    public void ClickAtPosition(int x, int y)
    {
        Actions actions = new Actions(driver);
        actions.MoveByOffset(x, y).Click().Build().Perform();
    }

    public void GoToUrl(string url)
    {
        driver.Navigate().GoToUrl(url);
    }

    public void Login(string userName, string passWord)
    {
        driver.FindElement(By.Name("username")).SendKeys(userName);
        driver.FindElement(By.Name("password")).SendKeys(passWord);
        
        selectElement = driver.FindElement(By.Name("app_key"));
        options = selectElement.FindElements(By.TagName("option"));

        foreach (IWebElement option in options)
        {
            if (option.Text.Trim() == "Đại học - Cao đẳng")
            {
                option.Click();
                break;
            }
        }
        driver.FindElement(By.XPath("//button[contains(text(), 'Đăng nhập')]")).Click();
    }

    public void Test()
    {
        List<School_Schedule> schedules = new List<School_Schedule>();
        for (int i = 0; i < 4; i++)
        {
            Thread.Sleep(2000);
            List<string> ThoiGian = new List<string>();
            int CountListTime = 0;

            IReadOnlyCollection<IWebElement> rows = driver.FindElements(By.CssSelector(".table.font-size-lich-thi tbody > tr"));
            foreach (IWebElement row in rows)
            {
                try
                {
                    ThoiGian.Add(row.FindElement(By.CssSelector("th.bagroud")).Text);
                }
                catch (NoSuchElementException) { }
            }

            foreach (IWebElement row in rows)
            {
                try
                {
                    string tiet = row.FindElement(By.CssSelector("td.text-center")).Text;
                    string maMonHocTenMon = row.FindElement(By.CssSelector("td.pt-4 p:nth-child(1)")).Text;
                    string phongHoc = row.FindElement(By.CssSelector("td.pt-4 span strong")).Text;

                    schedules.Add(new School_Schedule
                    {
                        ThoiGian = ThoiGian[CountListTime],
                        MaMonHocTenMon = maMonHocTenMon,
                        Tiet = tiet,
                        PhongHoc = phongHoc
                    });

                    CountListTime++;
                }
                catch (NoSuchElementException) { }
            }

            IWebElement rightButton = driver.FindElement(By.CssSelector(".col-md-6 .fa-chevron-right"));
            ThoiGian.Clear();
            rightButton.Click();
        }

        // Lưu thông tin vào Data
        Data.Gi().Schedule = schedules;
    }

    public void ShowMenu()
    {
        Terminal.gI().Print($"[Thời gian]", 5, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Mã Môn học - Tên môn]", 34, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Tiết]", 69, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Phòng]", 91, 0, ConsoleColor.Red);
    }
}