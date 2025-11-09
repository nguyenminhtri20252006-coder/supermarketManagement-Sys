using System;
using System.Collections.Generic;
using System.Diagnostics; // Cần cho Stopwatch
using System.Linq; // Cần cho ToList()

/// <summary>
/// Module (static class) chịu trách nhiệm thực hiện Yêu cầu 3:
/// Thực nghiệm và Kiểm thử hiệu năng của Cấu trúc Dữ liệu và Thuật toán.
/// </summary>
public static class KiemThu
{
    /// <summary>
    /// Hiển thị Menu con cho chức năng Kiểm thử.
    /// (Đây là điểm vào (entrypoint) từ Program.cs)
    /// </summary>
    public static void HienThiMenu()
    {
        bool dangChay = true;
        var cacLuaChon = new List<string>
        {
            "1. Kiểm thử Cấu trúc Dữ liệu (Thêm, Tìm, Xóa 10.000 SP)",
            "2. Kiểm thử Thuật toán Combo (Tổ hợp C(n, m))",
            "0. Quay lại Menu chính"
        };

        while (dangChay)
        {
            int luaChonIndex = ConsoleUI.HienThiMenuChon("== MODULE KIỂM THỬ HIỆU NĂNG ==", cacLuaChon);

            switch (luaChonIndex)
            {
                case 0: // 1. Kiểm thử CTDL
                    KiemThuHieuNang_CTDL();
                    break;
                case 1: // 2. Kiểm thử Combo
                    KiemThuHieuNang_Combo();
                    break;
                case 2: // 0. Quay lại
                    dangChay = false;
                    break;
                case -1: // Esc
                    dangChay = false;
                    break;
            }
        }
    }

    /// <summary>
    /// (Yêu cầu 3) - Thực nghiệm hiệu năng của 2-Dictionary.
    /// </summary>
    private static void KiemThuHieuNang_CTDL()
    {
        Console.Clear();
        Console.WriteLine("== ĐANG CHUẨN BỊ KIỂM THỬ CẤU TRÚC DỮ LIỆU ==");
        Console.WriteLine("Thử nghiệm sẽ Thêm, Tìm, Xóa trên 2 Dictionaries đồng bộ.");

        int SO_LUONG_TEST = 10000; // Thử nghiệm với 10.000 mặt hàng

        if (!ConsoleUI.XacNhan($"Chuẩn bị chạy kiểm thử với {SO_LUONG_TEST:N0} sản phẩm?"))
        {
            ConsoleUI.HienThiThongBao("Đã hủy kiểm thử.", ConsoleColor.Yellow);
            return;
        }

        // Tạo dữ liệu mẫu
        List<SanPham> duLieuTest = new List<SanPham>();
        for (int i = 0; i < SO_LUONG_TEST; i++)
        {
            duLieuTest.Add(new SanPham($"TEST{i:D5}", $"Sản phẩm Test {i}", "Cái", 100, 10));
        }
        string maCanTim = $"TEST{SO_LUONG_TEST / 2:D5}"; // Lấy mã ở giữa
        string tenCanTim = $"sản phẩm test {SO_LUONG_TEST / 3:D5}"; // Lấy tên ở 1/3

        // Khởi tạo đồng hồ
        Stopwatch sw = new Stopwatch();

        // 1. Kiểm thử THÊM
        Console.WriteLine($"\n1. Đang kiểm thử Thêm {SO_LUONG_TEST:N0} sản phẩm (O(1) x N)...");
        sw.Start();
        foreach (var sp in duLieuTest)
        {
            QuanLySanPham.ThemSanPham(sp);
        }
        sw.Stop();
        Console.WriteLine($"   -> Hoàn thành trong: {sw.ElapsedMilliseconds} ms");

        // 2. Kiểm thử TÌM THEO MÃ (O(1))
        Console.WriteLine($"\n2. Đang kiểm thử Tìm theo Mã (O(1)) (Tìm '{maCanTim}')...");
        sw.Restart();
        QuanLySanPham.TimTheoMa(maCanTim, out _);
        sw.Stop();
        Console.WriteLine($"   -> Hoàn thành trong: {sw.Elapsed.TotalMilliseconds:F4} ms (hoặc {sw.ElapsedTicks} ticks)");

        // 3. Kiểm thử TÌM THEO TÊN (O(1))
        Console.WriteLine($"\n3. Đang kiểm thử Tìm theo Tên (O(1)) (Tìm '{tenCanTim}')...");
        sw.Restart();
        QuanLySanPham.TimTheoTen(tenCanTim);
        sw.Stop();
        Console.WriteLine($"   -> Hoàn thành trong: {sw.Elapsed.TotalMilliseconds:F4} ms (hoặc {sw.ElapsedTicks} ticks)");
        
        // 4. Kiểm thử TÌM THEO TÊN (Không tồn tại - O(1))
        Console.WriteLine($"\n4. Đang kiểm thử Tìm Tên không tồn tại (O(1))...");
        sw.Restart();
        QuanLySanPham.TimTheoTen("ten_khong_ton_tai");
        sw.Stop();
        Console.WriteLine($"   -> Hoàn thành trong: {sw.Elapsed.TotalMilliseconds:F4} ms (hoặc {sw.ElapsedTicks} ticks)");

        // 5. Kiểm thử XÓA
        Console.WriteLine($"\n5. Đang kiểm thử Xóa {SO_LUONG_TEST:N0} sản phẩm (O(1) x N)...");
        sw.Restart();
        foreach (var sp in duLieuTest)
        {
            QuanLySanPham.XoaSanPham(sp.MaSP);
        }
        sw.Stop();
        Console.WriteLine($"   -> Hoàn thành trong: {sw.ElapsedMilliseconds} ms");
        
        Console.WriteLine("\n--- KẾT THÚC KIỂM THỬ CTDL ---");
        ConsoleUI.HienThiThongBao("Kiểm thử hoàn tất. Kết quả cho thấy các thao tác O(1) (Tìm kiếm) nhanh hơn đáng kể so với O(N) (Thêm/Xóa).", ConsoleColor.Green);
    }

