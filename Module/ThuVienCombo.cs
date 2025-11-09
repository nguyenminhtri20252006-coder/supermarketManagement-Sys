using System; // Cần cho Console
using System.Collections.Generic;
using System.Linq; // Cần cho .Values.ToList()

/// <summary>
/// Module (static class) chứa các thuật toán liên quan đến
/// Yêu cầu 2.2 (Liệt kê Combo Tết).
/// </summary>
public static class ThuVienCombo
{
    /// <summary>
    /// Hàm chính (public) để gọi thuật toán liệt kê tổ hợp chập m của n.
    /// (Yêu cầu 2.2)
    /// </summary>
    /// <param name="danhSachGoc">Danh sách (n) sản phẩm đầu vào.</param>
    /// <param name="m">Số lượng sản phẩm (m) cho mỗi combo (m <= n).</param>
    /// <returns>Một danh sách chứa tất cả các combo có thể có.</returns>
    public static List<List<SanPham>> LietKeCombos(List<SanPham> danhSachGoc, int m)
    {
        var tatCaCombos = new List<List<SanPham>>();
        
        // Gọi hàm đệ quy (helper)
        TimComboTiepTheo(danhSachGoc, m, 0, new List<SanPham>(), tatCaCombos);
        
        return tatCaCombos;
    }

    /// <summary>
    /// Hàm đệ quy/quay lui (backtracking) để tìm các tổ hợp.
    /// Đây là thuật toán cốt lõi của Yêu cầu 2.2.
    /// </summary>
    /// <param name="danhSachGoc">Danh sách (n) sản phẩm gốc.</param>
    /// <param name="m">Số lượng (m) cần chọn.</param>
    /// <param name="viTriBatDau">Index bắt đầu duyệt trong danhSachGoc (để tránh lặp lại).</param>
    /// <param name="comboHienTai">Combo đang được xây dựng trong quá trình đệ quy.</param>
    /// <param name="ketQuaCuoiCung">Danh sách (output) chứa tất cả các combo hoàn chỉnh.</param>
    private static void TimComboTiepTheo(
        List<SanPham> danhSachGoc, 
        int m, 
        int viTriBatDau, 
        List<SanPham> comboHienTai, 
        List<List<SanPham>> ketQuaCuoiCung)
    {
        // 1. Điều kiện dừng (Base case):
        // Khi comboHienTai đã đủ m sản phẩm.
        if (comboHienTai.Count == m)
        {
            // Thêm một *bản sao* của comboHienTai vào kết quả.
            // Phải tạo 'new List' nếu không nó sẽ bị thay đổi bởi các lệnh gọi đệ quy sau.
            ketQuaCuoiCung.Add(new List<SanPham>(comboHienTai));
            return; // Dừng nhánh đệ quy này.
        }

        // 2. Bước đệ quy (Recursive step):
        // Duyệt qua các sản phẩm còn lại
        for (int i = viTriBatDau; i < danhSachGoc.Count; i++)
        {
            // Lấy sản phẩm tại vị trí i
            SanPham sp = danhSachGoc[i];

            // 2.1. Thêm (Choose)
            // Thêm sản phẩm này vào combo hiện tại.
            comboHienTai.Add(sp);

            // 2.2. Khám phá (Explore)
            // Gọi đệ quy để tìm sản phẩm tiếp theo.
            // Quan trọng: viTriBatDau là 'i + 1' để đảm bảo không chọn lại
            // sản phẩm đã chọn và tránh hoán vị (ví dụ: (A,B) và (B,A)).
            TimComboTiepTheo(danhSachGoc, m, i + 1, comboHienTai, ketQuaCuoiCung);

            // 2.3. Bỏ chọn (Unchoose) - Quay lui (Backtrack)
            // Sau khi nhánh đệ quy (i+1) kết thúc, ta gỡ sản phẩm
            // cuối cùng ra khỏi combo để thử sản phẩm tiếp theo trong vòng 'for'.
            comboHienTai.RemoveAt(comboHienTai.Count - 1);
        }
    }

    /* --- GIAO DIỆN CONSOLE CHO MODULE NÀY --- */

