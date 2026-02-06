namespace HoatDongSinhVien.Models.Services_Interfaces
{
    public interface InterfaceOCR
    {
        public Task<MinhChung> ExtractMinhChungFromFlask(string imagePath, string idHoatDong);
    }
}
