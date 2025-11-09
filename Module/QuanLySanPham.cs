using System; // Cần cho Console
using System.Collections.Generic;
using System.Linq; // Cần cho .ToList()

/// <summary>
/// Module lõi (static class) chứa các cấu trúc dữ liệu chính 
/// và các hàm nghiệp vụ để quản lý Sản phẩm (Yêu cầu 2.1).
/// </summary>
public static class QuanLySanPham
{
    // Yêu cầu 2.1 (Chính): Cấu trúc dữ liệu Hash Table (Dictionary)
    // Dùng để Thêm, Xóa, Sửa, Tìm theo MaSP (O(1)).
    // Phải là 'static' để các module khác (Database, Program) có thể truy cập.
    public static Dictionary<string, SanPham> dsTheoMa = new Dictionary<string, SanPham>();

    // Yêu cầu 2.1 (Tối ưu): Cấu trúc dữ liệu Index (Hash Table lồng)
    // Dùng để Tìm theo TenSP (O(1)).
    // Key là TenSP (đã ToLower()), Value là danh sách các MaSP có tên đó.
    public static Dictionary<string, List<string>> dsTheoTen = new Dictionary<string, List<string>>();

    /* --- TRIỂN KHAI CÁC HÀM CỦA YÊU CẦU 2.1 --- */

    /// <summary>
    /// Thêm một sản phẩm mới vào cả hai cấu trúc dữ liệu.
    /// (Yêu cầu 2.1 - Thêm)
    /// </summary>
    /// <returns>Trả về true nếu thêm thành công, false nếu mã SP đã tồn tại.</returns>
    public static bool ThemSanPham(SanPham sp)
    {
        // 1. Kiểm tra trong dsTheoMa (O(1))
        if (dsTheoMa.ContainsKey(sp.MaSP))
        {
            return false; // Mã đã tồn tại
        }

        // 2. Thêm vào dsTheoMa (O(1))
        dsTheoMa.Add(sp.MaSP, sp);

        // 3. Cập nhật index dsTheoTen (O(1) trung bình)
        string tenKey = sp.TenSP.ToLowerInvariant();
        if (!dsTheoTen.ContainsKey(tenKey))
        {
            // Nếu chưa có tên này, tạo list mới
            dsTheoTen.Add(tenKey, new List<string>());
        }
        dsTheoTen[tenKey].Add(sp.MaSP);

        return true;
    }

    /// <summary>
    /// Xóa một sản phẩm khỏi cả hai cấu trúc dữ liệu.
    /// (Yêu cầu 2.1 - Xóa)
    /// </summary>
    /// <returns>Trả về true nếu xóa thành công, false nếu không tìm thấy mã SP.</returns>
    public static bool XoaSanPham(string maSP)
    {
        // 1. Tìm sản phẩm trong dsTheoMa (O(1))
        if (!dsTheoMa.TryGetValue(maSP, out SanPham spCanXoa))
        {
            return false; // Không tìm thấy sản phẩm
        }

        // 2. Xóa khỏi dsTheoMa (O(1))
        dsTheoMa.Remove(maSP);

        // 3. Cập nhật index dsTheoTen (O(1) trung bình)
        string tenKey = spCanXoa.TenSP.ToLowerInvariant();
        if (dsTheoTen.ContainsKey(tenKey))
        {
            // Xóa MaSP khỏi danh sách của tên đó
            dsTheoTen[tenKey].Remove(maSP);
            
            // Nếu danh sách đó rỗng (không còn SP nào có tên đó)
            if (dsTheoTen[tenKey].Count == 0)
            {
                // Xóa luôn key (tên) khỏi index
                dsTheoTen.Remove(tenKey);
            }
        }

        return true;
    }

