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
            "3. Tạo (GHI ĐÈ) 100 dữ liệu mẫu vào JSON", // Mục mới
            "0. Quay lại Menu chính" // Số thứ tự đã thay đổi
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
                case 2: // 3. Tạo dữ liệu mẫu (Mục mới)
                    TaoVaLuuDuLieuMau();
                    break;
                case 3: // 0. Quay lại (Số thứ tự đã thay đổi)
                    dangChay = false;
                    break;
                case -1: // Esc
                    dangChay = false;
                    break;
            }
        }
    }

    /// <summary>
    /// (BỔ SUNG MỚI)
    /// Tạo 100 sản phẩm mẫu và ghi đè vào Data/sanpham.json.
    /// </summary>
    private static void TaoVaLuuDuLieuMau()
    {
        Console.Clear();
        Console.WriteLine("== TẠO DỮ LIỆU MẪU (100 SẢN PHẨM) ==");
        
        if (!ConsoleUI.XacNhan("CẢNH BÁO: Thao tác này sẽ XÓA SẠCH RAM và GHI ĐÈ file Data/sanpham.json. Bạn có chắc chắn?"))
        {
            ConsoleUI.HienThiThongBao("Đã hủy thao tác.", ConsoleColor.Yellow);
            return;
        }

        Console.WriteLine("Đang xóa dữ liệu cũ trên RAM...");
        // Xóa sạch RAM
        QuanLySanPham.dsTheoMa.Clear();
        QuanLySanPham.dsTheoTen.Clear();
        QuanLySanPham_List.XoaTatCa();

        Console.WriteLine("Đang tạo 30 sản phẩm mẫu...");
        var duLieuMau = new List<SanPham>
        {
            new SanPham("SP001", "Sữa tươi Vinamilk 1L", "Hộp", 35000, 150),
            new SanPham("SP002", "Bánh mì Sandwich", "Gói", 25000, 300),
            new SanPham("SP003", "Nước suối Aquafina 500ml", "Chai", 5000, 1000),
            new SanPham("SP004", "Thịt ba rọi bò Mỹ", "Kg", 250000, 50),
            new SanPham("SP005", "Táo Envy New Zealand", "Kg", 120000, 80),
            new SanPham("SP006", "Coca-Cola Zero", "Lon", 10000, 500),
            new SanPham("SP007", "Dầu ăn Tường An 1L", "Chai", 45000, 200),
            new SanPham("SP008", "Gạo ST25", "Túi 5Kg", 180000, 120),
            new SanPham("SP009", "Trứng gà ta", "Vỉ 10", 40000, 250),
            new SanPham("SP010", "Cà phê G7 Trung Nguyên", "Hộp", 55000, 300),
 new SanPham("SP011", "Cá hồi Na-uy", "Kg", 450000, 30),
            new SanPham("SP012", "Nho đen không hạt Mỹ", "Kg", 180000, 70),
            new SanPham("SP013", "Bia Heineken", "Thùng 24L", 420000, 100),
            new SanPham("SP014", "Nước mắm Nam Ngư", "Chai 750ml", 30000, 200),
            new SanPham("SP015", "Tương ớt Chinsu", "Chai 250g", 12000, 400),
            new SanPham("SP016", "Kem đánh răng P/S", "Tuýp", 28000, 150),
            new SanPham("SP017", "Sữa chua Vinamilk", "Lốc 4", 22000, 300),
            new SanPham("SP018", "Bột giặt Omo 3Kg", "Túi", 150000, 60),
            new SanPham("SP019", "Nước xả Comfort", "Chai 1.8L", 90000, 70),
            new SanPham("SP020", "Khăn giấy Pulppy", "Bịch", 30000, 100),
            new SanPham("SP021", "Mì Hảo Hảo", "Thùng", 120000, 200),
            new SanPham("SP022", "Phô mai Con Bò Cười", "Hộp 16", 50000, 90),
            new SanPham("SP023", "Bánh Chocopie", "Hộp 12", 52000, 110),
            new SanPham("SP024", "Xúc xích CP", "Gói 500g", 65000, 80),
            new SanPham("SP025", "Nấm kim châm", "Gói", 15000, 50),
            new SanPham("SP026", "Rau muống VietGap", "Bó", 18000, 40),
            new SanPham("SP027", "Cải thìa VietGap", "Bó", 20000, 40),
            new SanPham("SP028", "Nước rửa chén Sunlight", "Chai", 38000, 130),
            new SanPham("SP029", "Sữa đặc Ông Thọ", "Lon", 23000, 200),
            new SanPham("SP030", "Bơ lạt Anchor", "Khối 200g", 85000, 50)
        };

        // Thêm 90 sản phẩm tự động
        for (int i = 11; i <= 100; i++)
        {
            duLieuMau.Add(new SanPham($"SP{i:D3}", $"Sản phẩm tự động {i}", "Cái", i * 1000, i * 2));
        }

        Console.WriteLine("Đang nạp dữ liệu mẫu vào RAM...");
        foreach (var sp in duLieuMau)
        {
            QuanLySanPham.ThemSanPham(sp);
        }

        Console.WriteLine("Đang lưu RAM vào file Data/sanpham.json...");
        // Gọi hàm lưu trữ
        Database.LuuDuLieu();

        ConsoleUI.HienThiThongBao($"Đã tạo và ghi đè thành công {duLieuMau.Count} sản phẩm vào Data/sanpham.json.", ConsoleColor.Green);
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