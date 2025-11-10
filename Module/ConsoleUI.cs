using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization; // Cần cho Parse decimal

/// <summary>
/// Module (static class) chịu trách nhiệm cung cấp các hàm
/// giao diện Console nâng cao (Key-Driven), hỗ trợ Tiếng Việt (Yêu cầu 6).
/// </summary>
public static class ConsoleUI
{
    /// <summary>
    /// Hiển thị một menu cho phép chọn bằng phím Lên/Xuống và Enter.
    /// </summary>
    /// <param name="tieuDe">Tiêu đề của menu (in màu vàng).</param>
    /// <param name="cacLuaChon">Danh sách các lựa chọn.</param>
    /// <returns>Index của mục được chọn (bắt đầu từ 0). Trả về -1 nếu nhấn Escape.</returns>
    public static int HienThiMenuChon(string tieuDe, List<string> cacLuaChon)
    {
        int mucChonHienTai = 0;
        ConsoleKeyInfo key;

        Console.CursorVisible = false; // Ẩn con trỏ

        while (true)
        {
            Console.Clear();
            
            // 1. In tiêu đề
            DatMau(ConsoleColor.Yellow);
            Console.WriteLine(tieuDe);
            Console.WriteLine("----------------------------------");
            Console.ResetColor();

            // 2. In các lựa chọn
            for (int i = 0; i < cacLuaChon.Count; i++)
            {
                if (i == mucChonHienTai)
                {
                    // Đánh dấu mục đang được chọn
                    DatMau(ConsoleColor.Black, ConsoleColor.Green);
                    Console.Write(">> ");
                    Console.WriteLine(cacLuaChon[i] + " <<");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {cacLuaChon[i]}   ");
                }
            }

            // 3. In hướng dẫn
            Console.WriteLine("\nSử dụng phím Lên/Xuống để di chuyển, Enter để chọn, Esc để thoát.");

            // 4. Đọc phím
            key = Console.ReadKey(true); // 'true' để không hiển thị phím nhấn
              // --- (BẮT ĐẦU CẬP NHẬT) Yêu cầu 2: Chọn bằng phím số ---
            // Kiểm tra xem người dùng có nhấn phím số (0-9) không
            if (char.IsDigit(key.KeyChar))
            {
                string soNhan = key.KeyChar.ToString();
                
                // Tìm index của mục đầu tiên bắt đầu bằng "soNhan." 
                // (ví dụ: "1." hoặc "0.")
                int indexTimThay = cacLuaChon.FindIndex(s => 
                    s.Trim().StartsWith(soNhan + ".")
                );

                if (indexTimThay != -1)
                {
                    // Nếu tìm thấy, chọn ngay lập tức (giống như Enter)
                    Console.CursorVisible = true; // Hiện lại con trỏ
                    return indexTimThay;
                }
            }

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    mucChonHienTai = (mucChonHienTai > 0) ? mucChonHienTai - 1 : cacLuaChon.Count - 1;
                    break;
                case ConsoleKey.DownArrow:
                    mucChonHienTai = (mucChonHienTai < cacLuaChon.Count - 1) ? mucChonHienTai + 1 : 0;
                    break;
                case ConsoleKey.Enter:
                    Console.CursorVisible = true; // Hiện lại con trỏ
                    return mucChonHienTai;
                case ConsoleKey.Escape:
                    Console.CursorVisible = true; // Hiện lại con trỏ
                    return -1; // Người dùng hủy
            }
        }
    }

    /// <summary>
    /// (BỔ SUNG MỚI)
    /// Hiển thị một menu cho phép chọn NHIỀU mục (tối đa 'soLuongCanChon' mục).
    /// Người dùng dùng Lên/Xuống để di chuyển, Phím cách (Spacebar) để Chọn/Bỏ chọn,
    /// và Enter để xác nhận khi đã chọn đủ.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của danh sách (phải implement ToString()).</typeparam>
    /// <param name="tieuDe">Tiêu đề của menu.</param>
    /// <param name="danhSachGoc">Toàn bộ danh sách để chọn.</param>
    /// <param name="soLuongCanChon">Số lượng (n) mục cần chọn.</param>
    /// <param name="hienThi">Một Func để tùy chỉnh cách hiển thị T (VD: sp => sp.TenSP).</param>
    /// <returns>Một List chứa các mục đã chọn. Trả về NULL nếu nhấn Escape.</returns>
    public static List<T>? HienThiMenuChonNhieuMuc<T>(string tieuDe, List<T> danhSachGoc, int soLuongCanChon, Func<T, string> hienThi)
    {
        int mucChonHienTai = 0;
        // Dùng HashSet để lưu các mục đã chọn (hiệu suất O(1) khi kiểm tra)
        HashSet<T> cacMucDaChon = new HashSet<T>();
        ConsoleKeyInfo key;

        Console.CursorVisible = false;

        while (true)
    {
            Console.Clear();
            
            // 1. In tiêu đề
            DatMau(ConsoleColor.Yellow);
            Console.WriteLine(tieuDe);
            Console.WriteLine($"(Đã chọn: {cacMucDaChon.Count}/{soLuongCanChon}) - Dùng phím Spacebar để chọn/bỏ chọn.");
            Console.WriteLine("---------------------------------------------------------------");
        Console.ResetColor();

            // 2. In các lựa chọn (Chỉ 15 mục một lúc để tránh tràn màn hình)
            int soMucToiDa = 15;
            int trangHienTai = mucChonHienTai / soMucToiDa;
            int batDau = trangHienTai * soMucToiDa;
            int ketThuc = Math.Min(batDau + soMucToiDa, danhSachGoc.Count);
            
            if (batDau > 0)
            {
                Console.WriteLine("  ... (cuộn lên để xem thêm) ...");
            }

            for (int i = batDau; i < ketThuc; i++)
            {
                var muc = danhSachGoc[i];
                string hienThiText = hienThi(muc); // Lấy text hiển thị
                bool daChon = cacMucDaChon.Contains(muc);

                if (i == mucChonHienTai)
                {
                    // Đánh dấu mục đang trỏ chuột
                    DatMau(ConsoleColor.Black, ConsoleColor.Green);
                    Console.Write(">> ");
                }
                else
                {
                    DatMau(ConsoleColor.White);
                    Console.Write("   ");
                }

                // Đánh dấu mục đã được chọn (bằng Space)
                if (daChon)
                {
                    DatMau(ConsoleColor.Cyan);
                    Console.Write("[X] ");
                }
                else
                {
                    DatMau(ConsoleColor.DarkGray);
                    Console.Write("[ ] ");
                }
                
                DatMau(ConsoleColor.Gray);
                Console.WriteLine(hienThiText);

                Console.ResetColor();
            }
            
            if (ketThuc < danhSachGoc.Count)
            {
                 Console.WriteLine("  ... (cuộn xuống để xem thêm) ...");
            }

            // 3. In hướng dẫn
            Console.WriteLine("\nLên/Xuống: Di chuyển | Space: Chọn/Bỏ | Enter: Xác nhận (khi đủ) | Esc: Hủy");

            // 4. Đọc phím
            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    mucChonHienTai = (mucChonHienTai > 0) ? mucChonHienTai - 1 : danhSachGoc.Count - 1;
                    break;
                case ConsoleKey.DownArrow:
                    mucChonHienTai = (mucChonHienTai < danhSachGoc.Count - 1) ? mucChonHienTai + 1 : 0;
                    break;
                case ConsoleKey.Spacebar:
                    var mucDuocChon = danhSachGoc[mucChonHienTai];
                    if (cacMucDaChon.Contains(mucDuocChon))
                    {
                        // Nếu đã chọn -> Bỏ chọn
                        cacMucDaChon.Remove(mucDuocChon);
                    }
                    else
                    {
                        // Nếu chưa chọn -> Kiểm tra nếu còn slot
                        if (cacMucDaChon.Count < soLuongCanChon)
                        {
                            cacMucDaChon.Add(mucDuocChon);
                        }
                        else
                        {
                            // Đã đủ số lượng
                            Console.Beep();
                        }
                    }
                    break;
                case ConsoleKey.Enter:
                    if (cacMucDaChon.Count == soLuongCanChon)
                    {
                        Console.CursorVisible = true;
                        return new List<T>(cacMucDaChon); // Trả về List
                    }
                    else
                    {
                        // Chưa chọn đủ
                        HienThiThongBao($"Vui lòng chọn đủ {soLuongCanChon} sản phẩm.", ConsoleColor.Red);
                    }
                    break;
                case ConsoleKey.Escape:
                    Console.CursorVisible = true;
                    return null; // Người dùng hủy
            }
        }
    }


    /// <summary>
    /// Hiển thị hộp thoại xác nhận (C/K).
    /// </summary>
    /// <returns>True nếu nhấn 'C' (Có), False nếu nhấn 'K' (Không) hoặc Esc.</returns>
    public static bool XacNhan(string cauHoi)
    {
        DatMau(ConsoleColor.Yellow);
        Console.Write($"\n{cauHoi} (C/K): ");
        Console.ResetColor();

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.C)
            {
                Console.WriteLine("Có");
                return true;
            }
            if (key.Key == ConsoleKey.K || key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Không");
                return false;
            }
        }
    }

    /// <summary>
    /// (BỔ SUNG LẠI)
    /// Hiển thị một thông báo nổi bật (dùng cho Lỗi, Thành công, Cảnh báo)
    /// và tạm dừng màn hình.
    /// </summary>
    public static void HienThiThongBao(string thongBao, ConsoleColor mauNen)
    {
        // Tự động chọn màu chữ (Trắng/Đen) dựa trên màu nền để đảm bảo độ tương phản
        ConsoleColor mauChu;
        switch (mauNen)
        {
            case ConsoleColor.Red:
            case ConsoleColor.DarkRed:
            case ConsoleColor.Blue:
            case ConsoleColor.DarkBlue:
                mauChu = ConsoleColor.White;
                Console.Beep(500, 200); // Beep cho lỗi
                break;
            case ConsoleColor.Green:
            case ConsoleColor.Yellow:
                mauChu = ConsoleColor.Black;
                break;
            case ConsoleColor.Black:
            default:
                mauChu = ConsoleColor.White;
                mauNen = ConsoleColor.Black; // Đảm bảo nền là đen nếu không chỉ định màu
                break;
        }

        Console.WriteLine();
        DatMau(mauChu, mauNen);
        // Thêm padding 2 bên cho nổi bật
        Console.WriteLine($" => {thongBao} <=");
        Console.ResetColor();
        Console.WriteLine("(Nhấn phím bất kỳ để tiếp tục...)");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Đọc một chuỗi (string) từ Console, hỗ trợ Placeholder và phím Esc để hủy.
    /// </summary>
    /// <param name="loiNhac">Dòng nhắc nhở (VD: "Nhập tên: ").</param>
    /// <param name="giaTriMacDinh">Giá trị hiển thị sẵn (dùng khi Sửa).</param>
    /// <param name="batBuoc">True nếu không cho phép chuỗi rỗng.</param>
    /// <returns>Chuỗi người dùng nhập. Trả về NULL nếu nhấn Escape.</returns>
    public static string? DocChuoi(string loiNhac, string giaTriMacDinh = "", bool batBuoc = false)
    {
        Console.CursorVisible = true;
        Console.Write(loiNhac);

        // Hiển thị giá trị mặc định (nếu có)
        var buffer = new StringBuilder();
       
        
        int viTriConTro = 0;
        // Lưu vị trí con trỏ ban đầu
        int left = Console.CursorLeft;
        int top = Console.CursorTop;

        while (true)
        {
            Console.SetCursorPosition(left + viTriConTro, top);
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                // --- LOGIC ĐÃ SỬA ---
                if (buffer.Length == 0) // Nếu người dùng không nhập gì
                {
                    if (batBuoc)
                    {
                        // Nếu bắt buộc -> Báo lỗi
                    Console.SetCursorPosition(0, top + 1);
                    HienThiThongBao("Trường này là bắt buộc, không được để trống!", ConsoleColor.Red);
                        
                        // Vẽ lại dòng nhắc (để xóa thông báo lỗi nếu nó đè lên)
                        Console.SetCursorPosition(0, top);
                        Console.Write(new string(' ', Console.WindowWidth)); // Xóa dòng
                        Console.SetCursorPosition(0, top);
                        Console.Write(loiNhac);
                        left = Console.CursorLeft; // Cập nhật lại 'left'
                        
                        // RedrawBuffer(left, top, buffer.ToString(), viTriConTro);
                    }
                    else
                    {
                        // Nếu không bắt buộc -> Chấp nhận giá trị mặc định (có thể là "" hoặc giá trị cũ)
                        Console.WriteLine();
                        Console.CursorVisible = false;
                        return giaTriMacDinh; 
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.CursorVisible = false;
                    return buffer.ToString();
                }
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                Console.CursorVisible = false;
                return null; // Hủy thao tác
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (viTriConTro > 0)
                {
                    viTriConTro--;
                    buffer.Remove(viTriConTro, 1);
                    // Xóa và vẽ lại buffer
                    RedrawBuffer(left, top, buffer.ToString(), viTriConTro);
                }
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (viTriConTro > 0) viTriConTro--;
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                if (viTriConTro < buffer.Length) viTriConTro++;
            }
            else if (key.Key == ConsoleKey.Home)
            {
                viTriConTro = 0;
            }
            else if (key.Key == ConsoleKey.End)
            {
                viTriConTro = buffer.Length;
            }
            else if (!char.IsControl(key.KeyChar))
            {
                // Hỗ trợ Tiếng Việt (UTF8)
                buffer.Insert(viTriConTro, key.KeyChar.ToString());
                viTriConTro++;
                RedrawBuffer(left, top, buffer.ToString(), viTriConTro);
            }
        }
    }
    
    // Hàm trợ giúp vẽ lại buffer cho DocChuoi
    private static void RedrawBuffer(int left, int top, string buffer, int viTriConTro)
    {
        Console.SetCursorPosition(left, top);
        // Xóa ký tự cũ (nếu có) bằng khoảng trắng
        Console.Write(buffer + " "); 
        Console.SetCursorPosition(left + viTriConTro, top);
    }

    /// <summary>
    /// Đọc một số nguyên (int), hỗ trợ Esc, validation.
    /// </summary>
    public static int? DocSoNguyen(string loiNhac, int? giaTriMacDinh = null)
    {
        while(true)
        {
            string? input = DocChuoi(loiNhac, giaTriMacDinh?.ToString() ?? "", false);
            
            if (input == null) return null; // Hủy (Escape)
            if (string.IsNullOrEmpty(input) && giaTriMacDinh != null) return giaTriMacDinh; // Giữ giá trị cũ
            if (string.IsNullOrEmpty(input) && giaTriMacDinh == null)
            {
                 HienThiThongBao("Vui lòng nhập một số.", ConsoleColor.Red);
                 continue;
            }
            
            if (int.TryParse(input, out int ketQua))
            {
                return ketQua;
            }
            else
            {
                HienThiThongBao("Đầu vào không phải là một số nguyên hợp lệ. Vui lòng nhập lại.", ConsoleColor.Red);
            }
        }
    }
    
    /// <summary>
    /// Đọc một số thập phân (decimal), hỗ trợ Esc, validation.
    /// (BỔ SUNG MỚI)
    /// </summary>
    public static decimal? DocSoThapPhan(string loiNhac, decimal? giaTriMacDinh = null)
    {
        // Sử dụng CultureInfo "vi-VN" để chấp nhận dấu phẩy (,) làm dấu thập phân
        var culture = new CultureInfo("vi-VN");

        while (true)
        {
            string? input = DocChuoi(loiNhac, giaTriMacDinh?.ToString() ?? "", false);

            if (input == null) return null; // Hủy (Escape)
            if (string.IsNullOrEmpty(input) && giaTriMacDinh != null) return giaTriMacDinh; // Giữ giá trị cũ
            if (string.IsNullOrEmpty(input) && giaTriMacDinh == null)
            {
                HienThiThongBao("Vui lòng nhập một số.", ConsoleColor.Red);
                continue;
            }

            // Cho phép nhập cả '.' và ','
            input = input.Replace('.', ',');

            if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint, culture, out decimal ketQua))
            {
                if (ketQua < 0)
                {
                    HienThiThongBao("Số không được âm.", ConsoleColor.Red);
                    continue;
                }
                return ketQua;
            }
            else
            {
                HienThiThongBao("Đầu vào không phải là một số thập phân hợp lệ. Vui lòng nhập lại.", ConsoleColor.Red);
            }
        }
    }

    /// <summary>
    /// Hàm tiện ích đặt màu Console.
    /// </summary>
    private static void DatMau(ConsoleColor foreground, ConsoleColor background = ConsoleColor.Black)
    {
        Console.ForegroundColor = foreground;
        Console.BackgroundColor = background;
    }
}