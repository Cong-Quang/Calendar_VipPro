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
        // Mở trình duyệt và điều hướng đến trang đăng nhập
        HandleChrome.Instance().OpenChrome("https://sinhvien.hutech.edu.vn/#/sinhvien/login/login");

        // Đợi trang tải xong
        HandleChrome.Instance().WaitForJavascript();

        // Đăng nhập
        HandleChrome.Instance().Login("2380601806", "02082003Itcoder@");

        // Đợi trang tải xong sau khi đăng nhập
        HandleChrome.Instance().WaitForJavascript();

        // Crawl dữ liệu thời khóa biểu
        try
        {
            HandleChrome.Instance().test();
        }
        catch (Exception)
        {  // Đóng trình duyệt
            HandleChrome.Instance().CloseChrome(true);
            throw;
        }

        // Đóng trình duyệt
        HandleChrome.Instance().CloseChrome(true);

    }

    static void ShowCrawledData()
    {
        var schedules = Data.Gi().Schedule;
        if (schedules != null && schedules.Count > 0)
        {
            foreach (var schedule in schedules)
            {
                Console.WriteLine($"Thời gian: {schedule.ThoiGian}");
                Console.WriteLine($"Mã Môn học - Tên môn: {schedule.MaMonHocTenMon}");
                Console.WriteLine($"Tiết: {schedule.Tiet}");
                Console.WriteLine($"Phòng: {schedule.PhongHoc}");
                Console.WriteLine(new string('-', 50));
            }
        }
        else
        {
            Console.WriteLine("Không có dữ liệu thời khóa biểu.");
        }
    }
}
