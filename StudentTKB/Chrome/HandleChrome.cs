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
        options.AddArguments("--disable-notifications");
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
        GoToScheduleUrl();

        for (int i = 0; i < 8; i++)
        {
            SleepRandom(2000, 2000);
            PrintWeekSchedule();
            MoveToNextWeek();
        }
    }

    private void GoToScheduleUrl()
    {
        driver.Navigate().GoToUrl("https://sinhvien.hutech.edu.vn/#/sinhvien/hoc-vu/thoi-khoa-bieu");
        SleepRandom(2000, 2000);
    }

    private void PrintWeekSchedule()
    {
        IReadOnlyCollection<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'font-size-lich-thi')]//tbody//tr"));
        string currentDate = string.Empty;
        int y = 1;

        List<School_Schedule> weekSchedule = new List<School_Schedule>();

        foreach (IWebElement row in rows)
        {
            try
            {
                IWebElement dateElement = row.FindElement(By.XPath(".//th[contains(@class, 'bagroud')]"));
                currentDate = dateElement.Text;
            }
            catch (NoSuchElementException)
            {
                School_Schedule scheduleItem = PrintClassDetails(row, currentDate, ref y);
                if (scheduleItem != null)
                {
                    weekSchedule.Add(scheduleItem);
                }
            }
        }

        Data.Gi().Schedule.AddRange(weekSchedule);
    }

    private School_Schedule PrintClassDetails(IWebElement row, string currentDate, ref int y)
    {
        try
        {
            string tiet = row.FindElement(By.XPath(".//td[contains(@class, 'text-center')]")).Text;
            string maMonHocTenMon = row.FindElement(By.XPath(".//td[contains(@class, 'pt-4')]//p")).Text;
            string phongHoc = row.FindElement(By.XPath(".//td[contains(@class, 'pt-4')]//span//strong")).Text.TrimEnd('-').Trim();

            y++;

            return new School_Schedule
            {
                ThoiGian = currentDate,
                MaMonHocTenMon = maMonHocTenMon,
                Tiet = tiet,
                PhongHoc = phongHoc
            };
        }
        catch (NoSuchElementException)
        {
            return null;
        }
    }

    private void MoveToNextWeek()
    {
        IWebElement rightButton = driver.FindElement(By.XPath("//*[@id=\"content\"]/div/div/div[2]/chon-tuan-tkb-hoc-vu/div/div[3]/button"));
        rightButton.Click();
    }

    public void ShowMenu()
    {
        Terminal.gI().Print($"[Thời gian]", 5, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Mã Môn học - Tên môn]", 34, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Tiết]", 69, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Phòng]", 91, 0, ConsoleColor.Red);
    }
}