/// <summary>
/// Định nghĩa cấu trúc dữ liệu cho Mặt hàng (Sản phẩm)
/// theo Yêu cầu 2.1 của đề tài.
/// 
/// (SỬA LỖI LOGIC: Chuyển từ public fields sang public properties {get; set;}
/// để tương thích với System.Text.Json.Deserialize)
/// </summary>
public struct SanPham
{
    // Đã thay đổi từ "public string MaSP;"
    public string MaSP { get; set; }
    public string TenSP { get; set; }
    public string DonViTinh { get; set; }
    public decimal GiaBan { get; set; }
    public int SoLuongTonKho { get; set; }

    /// <summary>
    /// Hàm khởi tạo (constructor) cho struct SanPham.
    /// </summary>
    public SanPham(string maSP, string tenSP, string donViTinh, decimal giaBan, int soLuongTonKho)
    {
        MaSP = maSP;
        TenSP = tenSP;
        DonViTinh = donViTinh;
        GiaBan = giaBan;
        SoLuongTonKho = soLuongTonKho;
    }
}