namespace HoatDongSinhVien.Models.Services_Interfaces
{
    public interface ITaoUpdateGGForm
    {
        Task<string> CreateGoogleFormAsync(string activityName, string moTa);
        Task<string> UpdateGoogleFormAsync(string oldName, string newName, string newDesc);
    }
}