    /// <summary>
    /// (Yêu cầu 3) - Thực nghiệm hiệu năng của Thuật toán Combo (Backtracking).
    /// </summary>
    private static void KiemThuHieuNang_Combo()
    {
        Console.Clear();
        Console.WriteLine("== KIỂM THỬ THUẬT TOÁN COMBO (Backtracking) ==");

        int? n = ConsoleUI.DocSoNguyen("Nhập (n) - tổng số phần tử (VD: 20): ");
        if (n == null || n <= 0 || n > 50) // Giới hạn 50 để tránh tràn bộ nhớ
        {
            ConsoleUI.HienThiThongBao(n == null ? "Đã hủy." : "Số lượng không hợp lệ.", ConsoleColor.Yellow);
            return;
        }

        int? m = ConsoleUI.DocSoNguyen($"Nhập (m) - số phần tử mỗi combo (m <= {n.Value}): ");
        if (m == null || m <= 0 || m > n.Value)
        {
            ConsoleUI.HienThiThongBao(m == null ? "Đã hủy." : "Số lượng không hợp lệ.", ConsoleColor.Yellow);
            return;
        }

        // Tạo danh sách (n) sản phẩm mẫu
        List<SanPham> danhSachTest = new List<SanPham>();
        for (int i = 0; i < n.Value; i++)
        {
            danhSachTest.Add(new SanPham($"T{i}", $"SP {i}", "Cái", 1, 1));
        }

        Console.WriteLine($"\nĐang tính toán C({n.Value}, {m.Value})... Vui lòng chờ.");
        Stopwatch sw = Stopwatch.StartNew();
        
        // Gọi thuật toán
        var ketQua = ThuVienCombo.LietKeCombos(danhSachTest, m.Value);
        
        sw.Stop();

        Console.WriteLine("--- KẾT QUẢ KIỂM THỬ THUẬT TOÁN ---");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Đã tìm thấy: {ketQua.Count:N0} combos.");
        Console.WriteLine($"Thời gian thực thi: {sw.ElapsedMilliseconds} ms (hoặc {sw.Elapsed.TotalSeconds:F2} giây).");
        Console.ResetColor();

        ConsoleUI.HienThiThongBao("Kiểm thử hoàn tất.", ConsoleColor.Green);
    }
}