    /// <summary>
    /// Cập nhật thông tin một sản phẩm (phức tạp nhất).
    /// (Yêu cầu 2.1 - Cập nhật)
    /// </summary>
    /// <returns>True nếu thành công, false nếu không tìm thấy mã SP.</returns>
    public static bool SuaSanPham(SanPham spDaSua)
    {
        // 1. Tìm sản phẩm cũ trong dsTheoMa (O(1))
        if (!dsTheoMa.TryGetValue(spDaSua.MaSP, out SanPham spCu))
        {
            return false; // Không tìm thấy sản phẩm để sửa
        }

        // 2. Cập nhật dsTheoMa (O(1))
        dsTheoMa[spDaSua.MaSP] = spDaSua;

        // 3. Kiểm tra xem Tên có bị thay đổi không
        string tenCuKey = spCu.TenSP.ToLowerInvariant();
        string tenMoiKey = spDaSua.TenSP.ToLowerInvariant();

        if (tenCuKey != tenMoiKey)
        {
            // Nếu tên thay đổi, phải cập nhật index dsTheoTen

            // 3.1. Xóa MaSP khỏi index Tên Cũ
            if (dsTheoTen.ContainsKey(tenCuKey))
            {
                dsTheoTen[tenCuKey].Remove(spDaSua.MaSP);
                if (dsTheoTen[tenCuKey].Count == 0)
                {
                    dsTheoTen.Remove(tenCuKey);
                }
            }

            // 3.2. Thêm MaSP vào index Tên Mới
            if (!dsTheoTen.ContainsKey(tenMoiKey))
            {
                dsTheoTen.Add(tenMoiKey, new List<string>());
            }
            dsTheoTen[tenMoiKey].Add(spDaSua.MaSP);
        }

        return true;
    }

    /// <summary>
    /// Tìm kiếm sản phẩm theo Mã SP (Rất nhanh).
    /// (Yêu cầu 2.1 - Tìm kiếm theo Mã)
    /// </summary>
    /// <param name="timThay">out: Sản phẩm tìm được</param>
    /// <returns>True nếu tìm thấy.</returns>
    public static bool TimTheoMa(string maSP, out SanPham timThay)
    {
        // Dùng O(1) của Dictionary
        return dsTheoMa.TryGetValue(maSP, out timThay);
    }

    /// <summary>
    /// Tìm kiếm sản phẩm theo Tên SP (Rất nhanh).
    /// (Yêu cầu 2.1 - Tìm kiếm theo Tên)
    /// </summary>
    /// <returns>Danh sách các sản phẩm (có thể rỗng) khớp tên.</returns>
    public static List<SanPham> TimTheoTen(string tenSP)
    {
        List<SanPham> ketQua = new List<SanPham>();
        string tenKey = tenSP.ToLowerInvariant();

        // 1. Tìm danh sách các MaSP khớp tên (O(1) trung bình)
        if (dsTheoTen.TryGetValue(tenKey, out List<string> danhSachMaSP))
        {
            // 2. Duyệt qua danh sách MaSP và lấy thông tin đầy đủ
            foreach (string maSP in danhSachMaSP)
            {
                // Lấy thông tin từ dsTheoMa (O(1))
                if (dsTheoMa.TryGetValue(maSP, out SanPham sp))
                {
                    ketQua.Add(sp);
                }
            }
        }
        return ketQua;
    }

    /* --- GIAO DIỆN CONSOLE CHO MODULE NÀY --- */

    /// <summary>
    /// Hiển thị Menu con cho chức năng Quản lý Sản phẩm.
    /// (Đây là điểm vào (entrypoint) từ Program.cs)
    /// (ĐÃ CẬP NHẬT - KHÔNG CÒN LÀ STUB)
    /// </summary>
    public static void HienThiMenu()
    {
        bool dangChay = true;
        var cacLuaChon = new List<string>
        {
            "1. Xem toàn bộ sản phẩm (Phân trang)",
            "2. Thêm sản phẩm mới",
            "3. Tìm kiếm sản phẩm (Theo Mã hoặc Tên)",
            "4. Sửa thông tin sản phẩm",
            "5. Xóa sản phẩm",
            "0. Quay lại Menu chính"
        };

        while (dangChay)
        {
            int luaChonIndex = ConsoleUI.HienThiMenuChon("== MODULE QUẢN LÝ SẢN PHẨM ==", cacLuaChon);

            switch (luaChonIndex)
            {
                case 0: // 1. Xem
                    GiaoDienXemTatCaSanPham();
                    break;
                case 1: // 2. Thêm
                    GiaoDienThemSanPham();
                    break;
                case 2: // 3. Tìm
                    GiaoDienTimSanPham();
                    break;
                case 3: // 4. Sửa
                    GiaoDienSuaSanPham();
                    break;
                case 4: // 5. Xóa
                    GiaoDienXoaSanPham();
                    break;
                case 5: // 0. Quay lại
                    dangChay = false;
                    break;
                case -1: // Esc
                    dangChay = false;
                    break;
            }
        }
    }

