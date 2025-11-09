using System;
using System.Collections.Generic;
using System.Linq; // Cần cho .Find(), .Where(), .Remove()

/// <summary>
/// Module "đối thủ" (static class) để so sánh hiệu năng.
/// Sử dụng cấu trúc dữ liệu cơ bản 'List<SanPham>'.
/// Các thao tác tìm kiếm và xóa sẽ có độ phức tạp O(N).
/// </summary>
public static class QuanLySanPham_List
{
    // Cấu trúc dữ liệu cơ bản: List (O(N) cho tìm kiếm/xóa)
    public static List<SanPham> dsSanPham = new List<SanPham>();

    /// <summary>
    /// Thêm sản phẩm (O(1) - vì Add vào cuối List).
    /// </summary>
    public static bool ThemSanPham(SanPham sp)
    {
        // Kiểm tra trùng lặp (O(N)) - nhưng để công bằng với Dictionary,
        // trong phép đo Thêm, chúng ta sẽ giả định mã không trùng và chỉ đo tốc độ Add.
        // if (dsSanPham.Exists(p => p.MaSP == sp.MaSP)) return false;
        
        dsSanPham.Add(sp);
        return true;
    }

    /// <summary>
    /// Xóa sản phẩm (O(N)).
    /// </summary>
    public static bool XoaSanPham(string maSP)
    {
        // 1. Tìm sản phẩm (O(N))
        var spCanXoa = dsSanPham.FirstOrDefault(sp => sp.MaSP == maSP);
        
        if (spCanXoa.MaSP != null) // Found
        {
            // 2. Xóa sản phẩm (O(N) nữa)
            dsSanPham.Remove(spCanXoa);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Sửa sản phẩm (O(N)).
    /// </summary>
    public static bool SuaSanPham(SanPham spDaSua)
    {
        // 1. Tìm index (O(N))
        int index = dsSanPham.FindIndex(sp => sp.MaSP == spDaSua.MaSP);
        
        if (index != -1)
        {
            // 2. Cập nhật (O(1) khi đã có index)
            dsSanPham[index] = spDaSua;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tìm theo Mã (O(N)).
    /// </summary>
    public static bool TimTheoMa(string maSP, out SanPham timThay)
    {
        // Phải duyệt toàn bộ danh sách
        timThay = dsSanPham.FirstOrDefault(sp => sp.MaSP == maSP);
        return timThay.MaSP != null; // Trả về true nếu tìm thấy (MaSP không null)
    }

    /// <summary>
    /// Tìm theo Tên (O(N)).
    /// </summary>
    public static List<SanPham> TimTheoTen(string tenSP)
    {
        string tenKey = tenSP.ToLowerInvariant();
        // Phải duyệt toàn bộ danh sách
        return dsSanPham.Where(sp => sp.TenSP.ToLowerInvariant() == tenKey).ToList();
    }
    
    /// <summary>
    /// Xóa toàn bộ dữ liệu (dùng cho kiểm thử).
    /// </summary>
    public static void XoaTatCa()
    {
        dsSanPham.Clear();
    }
}