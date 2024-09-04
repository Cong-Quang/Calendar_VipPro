using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Threading;

public class HandleChrome
{
    private IWebDriver driver; // WebDriver instance for Chrome
    private IWebElement selectElement; // WebElement for select dropdown
    private IList<IWebElement> options; // List of options in the select dropdown
    private static HandleChrome instance; // Singleton instance

    // Singleton pattern to get the instance of HandleChrome
    public static HandleChrome Instance()
    {
        if (instance == null) instance = new HandleChrome();
        return instance;
    }

    // Method to open Chrome browser and navigate to a URL
    public void OpenChrome(string url)
    {
        var driverService = ChromeDriverService.CreateDefaultService(); // Create ChromeDriver service
        driverService.HideCommandPromptWindow = true; // Hide command prompt window
        ChromeOptions options = new ChromeOptions(); // Chrome options
        options.AddArguments("--disable-notifications"); // Disable notifications
        driver = new ChromeDriver(driverService, options); // Initialize ChromeDriver
        driver.Navigate().GoToUrl(url); // Navigate to the specified URL
    }

    // Method to wait for JavaScript to load completely
    public void WaitForJavascript()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30)); // Wait for up to 30 seconds
        wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete")); // Wait until the document is fully loaded
    }

    // Method to sleep for a random time between timeMin and timeMax
    public void SleepRandom(int timeMin, int timeMax)
    {
        Random rnd = new Random(); // Random number generator
        int sleepTime = rnd.Next(timeMin, timeMax); // Generate random sleep time
        Thread.Sleep(sleepTime); // Sleep for the generated time
    }

    // Method to close Chrome browser
    public void CloseChrome(bool quit = false)
    {
        if (quit)
        {
            driver.Quit(); // Quit the driver and close all associated windows
        }
        else
        {
            driver.Close(); // Close the current window
        }
    }

    // Method to click at a specific position on the screen
    public void ClickAtPosition(int x, int y)
    {
        Actions actions = new Actions(driver); // Create Actions instance
        actions.MoveByOffset(x, y).Click().Build().Perform(); // Move to the specified position and click
    }

    // Method to navigate to a specific URL
    public void GoToUrl(string url)
    {
        driver.Navigate().GoToUrl(url); // Navigate to the specified URL
    }

    // Method to login to the application
    public void Login(string userName, string passWord)
    {
        driver.FindElement(By.Name("username")).SendKeys(userName); // Enter username
        driver.FindElement(By.Name("password")).SendKeys(passWord); // Enter password

        selectElement = driver.FindElement(By.Name("app_key")); // Find the select element
        options = selectElement.FindElements(By.TagName("option")); // Get all options

        foreach (IWebElement option in options)
        {
            if (option.Text.Trim() == "Đại học - Cao đẳng")
            {
                option.Click(); // Select the option if it matches
                break;
            }
        }
        driver.FindElement(By.XPath("//button[contains(text(), 'Đăng nhập')]")).Click(); // Click the login button
    }

    // Main method to get the schedule
    public void Test()
    {
        GoToScheduleUrl(); // Navigate to the schedule URL

        for (int i = 0; i < 8; i++) // Loop through 4 weeks
        {
            SleepRandom(2000, 2000); // Sleep for 2 seconds between weeks
            PrintWeekSchedule(); // Print the schedule for the current week
            MoveToNextWeek(); // Move to the next week
        }
    }

    // Method to navigate to the schedule URL
    private void GoToScheduleUrl()
    {
        driver.Navigate().GoToUrl("https://sinhvien.hutech.edu.vn/#/sinhvien/hoc-vu/thoi-khoa-bieu"); // Navigate to the schedule URL
        SleepRandom(2000, 2000); // Sleep for 2 seconds to allow the page to load
    }

    // Method to print the schedule for the current week
    private void PrintWeekSchedule()
    {
        IReadOnlyCollection<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'font-size-lich-thi')]//tbody//tr")); // Get all rows in the schedule table
        string currentDate = string.Empty; // Variable to store the current date
        int y = 1; // Variable to track the row position for printing

        List<School_Schedule> weekSchedule = new List<School_Schedule>(); // List to store the schedule for the week

        foreach (IWebElement row in rows)
        {
            try
            {
                IWebElement dateElement = row.FindElement(By.XPath(".//th[contains(@class, 'bagroud')]")); // Find the date element
                currentDate = dateElement.Text; // Get the date text
            }
            catch (NoSuchElementException)
            {
                School_Schedule scheduleItem = PrintClassDetails(row, currentDate, ref y); // Print class details and get the schedule item
                if (scheduleItem != null)
                {
                    weekSchedule.Add(scheduleItem); // Add the schedule item to the list
                }
            }
        }

        Data.Gi().Schedule.AddRange(weekSchedule); // Add the week's schedule to the main schedule list
    }

    // Method to print the details of a class
    private School_Schedule PrintClassDetails(IWebElement row, string currentDate, ref int y)
    {
        try
        {
            string tiet = row.FindElement(By.XPath(".//td[contains(@class, 'text-center')]")).Text; // Get the class period
            string maMonHocTenMon = row.FindElement(By.XPath(".//td[contains(@class, 'pt-4')]//p")).Text; // Get the course code and name
            string phongHoc = row.FindElement(By.XPath(".//td[contains(@class, 'pt-4')]//span//strong")).Text.TrimEnd('-').Trim(); // Get the classroom

            y++; // Increment the row position

            return new School_Schedule
            {
                ThoiGian = currentDate, // Set the date
                MaMonHocTenMon = maMonHocTenMon, // Set the course code and name
                Tiet = tiet, // Set the class period
                PhongHoc = phongHoc // Set the classroom
            };
        }
        catch (NoSuchElementException)
        {
            return null; // Return null if any element is not found
        }
    }

    // Method to move to the next week
    private void MoveToNextWeek()
    {
        IWebElement rightButton = driver.FindElement(By.CssSelector(".col-md-6 .fa-chevron-right")); // Find the button to move to the next week
        rightButton.Click(); // Click the button
    }

    // Method to show the menu
    public void ShowMenu()
    {
        Terminal.gI().Print($"[Thời gian]", 5, 0, ConsoleColor.Red); // Print the time header
        Terminal.gI().Print($"[Mã Môn học - Tên môn]", 34, 0, ConsoleColor.Red); // Print the course code and name header
        Terminal.gI().Print($"[Tiết]", 69, 0, ConsoleColor.Red); // Print the class period header
        Terminal.gI().Print($"[Phòng]", 91, 0, ConsoleColor.Red); // Print the classroom header
    }
}