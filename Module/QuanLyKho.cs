using System;
using System.Collections.Generic;
using System.Linq;

public static class QuanLyKho
{
    public static void HienThiMenu()
    {
        bool dangChay = true;
        var cacLuaChon = new List<string>
        {
            "1. Cảnh báo hàng sắp hết (Tồn < 10)",
            "2. Thống kê tổng giá trị kho hàng",
            "3. Nhập thêm hàng (Cập nhật tồn kho)",
            "0. Quay lại Menu chính"
        };

        while (dangChay)
        {
            int luaChon = ConsoleUI.HienThiMenuChon("== QUẢN LÝ KHO & THỐNG KÊ ==", cacLuaChon);
            switch (luaChon)
            {
                case 0: CanhBaoHangSapHet(); break;
                case 1: ThongKeTongGiaTri(); break;
                case 2: NhapHang(); break;
                case 3: dangChay = false; break;
                case -1: dangChay = false; break;
            }
        }
    }

    private static void CanhBaoHangSapHet()
    {
        Console.Clear();
        Console.WriteLine("== CẢNH BÁO HÀNG SẮP HẾT (Sorting O(N log N)) ==");

        var tatCaSP = QuanLySanPham.dsTheoMa.Values;
        // Sắp xếp tăng dần theo tồn kho
        var hangSapHet = tatCaSP
            .Where(p => p.SoLuongTonKho < 10)
            .OrderBy(p => p.SoLuongTonKho) 
            .ToList();

        if (hangSapHet.Count == 0)
        {
            ConsoleUI.HienThiThongBao("Kho hàng ổn định. Không có sản phẩm nào dưới mức an toàn.", ConsoleColor.Green);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nCẢNH BÁO: {hangSapHet.Count} SẢN PHẨM CẦN NHẬP GẤP:\n");
        Console.ResetColor();
        Console.WriteLine($"{"Mã SP",-10} | {"Tên Sản Phẩm",-30} | {"Tồn Kho",10}");
        Console.WriteLine(new string('-', 55));

        foreach (var sp in hangSapHet)
        {
            Console.WriteLine($"{sp.MaSP,-10} | {sp.TenSP,-30} | {sp.SoLuongTonKho,10}");
        }
        Console.ReadKey(true);
    }

    private static void ThongKeTongGiaTri()
    {
        Console.Clear();
        decimal tongGiaTri = 0;
        foreach (var sp in QuanLySanPham.dsTheoMa.Values)
        {
            tongGiaTri += (sp.GiaBan * sp.SoLuongTonKho);
        }
        Console.WriteLine($"Tổng số mặt hàng: {QuanLySanPham.dsTheoMa.Count:N0}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"TỔNG TÀI SẢN KHO: {tongGiaTri:N0} VNĐ");
        Console.ResetColor();
        ConsoleUI.HienThiThongBao("Thống kê hoàn tất.", ConsoleColor.Yellow);
    }

    private static void NhapHang()
    {
        Console.Clear();
        string? maSP = ConsoleUI.DocChuoi("Nhập Mã SP cần nhập thêm: ", "", true);
        if (maSP == null) return;

        if (QuanLySanPham.TimTheoMa(maSP, out SanPham sp))
        {
            Console.WriteLine($"Tồn hiện tại: {sp.SoLuongTonKho}");
            int? soLuongNhap = ConsoleUI.DocSoNguyen("Nhập số lượng thêm: ");
            if (soLuongNhap != null && soLuongNhap > 0)
            {
                sp.SoLuongTonKho += soLuongNhap.Value;
                QuanLySanPham.dsTheoMa[maSP] = sp;
                ConsoleUI.HienThiThongBao("Đã nhập hàng thành công.", ConsoleColor.Green);
            }
        }
        else ConsoleUI.HienThiThongBao("Không tìm thấy sản phẩm!", ConsoleColor.Red);
    }
}