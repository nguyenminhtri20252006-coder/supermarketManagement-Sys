using System;
using System.Collections.Generic;
using System.Linq;

public static class ThuVienCombo
{
    // Cấu trúc định nghĩa một Ngày Lễ (Preset)
    struct LeHoi
    {
        public string TenLeHoi;
        public int SoLuongMon; // m (Số món trong combo)
        public string MoTa;

        public LeHoi(string ten, int sl, string moTa)
        {
            TenLeHoi = ten;
            SoLuongMon = sl;
            MoTa = moTa;
        }
    }

    // Danh sách các ngày lễ ở Việt Nam và cấu hình Combo tương ứng
    private static readonly List<LeHoi> DsLeHoi = new List<LeHoi>
    {
        new LeHoi("Tết Nguyên Đán", 6, "Giỏ quà Tết Sum Vầy (6 món)"),
        new LeHoi("Lễ Tình Nhân (Valentine)", 2, "Cặp đôi Hoàn hảo (2 món xịn nhất)"),
        new LeHoi("Giáng Sinh (Noel)", 4, "Combo Đêm Đông (4 món)"),
        new LeHoi("Quốc Khánh (2/9)", 3, "Mừng Đại Lễ (3 món)"),
        new LeHoi("Ngày Phụ Nữ VN (20/10)", 5, "Quà tặng Phái Đẹp (5 món)"),
        new LeHoi("Rằm Trung Thu", 4, "Hộp quà Trăng Rằm (4 món)")
    };

    // --- THUẬT TOÁN ĐỆ QUY (BACKTRACKING) ---
    // Tìm tổ hợp chập m của n (Không quan tâm thứ tự)
    public static List<List<SanPham>> LietKeCombos(List<SanPham> danhSachGoc, int m)
    {
        var ketQua = new List<List<SanPham>>();
        TimComboTiepTheo(danhSachGoc, m, 0, new List<SanPham>(), ketQua);
        return ketQua;
    }

    private static void TimComboTiepTheo(List<SanPham> danhSachGoc, int m, int start, List<SanPham> current, List<List<SanPham>> result)
    {
        // Điều kiện dừng: Đã chọn đủ m món
        if (current.Count == m)
        {
            result.Add(new List<SanPham>(current));
            return;
        }

        // Duyệt các phần tử còn lại
        for (int i = start; i < danhSachGoc.Count; i++)
        {
            current.Add(danhSachGoc[i]); // Chọn
            
            // Đệ quy: Tìm món tiếp theo (bắt đầu từ i+1 để không trùng lặp)
            TimComboTiepTheo(danhSachGoc, m, i + 1, current, result);
            
            current.RemoveAt(current.Count - 1); // Quay lui (Bỏ chọn để thử món khác)
        }
    }

    // --- GIAO DIỆN MENU NÂNG CẤP ---
    public static void HienThiMenu()
    {
        bool dangChay = true;
        while (dangChay)
        {
            // Tạo danh sách menu động từ DsLeHoi
            var menuItems = new List<string>();
            for (int i = 0; i < DsLeHoi.Count; i++)
            {
                menuItems.Add($"{i + 1}. {DsLeHoi[i].TenLeHoi} - {DsLeHoi[i].MoTa}");
            }
            menuItems.Add("--------------------------");
            menuItems.Add($"{DsLeHoi.Count + 1}. Tự chọn thủ công (Nhập n, m)");
            menuItems.Add("0. Quay lại");

            int chon = ConsoleUI.HienThiMenuChon("== GỢI Ý COMBO QUÀ TẶNG THEO DỊP LỄ (NÂNG CẤP) ==", menuItems);

            // Xử lý lựa chọn
            if (chon >= 0 && chon < DsLeHoi.Count)
            {
                // Chọn một ngày lễ cụ thể
                ChayCheDoLeHoi(DsLeHoi[chon]);
            }
            else if (chon == DsLeHoi.Count + 1) // Mục Tự chọn (Sau vạch ngăn cách)
            {
                ChayThuCong();
            }
            else if (chon == -1 || chon == menuItems.Count - 1) // Quay lại
            {
                dangChay = false;
            }
        }
    }

