using System;
using System.Collections.Generic;
using System.Linq;

public static class QuanLyBanHang
{
    // --- CẤU TRÚC DỮ LIỆU ---
    
    // 1. Struct Giao Dịch (Hóa đơn) để lưu vào Linked List
    public struct GiaoDich
    {
        public string TenKhach;
        public DateTime ThoiGian;
        public decimal TongTien;
        public List<SanPham> SanPhamDaMua; // Lưu để còn hoàn tác (trả kho)
    }

    // 2. Priority Queue: Hàng đợi ưu tiên (Khách hàng, Độ ưu tiên)
    // Độ ưu tiên càng nhỏ càng được phục vụ trước (1: VIP, 2: Thường)
    private static PriorityQueue<string, int> hangDoiThanhToan = new PriorityQueue<string, int>();

    // 3. Linked List: Lịch sử giao dịch (Dùng để Undo)
    private static LinkedList<GiaoDich> lichSuGiaoDich = new LinkedList<GiaoDich>();

    // Giỏ hàng tạm (Queue thường) cho khách đang được phục vụ
    private static Queue<SanPham> gioHangHienTai = new Queue<SanPham>();


    // --- MENU CHÍNH ---
    public static void HienThiMenu()
    {
        bool dangChay = true;
        while (dangChay)
        {
            Console.Clear();
            // Hiển thị trạng thái hàng đợi
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"HÀNG ĐỢI: {hangDoiThanhToan.Count} khách đang chờ.");
            Console.WriteLine($"LỊCH SỬ:  {lichSuGiaoDich.Count} hóa đơn đã lưu.");
            Console.ResetColor();
            Console.WriteLine("---------------------------------------------");

            var cacLuaChon = new List<string>
            {
                "1. Xếp hàng thanh toán (Thêm khách VIP/Thường)",
                "2. Gọi khách tiếp theo (Xử lý Priority Queue)",
                "3. Xem Lịch sử Giao dịch (Duyệt Linked List)",
                "4. Hoàn tác giao dịch cuối (Undo - RemoveLast)",
                "0. Quay lại Menu chính"
            };

            int luaChon = ConsoleUI.HienThiMenuChon("== QUẦY THU NGÂN NÂNG CAO ==", cacLuaChon);
            switch (luaChon)
            {
                case 0: ThemKhachVaoHang(); break;
                case 1: XuLyKhachHang(); break;
                case 2: XemLichSu(); break;
                case 3: HoanTacGiaoDich(); break;
                case 4: dangChay = false; break;
                case -1: dangChay = false; break;
            }
        }
    }

    // --- TÍNH NĂNG 1: PRIORITY QUEUE (XẾP HÀNG) ---
    private static void ThemKhachVaoHang()
    {
        Console.Clear();
        Console.WriteLine("== XẾP HÀNG THANH TOÁN ==");
        
        string? tenKhach = ConsoleUI.DocChuoi("Nhập tên khách hàng: ", "", true);
        if (tenKhach == null) return;

        Console.WriteLine("\nChọn loại khách hàng:");
        Console.WriteLine("1. Khách VIP (Ưu tiên 1 - Được phục vụ trước)");
        Console.WriteLine("2. Khách Thường (Ưu tiên 2)");
        
        int? loai = ConsoleUI.DocSoNguyen("Lựa chọn (1-2): ");
        int doUuTien = (loai == 1) ? 1 : 2;

        // Enqueue vào Priority Queue
        hangDoiThanhToan.Enqueue(tenKhach, doUuTien);

        string loaiKhach = (doUuTien == 1) ? "VIP" : "Thường";
        ConsoleUI.HienThiThongBao($"Đã thêm khách '{tenKhach}' ({loaiKhach}) vào hàng đợi.", ConsoleColor.Green);
    }

    private static void XuLyKhachHang()
    {
        Console.Clear();
        if (hangDoiThanhToan.Count == 0)
        {
            ConsoleUI.HienThiThongBao("Không có ai đang xếp hàng cả.", ConsoleColor.Red);
            return;
        }

        // Dequeue: Lấy khách có độ ưu tiên cao nhất ra
        if (hangDoiThanhToan.TryDequeue(out string? tenKhach, out int doUuTien))
        {
            string danhHieu = (doUuTien == 1) ? "[VIP]" : "[Thường]";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nĐANG PHỤC VỤ: {danhHieu} {tenKhach}");
            Console.ResetColor();

            // Tự động tạo giỏ hàng ngẫu nhiên cho khách này (để demo cho nhanh)
            TaoGioHangNgauNhien();

            // Thực hiện thanh toán
            ThucHienThanhToan(tenKhach);
        }
    }

    // --- TÍNH NĂNG 2: LINKED LIST (LỊCH SỬ & UNDO) ---
    private static void ThucHienThanhToan(string tenKhach)
    {
        decimal tongTien = 0;
        var dsMua = new List<SanPham>(); // Lưu lại danh sách để đưa vào lịch sử

        Console.WriteLine("\n--- HÓA ĐƠN ---");
        while (gioHangHienTai.Count > 0)
        {
            SanPham sp = gioHangHienTai.Dequeue();
            
            // Trừ tồn kho
            if (QuanLySanPham.dsTheoMa.TryGetValue(sp.MaSP, out SanPham spKho))
            {
                if (spKho.SoLuongTonKho > 0)
                {
                    spKho.SoLuongTonKho--;
                    QuanLySanPham.dsTheoMa[sp.MaSP] = spKho;

                    tongTien += spKho.GiaBan;
                    dsMua.Add(spKho); // Thêm vào list đã mua
                    Console.WriteLine($"- {spKho.TenSP}: {spKho.GiaBan:N0}");
                }
            }
        }
        Console.WriteLine("---------------");
        Console.WriteLine($"TỔNG TIỀN: {tongTien:N0} VNĐ");

        // Lưu vào Linked List (Thêm vào cuối)
        var giaoDichMoi = new GiaoDich 
        { 
            TenKhach = tenKhach, 
            ThoiGian = DateTime.Now, 
            TongTien = tongTien,
            SanPhamDaMua = dsMua
        };
        lichSuGiaoDich.AddLast(giaoDichMoi);

        ConsoleUI.HienThiThongBao("Thanh toán xong! Đã lưu vào Lịch sử.", ConsoleColor.Green);
    }

    private static void XemLichSu()
    {
        Console.Clear();
        Console.WriteLine($"== LỊCH SỬ GIAO DỊCH (Linked List) - Tổng: {lichSuGiaoDich.Count} ==\n");
        
        if (lichSuGiaoDich.Count == 0) Console.WriteLine("[Trống]");

        // Duyệt danh sách liên kết (O(N))
        // Duyệt ngược từ cuối lên đầu (để thấy cái mới nhất)
        var node = lichSuGiaoDich.Last;
        int i = 1;
        while (node != null)
        {
            GiaoDich gd = node.Value;
            Console.WriteLine($"#{i++} [{gd.ThoiGian:HH:mm:ss}] {gd.TenKhach} - {gd.TongTien:N0} VNĐ ({gd.SanPhamDaMua.Count} món)");
            node = node.Previous;
        }
        Console.ReadKey(true);
    }

    private static void HoanTacGiaoDich()
    {
        Console.Clear();
        Console.WriteLine("== HOÀN TÁC GIAO DỊCH (UNDO) ==");

        if (lichSuGiaoDich.Count == 0)
        {
            ConsoleUI.HienThiThongBao("Không có giao dịch nào để hoàn tác.", ConsoleColor.Red);
            return;
        }

        // Lấy giao dịch cuối cùng (O(1))
        GiaoDich gdCuoi = lichSuGiaoDich.Last.Value;

        Console.WriteLine($"Giao dịch gần nhất: Khách {gdCuoi.TenKhach} - {gdCuoi.TongTien:N0} VNĐ");
        if (ConsoleUI.XacNhan("Bạn có chắc chắn muốn hủy giao dịch này và hoàn kho?"))
        {
            // 1. Hoàn trả số lượng tồn kho
            foreach (var sp in gdCuoi.SanPhamDaMua)
            {
                if (QuanLySanPham.dsTheoMa.TryGetValue(sp.MaSP, out SanPham spKho))
                {
                    spKho.SoLuongTonKho++;
                    QuanLySanPham.dsTheoMa[sp.MaSP] = spKho;
                }
            }

            // 2. Xóa khỏi Linked List (O(1))
            lichSuGiaoDich.RemoveLast();

            ConsoleUI.HienThiThongBao("Đã hoàn tác thành công!", ConsoleColor.Green);
        }
    }

    // --- HÀM HỖ TRỢ ---
    private static void TaoGioHangNgauNhien()
    {
        gioHangHienTai.Clear();
        if (QuanLySanPham.dsTheoMa.Count == 0) return;

        Random rnd = new Random();
        var keys = QuanLySanPham.dsTheoMa.Keys.ToList();
        // Mua 3 món ngẫu nhiên
        for (int i = 0; i < 3; i++)
        {
            string key = keys[rnd.Next(keys.Count)];
            gioHangHienTai.Enqueue(QuanLySanPham.dsTheoMa[key]);
        }
    }
    
    // Hàm hỗ trợ Module Kệ Hàng gọi sang (để tương thích code cũ)
    public static void ThemVaoGioTuDong(SanPham sp)
    {
        gioHangHienTai.Enqueue(sp);
        ConsoleUI.HienThiThongBao($"Đã thêm '{sp.TenSP}' vào giỏ.", ConsoleColor.Green);
    }
}