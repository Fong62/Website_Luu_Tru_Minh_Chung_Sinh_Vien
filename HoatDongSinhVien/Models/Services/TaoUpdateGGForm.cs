using HoatDongSinhVien.Models.Services_Interfaces;

namespace HoatDongSinhVien.Models.Services
{
    public class TaoUpdateGGForm : ITaoUpdateGGForm
    {
        private readonly HttpClient _httpClient;
        private const string googleScriptUrlPhong = "https://script.google.com/macros/s/AKfycbxrGTk--s0-WTL_qCJSbn5BR3m3ojp4igAy6qzEvCD9pq69k_y7XVMtxYAeKP-bG_c5/exec";
        private const string googleScriptUrlHuy = "https://script.google.com/macros/s/AKfycbxQGWA02IH_HntzpIDfOqFk5lHp1z0zXbesnSC9_5bSc_I84ygFKxHiNQazmDZFxVnw/exec";

        public TaoUpdateGGForm(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }

        public async Task<string> CreateGoogleFormAsync(string activityName, string moTa)
        {
            try
            {
                // Gửi yêu cầu GET tới Google Apps Script để tạo form và nhận URL chỉnh sửa
                var response = await _httpClient.GetStringAsync($"{googleScriptUrlPhong}?action=create&activityName={activityName}&moTa={moTa}");

                // Trả về URL chỉnh sửa của Google Form (đảm bảo response hợp lệ)
                Console.WriteLine("Apps Script response: " + response);
                return response;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                Console.WriteLine("Lỗi khi gọi Google Apps Script: " + ex.Message);
                return "Có lỗi xảy ra khi tạo Google Form.";
            }
        }

        public async Task<string> UpdateGoogleFormAsync(string oldName, string newName, string newMoTa)
        {
            try
            {
                var url = $"{googleScriptUrlPhong}?action=update&oldName={oldName}&newName={newName}&newMoTa={newMoTa}";
                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine("Google Script URL gọi: " + url);
                return response;
            }
            catch (Exception ex)
            {
                return "Có lỗi xảy ra khi update: " + ex.Message;
            }
        }
    }
}