    // --- XỬ LÝ LOGIC CHO NGÀY LỄ ---
    private static void ChayCheDoLeHoi(LeHoi leHoi)
    {
        Console.Clear();
        // Tự động lấy dữ liệu từ kho để Demo (Lấy 12 món đắt nhất để tính toán cho ra số to)
        var dsDauVao = QuanLySanPham.dsTheoMa.Values
                        .OrderByDescending(sp => sp.GiaBan) // Lấy hàng đắt tiền
                        .Take(12) // Lấy 12 món làm nguồn (n=12)
                        .ToList();
        
        if (dsDauVao.Count < leHoi.SoLuongMon) {
            ConsoleUI.HienThiThongBao($"Kho không đủ {leHoi.SoLuongMon} món để tạo combo này. Hãy tạo dữ liệu mẫu trước.", ConsoleColor.Red);
            return;
        }

        Console.WriteLine($"Đang tính toán các phương án gói quà cho: {leHoi.TenLeHoi}...");
        Console.WriteLine($"Yêu cầu: Chọn {leHoi.SoLuongMon} món từ {dsDauVao.Count} sản phẩm cao cấp nhất trong kho.");
        Console.WriteLine("(Sử dụng thuật toán Đệ quy Quay lui để liệt kê tất cả trường hợp...)");

        // 1. Chạy thuật toán
        var tatCaCombos = LietKeCombos(dsDauVao, leHoi.SoLuongMon);

        // 2. Tính toán giá trị và Sắp xếp (Tìm Combo Lời Nhất)
        // OrderByDescending: Giá cao nhất lên đầu
        var combosSapXep = tatCaCombos
            .OrderByDescending(combo => combo.Sum(sp => sp.GiaBan))
            .ToList();

        // 3. Hiển thị kết quả tối ưu
        HienThiKetQuaToiUu(combosSapXep, leHoi);
    }

    private static void HienThiKetQuaToiUu(List<List<SanPham>> dsCombo, LeHoi leHoi)
    {
        if (dsCombo.Count == 0) return;

        int trang = 0;
        int size = 5;
        int tongSoTrang = (int)Math.Ceiling((double)dsCombo.Count / size);

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"== KẾT QUẢ: {leHoi.TenLeHoi} (Tổng phương án: {dsCombo.Count}) ==");
            Console.ResetColor();
            
            // --- PHẦN QUAN TRỌNG: HIỂN THỊ COMBO LỜI NHẤT ---
            if (trang == 0)
            {
                var bestCombo = dsCombo[0];
                decimal maxPrice = bestCombo.Sum(sp => sp.GiaBan);

                Console.WriteLine("\n$$$ COMBO GIÁ TRỊ CAO NHẤT (ĐỀ XUẤT CHO NGƯỜI BÁN) $$$");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("**************************************************");
                Console.WriteLine($"* TỔNG GIÁ TRỊ: {maxPrice:N0} VNĐ");
                foreach (var sp in bestCombo)
                {
                    Console.WriteLine($"* + {sp.TenSP,-30} ({sp.GiaBan:N0})");
                }
                Console.WriteLine("**************************************************");
                Console.ResetColor();
                Console.WriteLine("\n--- Các phương án khác (Sắp xếp giảm dần) ---");
            }
            // --------------------------------------------------

            var page = dsCombo.Skip(trang * size).Take(size);
            int i = trang * size + 1;
            
            foreach (var c in page)
            {
                decimal tong = c.Sum(x => x.GiaBan);
                // Nếu là combo đầu tiên (best) thì đánh dấu khác chút
                string prefix = (trang == 0 && i == 1) ? "" : $"#{i}     "; 
                
                Console.WriteLine($"{prefix}Combo: {tong:N0} đ");
                Console.WriteLine($"       ({string.Join(", ", c.Select(x => x.TenSP))})");
                Console.WriteLine("-");
                i++;
            }