    /// <summary>
    /// (Yêu cầu 2.1) - Giao diện cho chức năng Thêm.
    /// </summary>
    private static void GiaoDienThemSanPham()
    {
        Console.Clear();
        Console.WriteLine("== THÊM SẢN PHẨM MỚI (Yêu cầu 2.1) ==");
        Console.WriteLine("Nhập thông tin. Nhấn Esc để hủy bất kỳ lúc nào.");
        
        // Vòng lặp để đảm bảo Mã SP là duy nhất
        string? maSP;
        while (true)
        {
            maSP = ConsoleUI.DocChuoi("1. Mã SP (VD: SP001): ", "", true);
            if (maSP == null) return; // Người dùng hủy

            if (dsTheoMa.ContainsKey(maSP))
            {
                ConsoleUI.HienThiThongBao($"Lỗi: Mã SP '{maSP}' đã tồn tại. Vui lòng nhập mã khác.", ConsoleColor.Red);
            }
            else
            {
                break;
            }
        }

        string? tenSP = ConsoleUI.DocChuoi("2. Tên Sản phẩm: ", "", true);
        if (tenSP == null) return;
        
        string? dvt = ConsoleUI.DocChuoi("3. Đơn vị tính (VD: Cái, Hộp, Kg): ", "", true);
        if (dvt == null) return;

        decimal? giaBan = ConsoleUI.DocSoThapPhan("4. Giá bán (VNĐ): ");
        if (giaBan == null) return;

        int? soLuong = ConsoleUI.DocSoNguyen("5. Số lượng tồn kho: ");
        if (soLuong == null) return;

        // Xác nhận
        Console.WriteLine("\n--- XÁC NHẬN THÔNG TIN ---");
        Console.WriteLine($"Mã: {maSP}, Tên: {tenSP}, DVT: {dvt}, Giá: {giaBan.Value:N0}, Tồn: {soLuong.Value}");
        
        if (ConsoleUI.XacNhan("Bạn có chắc chắn muốn thêm sản phẩm này?"))
        {
            SanPham spMoi = new SanPham(maSP, tenSP, dvt, giaBan.Value, soLuong.Value);
            bool thanhCong = ThemSanPham(spMoi);

            if (thanhCong)
            {
                ConsoleUI.HienThiThongBao("Thêm sản phẩm thành công!", ConsoleColor.Green);
            }
            else
            {
                // Trường hợp (hiếm) bị xung đột (race condition) nếu có đa luồng
                ConsoleUI.HienThiThongBao("Lỗi: Mã SP có thể đã tồn tại.", ConsoleColor.Red);
            }
        }
        else
        {
            ConsoleUI.HienThiThongBao("Đã hủy thao tác thêm.", ConsoleColor.Yellow);
        }
    }

