using System;
using System.Text; // Cần cho Encoding
using System.Collections.Generic; // Cần cho List
using System.Threading; // Cần cho Thread.Sleep

/// <summary>
/// Lớp chính của chương trình Console.
/// Phong cách thủ tục, đóng vai trò là điểm vào (Entrypoint).
/// </summary>
public static class Program
{
    /// <summary>
    /// Điểm khởi động chính của ứng dụng.
    /// </summary>
    public static void Main(string[] args)
    {
        // Yêu cầu 4: Đảm bảo Console hỗ trợ Tiếng Việt (có dấu)
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.Title = "Báo cáo CTDL & GT - Quản lý Siêu thị";

        // Bước 1: (Yêu cầu 5) Tải dữ liệu từ "DB" (tệp JSON)
        Database.LoadDuLieu(); // Đã kích hoạt

        Console.WriteLine("Hệ thống Quản lý Siêu thị (CTDL & GT)");
        Console.WriteLine("========================================");
        Console.WriteLine("Đã tải dữ liệu. Nhấn phím bất kỳ để vào Menu chính...");
        Console.ReadKey();

        // Bước 2: Hiển thị Menu chính
        HienThiMenuChinh();

        // Bước 3: (Yêu cầu 5) Lưu dữ liệu khi kết thúc
        Database.LuuDuLieu(); // Đã kích hoạt
        Console.WriteLine("Đã lưu dữ liệu. Tạm biệt!");
    }

    /// <summary>
    /// Hiển thị Menu chính và điều hướng lựa chọn của người dùng.
    /// (ĐÃ NÂNG CẤP LÊN KEY-DRIVEN - Yêu cầu 6)
    /// </summary>
    private static void HienThiMenuChinh()
    {
        // Giả lập Menu chính (sẽ nâng cấp bằng ConsoleUI sau)
        bool dangChay = true;
        
        // Danh sách các lựa chọn cho menu
        var cacLuaChon = new List<string>
        {
            "1. Quản lý Sản phẩm (CRUD)",
            "2. Liệt kê Combo Tết (Thuật toán)",
            "3. Kiểm thử hiệu năng (Unit Test)",
            "0. Thoát và Lưu"
        };

        while (dangChay)
        {
            // Sử dụng module ConsoleUI mới
            int luaChonIndex = ConsoleUI.HienThiMenuChon("== MENU CHÍNH (Báo cáo CTDL & GT) ==", cacLuaChon);

            switch (luaChonIndex)
            {
                case 0: // 1. Quản lý Sản phẩm
                    // Gọi hàm HienThiMenu() của module QuanLySanPham
                    QuanLySanPham.HienThiMenu(); 
                    break;
                case 1: // 2. Liệt kê Combo Tết
                    // Gọi hàm HienThiMenu() của module ThuVienCombo
                    ThuVienCombo.HienThiMenu();
                    break;
                case 2: // 3. Kiểm thử
                    // (CẬP NHẬT) - Bỏ stub, gọi module thật
                    KiemThu.HienThiMenu();
                    break;
                case 3: // 0. Thoát
                    dangChay = false;
                    break;
                case -1: // Nhấn ESC
                    dangChay = false;
                    break;
            }
        }
    }
}