using System;
using System.Text;
using System.IO;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Setup();
        RunCrawl();
    }

    static void Setup()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Data.Gi();
    }

    static void RunCrawl()
    {
        HandleChrome.Instance().OpenChrome("https://sinhvien.hutech.edu.vn/#/sinhvien/login/login");
        Thread.Sleep(2000);
        HandleChrome.Instance().Login(Data.Gi().Setting.userName, Data.Gi().Setting.password);
        Thread.Sleep(2000);
        try
        {
            HandleChrome.Instance().Test();
            ShowCrawledData(); // Add this line to show the crawled data
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            HandleChrome.Instance().CloseChrome(true);
            throw;
        }

        HandleChrome.Instance().CloseChrome(true);
    }

    static void ShowCrawledData()
    {
        var schedules = Data.Gi().Schedule;

        int i = 0;
        foreach (var schedule in schedules)
        {
            Console.WriteLine($"{i++}" + new string('-', 80));

            Console.WriteLine($"Thời gian: {schedule.ThoiGian}");
            Console.WriteLine($"Mã Môn học - Tên môn: {schedule.MaMonHocTenMon}");
            Console.WriteLine($"Tiết: {schedule.Tiet}");
            Console.WriteLine($"Phòng: {schedule.PhongHoc}");
        }
    }
}