            Console.WriteLine($"\nTrang {trang + 1}/{tongSoTrang}. Trái/Phải: Chuyển trang | Esc: Thoát");
            var k = Console.ReadKey(true).Key;
            if (k == ConsoleKey.RightArrow && trang < tongSoTrang - 1) trang++;
            else if (k == ConsoleKey.LeftArrow && trang > 0) trang--;
            else if (k == ConsoleKey.Escape) break;
        }
    }

    private static void ChayThuCong()
    {
        Console.Clear();
        Console.WriteLine("== TẠO COMBO TÙY CHỌN (YÊU CẦU 2.2) ==");
        
        int tongKho = QuanLySanPham.dsTheoMa.Count;
        if (tongKho == 0) { ConsoleUI.HienThiThongBao("Kho rỗng.", ConsoleColor.Red); return; }

        // Bước 1: Chọn n (Số lượng sản phẩm đầu vào)
        int? n = ConsoleUI.DocSoNguyen($"1. Nhập số lượng sản phẩm muốn chọn (n) (Tối đa {tongKho}): ");
        if (n == null || n <= 0 || n > tongKho) return;

        // Bước 2: Chọn m (Số lượng trong 1 combo)
        int? m = ConsoleUI.DocSoNguyen($"2. Nhập số món trong mỗi combo (m) (m <= {n}): ");
        if (m == null || m <= 0 || m > n) return;

        List<SanPham> dsChon = new List<SanPham>();

        // --- CẢI TIẾN: HỎI NGƯỜI DÙNG CÓ MUỐN TỰ ĐỘNG KHÔNG ---
        bool tuDong = ConsoleUI.XacNhan($"Bạn có muốn hệ thống tự chọn ngẫu nhiên {n} sản phẩm từ kho không? (Chọn 'K' để chọn tay từng món)");

        if (tuDong)
        {
            // Tự động lấy n sản phẩm ngẫu nhiên
            Random rnd = new Random();
            var allProducts = QuanLySanPham.dsTheoMa.Values.ToList();
            
            // Shuffle (Tráo bài) và lấy n món
            dsChon = allProducts.OrderBy(x => rnd.Next()).Take(n.Value).ToList();
            
            Console.WriteLine($"\nĐã chọn ngẫu nhiên xong {dsChon.Count} sản phẩm.");
            Console.WriteLine("Nhấn phím bất kỳ để bắt đầu tính toán...");
            Console.ReadKey();
        }
        else
        {
            // Chọn tay (Như cũ)
            Console.Clear();
            var dsKho = QuanLySanPham.dsTheoMa.Values.ToList();
            dsChon = ConsoleUI.HienThiMenuChonNhieuMuc(
                $"Vui lòng chọn đúng {n} sản phẩm để làm nguyên liệu tạo Combo:",
                dsKho, n.Value, sp => $"{sp.MaSP} - {sp.TenSP}");
            
            if (dsChon == null) return;
        }
        // ------------------------------------------------------

        // Bước 4: Chạy thuật toán
        Console.WriteLine("\nĐang chạy thuật toán đệ quy...");
        var ketQua = LietKeCombos(dsChon, m.Value);
        InKetQuaCombos(ketQua);
    }

    // Hàm in kết quả chung
    private static void InKetQuaCombos(List<List<SanPham>> dsCombo)
    {
        if (dsCombo.Count == 0) { ConsoleUI.HienThiThongBao("Không tìm thấy combo nào.", ConsoleColor.Yellow); return; }

        int trang = 0;
        int size = 5;
        int tongTrang = (int)Math.Ceiling((double)dsCombo.Count / size);

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"KẾT QUẢ: Tìm thấy {dsCombo.Count} combo.");
            Console.WriteLine("--------------------------------------------------");

            var page = dsCombo.Skip(trang * size).Take(size);
            int i = trang * size + 1;

            foreach (var c in page)
            {
                decimal tong = c.Sum(x => x.GiaBan);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Combo #{i++}: ");
                Console.ResetColor();
                Console.WriteLine($"{tong:N0} VNĐ");
                Console.WriteLine($"   ({string.Join(", ", c.Select(x => x.TenSP))})");
                Console.WriteLine("-");
            }

            Console.WriteLine($"\nTrang {trang + 1}/{tongTrang}. [Trái/Phải] Chuyển trang, [Esc] Thoát.");
            var k = Console.ReadKey(true).Key;
            if (k == ConsoleKey.RightArrow && trang < tongTrang - 1) trang++;
            else if (k == ConsoleKey.LeftArrow && trang > 0) trang--;
            else if (k == ConsoleKey.Escape) break;
        }
    }
}