    /// <summary>
    /// (Yêu cầu 2.1) - Giao diện cho chức năng Tìm kiếm.
    /// </summary>
    private static void GiaoDienTimSanPham()
    {
        Console.Clear();
        Console.WriteLine("== TÌM KIẾM SẢN PHẨM (Yêu cầu 2.1) ==");
        var luaChonTimKiem = new List<string> { "1. Tìm theo Mã SP (O(1))", "2. Tìm theo Tên SP (O(1))", "0. Quay lại" };
        int luaChon = ConsoleUI.HienThiMenuChon("Chọn phương thức tìm kiếm:", luaChonTimKiem);

        if (luaChon == 0) // Tìm theo Mã
        {
            string? maSP = ConsoleUI.DocChuoi("Nhập Mã SP cần tìm: ", "", true);
            if (maSP == null) return;

            if (TimTheoMa(maSP, out SanPham sp)) // Dùng hàm O(1)
            {
                Console.WriteLine("\n--- KẾT QUẢ TÌM THẤY ---");
                HienThiChiTietSanPham(sp);
            }
            else
            {
                ConsoleUI.HienThiThongBao("Không tìm thấy sản phẩm.", ConsoleColor.Red);
            }
        }
        else if (luaChon == 1) // Tìm theo Tên
        {
            string? tenSP = ConsoleUI.DocChuoi("Nhập Tên SP cần tìm: ", "", true);
            if (tenSP == null) return;

            List<SanPham> ketQua = TimTheoTen(tenSP); // Dùng hàm O(1)
            if (ketQua.Count > 0)
            {
                Console.WriteLine($"\n--- TÌM THẤY {ketQua.Count} KẾT QUẢ ---");
                foreach (var sp in ketQua)
                {
                    HienThiChiTietSanPham(sp);
                    Console.WriteLine("---");
                }
            }
            else
            {
                ConsoleUI.HienThiThongBao("Không tìm thấy sản phẩm nào có tên này.", ConsoleColor.Red);
            }
        }
        
        if(luaChon == 0 || luaChon == 1)
        {
             Console.WriteLine("Nhấn phím bất kỳ để tiếp tục...");
             Console.ReadKey(true);
        }
    }
    
    /// <summary>
    /// (Yêu cầu 2.1) - Giao diện cho chức năng Sửa.
    /// </summary>
    private static void GiaoDienSuaSanPham()
    {
        Console.Clear();
        Console.WriteLine("== SỬA SẢN PHẨM (Yêu cầu 2.1) ==");
        string? maSP = ConsoleUI.DocChuoi("Nhập Mã SP cần sửa (hoặc Esc để hủy): ", "", true);
        if (maSP == null) return;

        if (!TimTheoMa(maSP, out SanPham spCu))
        {
            ConsoleUI.HienThiThongBao("Không tìm thấy sản phẩm có mã này.", ConsoleColor.Red);
            return;
        }

        Console.WriteLine("\n--- TÌM THẤY SẢN PHẨM ---");
        HienThiChiTietSanPham(spCu);
        Console.WriteLine("\nNhập thông tin mới (Nhấn Enter để giữ nguyên giá trị cũ, Esc để hủy):");

        string? tenMoi = ConsoleUI.DocChuoi($"1. Tên Sản phẩm ({spCu.TenSP}): ", spCu.TenSP, false);
        if (tenMoi == null) return;

        string? dvtMoi = ConsoleUI.DocChuoi($"2. Đơn vị tính ({spCu.DonViTinh}): ", spCu.DonViTinh, false);
        if (dvtMoi == null) return;

        decimal? giaMoi = ConsoleUI.DocSoThapPhan($"3. Giá bán ({spCu.GiaBan:N0}): ", spCu.GiaBan);
        if (giaMoi == null) return;

        int? slMoi = ConsoleUI.DocSoNguyen($"4. Số lượng tồn ({spCu.SoLuongTonKho}): ", spCu.SoLuongTonKho);
        if (slMoi == null) return;

        if (ConsoleUI.XacNhan("Bạn có chắc chắn muốn cập nhật?"))
        {
            SanPham spMoi = new SanPham(maSP, tenMoi, dvtMoi, giaMoi.Value, slMoi.Value);
            if (SuaSanPham(spMoi))
            {
                ConsoleUI.HienThiThongBao("Cập nhật thành công!", ConsoleColor.Green);
            }
            else
            {
                ConsoleUI.HienThiThongBao("Lỗi: Không thể cập nhật.", ConsoleColor.Red);
            }
        }
        else
        {
            ConsoleUI.HienThiThongBao("Đã hủy thao tác sửa.", ConsoleColor.Yellow);
        }
    }

