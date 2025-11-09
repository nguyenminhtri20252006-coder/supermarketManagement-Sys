using System;
using System.Collections.Generic;
using System.Diagnostics; // Cần cho Stopwatch
using System.Linq; // Cần cho ToList()

/// <summary>
/// Module (static class) chịu trách nhiệm thực hiện Yêu cầu 3:
/// Thực nghiệm và Kiểm thử hiệu năng của Cấu trúc Dữ liệu và Thuật toán.
/// (ĐÃ CẬP NHẬT: So sánh Dictionary vs List)
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
            // Đã cập nhật tên
            "1. So sánh hiệu năng CTDL (Dictionary O(1) vs List O(N))",
            "2. Kiểm thử Thuật toán Combo (Tổ hợp C(n, m))",
            "0. Quay lại Menu chính"
        };

        while (dangChay)
        {
            int luaChonIndex = ConsoleUI.HienThiMenuChon("== MODULE KIỂM THỬ HIỆU NĂNG ==", cacLuaChon);

            switch (luaChonIndex)
            {
                case 0: // 1. Kiểm thử CTDL
                    KiemThuHieuNang_CTDL_SoSanh(); // Gọi hàm so sánh mới
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
    /// (Yêu cầu 3 - Nâng cao) - Thực nghiệm so sánh hiệu năng của
    /// 2-Dictionary (O(1)) với List (O(N)).
    /// </summary>
    private static void KiemThuHieuNang_CTDL_SoSanh()
    {
        Console.Clear();
        Console.WriteLine("== SO SÁNH HIỆU NĂNG CTDL (Dictionary vs List) ==");
        
        int? soLuongTest = ConsoleUI.DocSoNguyen("Nhập số lượng (N) sản phẩm để kiểm thử (VD: 10000): ");
        if (soLuongTest == null || soLuongTest <= 0)
        {
            ConsoleUI.HienThiThongBao("Đã hủy kiểm thử.", ConsoleColor.Yellow);
            return;
        }

        int N = soLuongTest.Value;
        Console.WriteLine($"Đang chuẩn bị {N:N0} dữ liệu mẫu...");

        // Tạo dữ liệu mẫu
        List<SanPham> duLieuTest = new List<SanPham>();
        for (int i = 0; i < N; i++)
        {
            duLieuTest.Add(new SanPham($"TEST{i:D5}", $"Sản phẩm Test {i}", "Cái", 100, 10));
        }
        string maCanTim = $"TEST{N / 2:D5}"; // Lấy mã ở giữa
        string tenCanTim = $"sản phẩm test {N / 3:D5}"; // Lấy tên ở 1/3
        string tenKhongTimThay = "ten_khong_ton_tai";

        // Xóa dữ liệu cũ (nếu có)
        QuanLySanPham.dsTheoMa.Clear();
        QuanLySanPham.dsTheoTen.Clear();
        QuanLySanPham_List.XoaTatCa();

        // Khởi tạo đồng hồ
        Stopwatch sw = new Stopwatch();

        // Bảng kết quả (dùng List<string> để dễ định dạng)
        var ketQua = new List<Tuple<string, string, string>>();

        // 1. Kiểm thử THÊM (O(1) vs O(1))
        Console.WriteLine($"\n1. Đang kiểm thử Thêm {N:N0} sản phẩm...");
        sw.Start();
        foreach (var sp in duLieuTest)
        {
            QuanLySanPham.ThemSanPham(sp); // Dictionary (O(1))
        }
        sw.Stop();
        string thoiGianDict_Them = $"{sw.Elapsed.TotalMilliseconds:F4} ms";

        sw.Restart();
        foreach (var sp in duLieuTest)
        {
            QuanLySanPham_List.ThemSanPham(sp); // List.Add (O(1))
        }
        sw.Stop();
        string thoiGianList_Them = $"{sw.Elapsed.TotalMilliseconds:F4} ms";
        ketQua.Add(Tuple.Create($"Thêm {N:N0} SP (Tổng)", thoiGianDict_Them, thoiGianList_Them));


        // 2. Kiểm thử TÌM THEO MÃ (O(1) vs O(N))
        Console.WriteLine($"\n2. Đang kiểm thử Tìm theo Mã (O(1) vs O(N))...");
        sw.Restart();
        QuanLySanPham.TimTheoMa(maCanTim, out _);
        sw.Stop();
        string thoiGianDict_TimMa = $"{sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)";

        sw.Restart();
        QuanLySanPham_List.TimTheoMa(maCanTim, out _);
        sw.Stop();
        string thoiGianList_TimMa = $"{sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)";
        ketQua.Add(Tuple.Create("Tìm Mã (Tồn tại)", thoiGianDict_TimMa, thoiGianList_TimMa));

        // 3. Kiểm thử TÌM THEO TÊN (O(1) vs O(N))
        Console.WriteLine($"\n3. Đang kiểm thử Tìm theo Tên (O(1) vs O(N))...");
        sw.Restart();
        QuanLySanPham.TimTheoTen(tenCanTim);
        sw.Stop();
        string thoiGianDict_TimTen = $"{sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)";

        sw.Restart();
        QuanLySanPham_List.TimTheoTen(tenCanTim);
        sw.Stop();
        string thoiGianList_TimTen = $"{sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)";
        ketQua.Add(Tuple.Create("Tìm Tên (Tồn tại)", thoiGianDict_TimTen, thoiGianList_TimTen));
        
        // 4. Kiểm thử TÌM TÊN (Không tồn tại - O(1) vs O(N))
        Console.WriteLine($"\n4. Đang kiểm thử Tìm Tên (Không tồn tại)...");
        sw.Restart();
        QuanLySanPham.TimTheoTen(tenKhongTimThay);
        sw.Stop();
        string thoiGianDict_TimTenFail = $"{sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)";

        sw.Restart();
        QuanLySanPham_List.TimTheoTen(tenKhongTimThay);
        sw.Stop();
        string thoiGianList_TimTenFail = $"{sw.Elapsed.TotalMilliseconds:F4} ms ({sw.ElapsedTicks} ticks)";
        ketQua.Add(Tuple.Create("Tìm Tên (Không thấy)", thoiGianDict_TimTenFail, thoiGianList_TimTenFail));

        // 5. Kiểm thử XÓA (O(1) vs O(N))
        Console.WriteLine($"\n5. Đang kiểm thử Xóa {N:N0} sản phẩm...");
        sw.Restart();
        foreach (var sp in duLieuTest)
        {
            QuanLySanPham.XoaSanPham(sp.MaSP); // Dictionary (O(1))
        }
        sw.Stop();
        string thoiGianDict_Xoa = $"{sw.Elapsed.TotalMilliseconds:F4} ms";

        sw.Restart();
        foreach (var sp in duLieuTest)
        {
            QuanLySanPham_List.XoaSanPham(sp.MaSP); // List (O(N) để tìm + O(N) để xóa)
        }
        sw.Stop();
        string thoiGianList_Xoa = $"{sw.Elapsed.TotalMilliseconds:F4} ms";
        ketQua.Add(Tuple.Create($"Xóa {N:N0} SP (Tổng)", thoiGianDict_Xoa, thoiGianList_Xoa));
        
        // 6. In Bảng Kết Quả
        Console.Clear();
        Console.WriteLine($"== BẢNG KẾT QUẢ SO SÁNH HIỆU NĂNG (N = {N:N0}) ==");
        Console.WriteLine();
        Console.WriteLine($"| {"Phép toán", -25} | {"Dictionary (Tối ưu)", -25} | {"List (Cơ bản)", -25} |");
        Console.WriteLine($"| {new string('-', 25)} | {new string('-', 25)} | {new string('-', 25)} |");
        
        foreach (var (phepToan, dict, list) in ketQua)
        {
            Console.WriteLine($"| {phepToan, -25} | {dict, -25} | {list, -25} |");
        }
        
        Console.WriteLine("\n--- KẾT THÚC KIỂM THỬ CTDL ---");
        ConsoleUI.HienThiThongBao("Kiểm thử hoàn tất. Kết quả 'ticks' cho thấy phép toán O(1) của Dictionary nhanh hơn đáng kể so với O(N) của List.", ConsoleColor.Green);
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