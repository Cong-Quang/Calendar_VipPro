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
        // Removed "headless" argument
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

    public void test()
    {
        // Điều hướng đến URL của thời khóa biểu
        driver.Navigate().GoToUrl("https://sinhvien.hutech.edu.vn/#/sinhvien/hoc-vu/thoi-khoa-bieu");

        // Tạm dừng 2 giây để trang tải
        Thread.Sleep(2000);
        int y = 1;
        for (int i = 0; i < 4; i++)
        {
            Thread.Sleep(2000);
            IReadOnlyCollection<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'font-size-lich-thi')]//tbody//tr"));
            var rows2 = rows;
            foreach (IWebElement rowx in rows2)
            {
                try
                {
                    foreach (IWebElement row in rows)
                    {
                        try
                        {
                            // Lấy tiết học từ cột có class 'text-center'
                            string tiet = row.FindElement(By.XPath(".//td[contains(@class, 'text-center')]")).Text;

                            // Lấy mã môn học và tên môn từ cột có class 'pt-4'
                            string maMonHocTenMon = row.FindElement(By.XPath(".//td[contains(@class, 'pt-4')]//p")).Text;

                            // Lấy phòng học từ cột có class 'pt-4'
                            string phongHoc = row.FindElement(By.XPath(".//td[contains(@class, 'pt-4')]//span//strong")).Text;


                            Terminal.gI().Print($"{rowx.FindElement(By.XPath(".//th[contains(@class, 'bagroud')]")).Text}", 0, y, ConsoleColor.Green);
                            Terminal.gI().Print($"{maMonHocTenMon}", 30, y, ConsoleColor.Blue);
                            Terminal.gI().Print($"{tiet}", 70, y, ConsoleColor.Green);
                            Terminal.gI().Print($"{phongHoc}", 90, y, ConsoleColor.Blue);

                            y++;
                        }
                        catch (NoSuchElementException)
                        {
                            // Bỏ qua hàng không có các phần tử cần truy cập
                        }
                    }
                }
                catch (NoSuchElementException)
                {
                }
            }
            
            IWebElement rightButton = driver.FindElement(By.CssSelector(".col-md-6 .fa-chevron-right"));
            rightButton.Click();
        }
        Console.ResetColor();
    }

    public void ShowMenu()
    {
        Terminal.gI().Print($"[Thời gian]", 5, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Mã Môn học - Tên môn]", 34, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Tiết]", 69, 0, ConsoleColor.Red);
        Terminal.gI().Print($"[Phòng]", 91, 0, ConsoleColor.Red);
    }
}