    /// <summary>
    /// (Yêu cầu 2.1) - Giao diện cho chức năng Xóa.
    /// </summary>
    private static void GiaoDienXoaSanPham()
    {
        Console.Clear();
        Console.WriteLine("== XÓA SẢN PHẨM (Yêu cầu 2.1) ==");
        string? maSP = ConsoleUI.DocChuoi("Nhập Mã SP cần xóa (hoặc Esc để hủy): ", "", true);
        if (maSP == null) return;

        if (!TimTheoMa(maSP, out SanPham spCanXoa))
        {
            ConsoleUI.HienThiThongBao("Không tìm thấy sản phẩm có mã này.", ConsoleColor.Red);
            return;
        }
        
        Console.WriteLine("\n--- TÌM THẤY SẢN PHẨM ---");
        HienThiChiTietSanPham(spCanXoa);

        if (ConsoleUI.XacNhan("CẢNH BÁO: Bạn có chắc chắn muốn XÓA vĩnh viễn sản phẩm này?"))
        {
            if (XoaSanPham(maSP))
            {
                ConsoleUI.HienThiThongBao("Đã xóa sản phẩm thành công!", ConsoleColor.Green);
            }
            else
            {
                ConsoleUI.HienThiThongBao("Lỗi: Không thể xóa.", ConsoleColor.Red);
            }
        }
        else
        {
            ConsoleUI.HienThiThongBao("Đã hủy thao tác xóa.", ConsoleColor.Yellow);
        }
    }

    /// <summary>
    /// (Yêu cầu 2.1) - Giao diện cho chức năng Xem (Phân trang).
    /// </summary>
    private static void GiaoDienXemTatCaSanPham()
    {
        Console.Clear();
        Console.WriteLine("== XEM TẤT CẢ SẢN PHẨM (Phân trang) ==");

        // Chuyển Dictionary (O(N)) thành List để sắp xếp và phân trang
        List<SanPham> danhSach = dsTheoMa.Values.OrderBy(sp => sp.MaSP).ToList();
        
        if (danhSach.Count == 0)
        {
            ConsoleUI.HienThiThongBao("Chưa có sản phẩm nào trong hệ thống.", ConsoleColor.Yellow);
            return;
        }

        int trangHienTai = 0;
        int soMucTrenTrang = 10;
        int tongSoTrang = (int)Math.Ceiling((double)danhSach.Count / soMucTrenTrang);

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"== XEM TẤT CẢ SẢN PHẨM (Trang {trangHienTai + 1}/{tongSoTrang}) ==");
            Console.WriteLine("-----------------------------------------------------------------");
            // Định dạng tiêu đề cột
            Console.WriteLine($"{"Mã SP", -10} | {"Tên Sản Phẩm", -30} | {"Giá Bán (VNĐ)", 15} | {"Tồn Kho", 10}");
            Console.WriteLine(new string('-', 70));

            // Lấy các mục cho trang hiện tại
            var cacMucCuaTrang = danhSach
                .Skip(trangHienTai * soMucTrenTrang)
                .Take(soMucTrenTrang);

            foreach (var sp in cacMucCuaTrang)
            {
                Console.WriteLine($"{sp.MaSP, -10} | {sp.TenSP, -30} | {sp.GiaBan, 15:N0} | {sp.SoLuongTonKho, 10} {sp.DonViTinh}");
            }
            
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("Sử dụng phím Trái/Phải để chuyển trang. Esc để thoát.");

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.RightArrow)
            {
                if (trangHienTai < tongSoTrang - 1) trangHienTai++;
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (trangHienTai > 0) trangHienTai--;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                break; // Thoát vòng lặp
            }
        }
    }

    /// <summary>
    /// Hàm trợ giúp in chi tiết 1 sản phẩm.
    /// </summary>
    private static void HienThiChiTietSanPham(SanPham sp)
    {
        Console.WriteLine($"Mã SP:       {sp.MaSP}");
        Console.WriteLine($"Tên SP:      {sp.TenSP}");
        Console.WriteLine($"Đơn vị tính: {sp.DonViTinh}");
        Console.WriteLine($"Giá bán:     {sp.GiaBan:N0} VNĐ");
        Console.WriteLine($"Tồn kho:     {sp.SoLuongTonKho}");
    }
}