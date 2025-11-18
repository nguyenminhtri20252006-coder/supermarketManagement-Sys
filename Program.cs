using System;
using System.Text;
using System.Collections.Generic;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title = "Báo cáo CTDL & GT - Hệ thống Siêu thị";

        // Tự động tải dữ liệu
        Database.LoadDuLieu(); 

        if(QuanLySanPham.dsTheoMa.Count == 0)
        {
            Console.WriteLine("\nCẢNH BÁO: Chưa có dữ liệu. Vui lòng vào menu Kiểm thử -> Tạo dữ liệu mẫu.");
        }

        Console.WriteLine("\nHệ thống Quản lý Siêu thị (CTDL & GT)");
        Console.WriteLine("========================================");
        Console.WriteLine("Nhấn phím bất kỳ để vào Menu chính...");
        Console.ReadKey();

        HienThiMenuChinh();

        Database.LuuDuLieu();
        Console.WriteLine("Đã lưu dữ liệu. Tạm biệt!");
    }

    private static void HienThiMenuChinh()
    {
        bool dangChay = true;
        var cacLuaChon = new List<string>
        {
            "1. Quản lý Sản phẩm (CRUD - Dictionary)",
            "2. Quản lý Kho (Sort & Thống kê)",
            "3. Quản lý Bán hàng (Queue - Hàng đợi)",
            "4. Liệt kê Combo (Đệ quy)",
            "5. Kiểm thử hiệu năng (So sánh CTDL)",
            "0. Thoát và Lưu"
        };

        while (dangChay)
        {
            int luaChonIndex = ConsoleUI.HienThiMenuChon("== MENU CHÍNH (Báo cáo CTDL & GT) ==", cacLuaChon);

            switch (luaChonIndex)
            {
                case 0: QuanLySanPham.HienThiMenu(); break;
                case 1: QuanLyKho.HienThiMenu(); break;
                case 2: QuanLyBanHang.HienThiMenu(); break;
                case 3: ThuVienCombo.HienThiMenu(); break;
                case 4: KiemThu.HienThiMenu(); break;
                case 5: dangChay = false; break;
                case -1: dangChay = false; break;
            }
        }
    }
}