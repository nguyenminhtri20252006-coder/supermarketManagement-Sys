using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web; // Cần cho Tiếng Việt
using System.Text.Json;         // Thư viện JSON của .NET Core
using System.Text.Unicode;      // Cần cho Tiếng Việt

/// <summary>
/// Module (static class) chịu trách nhiệm mô phỏng việc đọc/ghi CSDL
/// bằng cách sử dụng tệp JSON (Yêu cầu 5).
/// </summary>
public static class Database
{
    // Đường dẫn tới tệp "CSDL" JSON
    private const string DUONG_DAN_FILE = "Data/sanpham.json";

    /// <summary>
    /// Tải dữ liệu từ tệp JSON vào 2 Dictionary trên RAM (trong QuanLySanPham).
    /// </summary>
    public static void LoadDuLieu()
    {
        Console.WriteLine("Đang tải dữ liệu từ CSDL (JSON)...");

        if (!File.Exists(DUONG_DAN_FILE))
        {
            Console.WriteLine("Cảnh báo: Không tìm thấy tệp 'Data/sanpham.json'. Bỏ qua tải dữ liệu.");
            return;
        }

        try
        {
            // 1. Đọc tệp JSON
            string jsonString = File.ReadAllText(DUONG_DAN_FILE);
            
            // 2. Chuyển đổi (Deserialize) JSON thành List<SanPham>
            // (Chúng ta lưu dạng List, nhưng tải vào Dictionary)
            var sanPhams = JsonSerializer.Deserialize<List<SanPham>>(jsonString);

            // 3. Xóa dữ liệu cũ trên RAM (nếu có)
            QuanLySanPham.dsTheoMa.Clear();
            QuanLySanPham.dsTheoTen.Clear();

            // 4. Nạp dữ liệu mới vào 2 Dictionary (Yêu cầu 2.1)
            if (sanPhams != null)
            {
                int soDongBiLoi = 0; // (BỔ SUNG) Biến đếm lỗi

                foreach (var sp in sanPhams)
                {
        
                    if (string.IsNullOrEmpty(sp.MaSP) || string.IsNullOrEmpty(sp.TenSP))
                    {
                        soDongBiLoi++;
                        continue; // Bỏ qua sản phẩm bị lỗi này
                    }

                    // 4.1. Nạp vào dsTheoMa
                    if (!QuanLySanPham.dsTheoMa.ContainsKey(sp.MaSP))
                    {
                        QuanLySanPham.dsTheoMa.Add(sp.MaSP, sp);
                    }
                    else
                    {
                        // (BỔ SUNG) Ghi nhận lỗi nếu trùng mã
                        soDongBiLoi++;
                    }

                    // 4.2. Nạp vào index dsTheoTen
                    string tenKey = sp.TenSP.ToLowerInvariant(); // (Đã an toàn vì ta đã check null ở trên)
                    if (!QuanLySanPham.dsTheoTen.ContainsKey(tenKey))
                    {
                        // Nếu chưa có tên này, tạo một danh sách mới
                        QuanLySanPham.dsTheoTen.Add(tenKey, new List<string>());
                    }
                    // Thêm MaSP vào danh sách của tên đó
                    QuanLySanPham.dsTheoTen[tenKey].Add(sp.MaSP);
                }

                Console.WriteLine($"Tải thành công {QuanLySanPham.dsTheoMa.Count} sản phẩm.");
                
                // (BỔ SUNG) Thông báo nếu có lỗi
                if (soDongBiLoi > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Cảnh báo: Đã bỏ qua {soDongBiLoi} sản phẩm bị lỗi (thiếu MaSP/TenSP hoặc trùng Mã) trong tệp JSON.");
                    Console.ResetColor();
                }
            }

            // Console.WriteLine($"Tải thành công {QuanLySanPham.dsTheoMa.Count} sản phẩm."); // (Di chuyển lên trên)
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LỖI NGHIÊM TRỌNG KHI ĐỌC FILE: {ex.Message}");
            Console.WriteLine("Vui lòng kiểm tra tệp 'Data/sanpham.json'");
            Console.ReadKey(); // Dừng lại để người dùng đọc lỗi
        }
    }

    /// <summary>
    /// Lưu toàn bộ dữ liệu từ Dictionary (RAM) vào lại tệp JSON.
    /// </summary>
    public static void LuuDuLieu()
    {
        Console.WriteLine("Đang lưu dữ liệu vào CSDL (JSON)...");
        try
        {
            // 1. Lấy toàn bộ dữ liệu từ dsTheoMa (nguồn chính)
            var danhSachSanPham = QuanLySanPham.dsTheoMa.Values.ToList();

            // 2. Cấu hình JSON Serializer để hỗ trợ Tiếng Việt
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // Ghi cho đẹp, dễ đọc
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) // Hỗ trợ Tiếng Việt
            };

            // 3. Chuyển đổi (Serialize) C# List -> Chuỗi JSON
            string jsonString = JsonSerializer.Serialize(danhSachSanPham, options);

            // 4. Ghi đè vào tệp
            File.WriteAllText(DUONG_DAN_FILE, jsonString);

            Console.WriteLine("Lưu dữ liệu thành công!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LỖI NGHIÊM TRỌNG KHI LƯU FILE: {ex.Message}");
        }
    }
}