    /// <summary>
    /// Hiển thị Menu con cho chức năng Liệt kê Combo.
    /// (Đây là điểm vào (entrypoint) từ Program.cs)
    /// (ĐÃ CẬP NHẬT - KHÔNG CÒN LÀ STUB)
    /// </summary>
    public static void HienThiMenu()
    {
        // (Đây là stub - Sẽ triển khai đầy đủ ở bước sau)
        Console.Clear();
        Console.WriteLine("== THUẬT TOÁN LIỆT KÊ COMBO TẾT (Yêu cầu 2.2) ==");
        Console.WriteLine("Thuật toán này sử dụng Đệ quy - Quay lui (Backtracking) để tìm tổ hợp chập m của n.");
        
        int tongSoSP = QuanLySanPham.dsTheoMa.Count;
        if (tongSoSP == 0)
        {
            ConsoleUI.HienThiThongBao("Không có sản phẩm nào trong CSDL để tạo combo.", ConsoleColor.Red);
            return;
        }

        // 1. Nhập N (Số sản phẩm trong nhóm)
        int? n = ConsoleUI.DocSoNguyen($"1. Nhập tổng số sản phẩm trong nhóm (n) (Tối đa {tongSoSP}): ");
        if (n == null || n <= 0 || n > tongSoSP)
        {
            ConsoleUI.HienThiThongBao(n == null ? "Đã hủy." : "Số lượng không hợp lệ.", ConsoleColor.Yellow);
            return;
        }

        // 2. Nhập M (Số sản phẩm mỗi combo)
        int? m = ConsoleUI.DocSoNguyen($"2. Nhập số sản phẩm mỗi combo (m) (m <= {n.Value}): ");
        if (m == null || m <= 0 || m > n.Value)
        {
            ConsoleUI.HienThiThongBao(m == null ? "Đã hủy." : "Số lượng không hợp lệ.", ConsoleColor.Yellow);
            return;
        }

        // 3. Chọn N sản phẩm
        Console.Clear();
        var danhSachGoc = QuanLySanPham.dsTheoMa.Values.ToList();
        
        // Gọi hàm UI chọn nhiều mục (Bổ sung ở ConsoleUI.cs)
        var danhSachDaChon = ConsoleUI.HienThiMenuChonNhieuMuc(
            $"Vui lòng chọn {n.Value} sản phẩm cho nhóm (n):",
            danhSachGoc,
            n.Value,
            (sp) => $"{sp.MaSP} - {sp.TenSP} (Giá: {sp.GiaBan:N0})" // Cách hiển thị
        );

        if (danhSachDaChon == null)
        {
            ConsoleUI.HienThiThongBao("Đã hủy thao tác chọn sản phẩm.", ConsoleColor.Yellow);
            return;
        }

        // 4. Gọi thuật toán (Yêu cầu 2.2)
        Console.WriteLine("\nĐang tính toán các combo... Vui lòng chờ...");
        var tatCaCombos = LietKeCombos(danhSachDaChon, m.Value);

        // 5. In kết quả (Phân trang)
        InKetQuaCombos(tatCaCombos, n.Value, m.Value);
    }
    
    /// <summary>
    /// Hàm trợ giúp in kết quả combo ra màn hình (có phân trang).
    /// </summary>
    private static void InKetQuaCombos(List<List<SanPham>> tatCaCombos, int n, int m)
    {
        if (tatCaCombos.Count == 0)
        {
            ConsoleUI.HienThiThongBao("Không thể tạo combo nào từ các lựa chọn này.", ConsoleColor.Yellow);
            return;
        }

        int trangHienTai = 0;
        int soMucTrenTrang = 5; // 5 combo mỗi trang
        int tongSoTrang = (int)Math.Ceiling((double)tatCaCombos.Count / soMucTrenTrang);

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"== KẾT QUẢ LIỆT KÊ COMBO (n={n}, m={m}) ==");
            Console.WriteLine($"Tìm thấy tổng cộng: {tatCaCombos.Count} combos.");
            Console.WriteLine($"Hiển thị trang {trangHienTai + 1}/{tongSoTrang}:");
            Console.ResetColor();
            Console.WriteLine("---------------------------------------------");

            // Lấy các mục cho trang hiện tại
            var cacMucCuaTrang = tatCaCombos
                .Skip(trangHienTai * soMucTrenTrang)
                .Take(soMucTrenTrang);

            int comboSo = (trangHienTai * soMucTrenTrang) + 1;
            foreach (var combo in cacMucCuaTrang)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nCOMBO #{comboSo++}:");
                Console.ResetColor();
                
                decimal tongGia = 0;
                foreach (var sp in combo)
                {
                    Console.WriteLine($"  + {sp.MaSP}: {sp.TenSP} (Giá: {sp.GiaBan:N0})");
                    tongGia += sp.GiaBan;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  => TỔNG GIÁ COMBO: {tongGia:N0} VNĐ");
                Console.ResetColor();
            }

            Console.WriteLine("\n---------------------------------------------");
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
}