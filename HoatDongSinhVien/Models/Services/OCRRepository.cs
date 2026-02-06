using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using HoatDongSinhVien.Models.Services_Interfaces;

namespace HoatDongSinhVien.Models.Services
{
    public class OCRRepository : InterfaceOCR
    {
        private readonly HttpClient _httpClient;

        public OCRRepository(HttpClient httpClient) { _httpClient = httpClient; }

        public async Task<MinhChung> ExtractMinhChungFromFlask(string imagePath, string idHoatDong)
        {
            using MultipartFormDataContent form = new();
            StreamContent file = new(File.OpenRead(imagePath));
            file.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            form.Add(file, "file", Path.GetFileName(imagePath));

            HttpResponseMessage res = await _httpClient.PostAsync("http://127.0.0.1:5000/ocr", form);
            if (!res.IsSuccessStatusCode) return null;

            OcrResponse ocr = JsonSerializer.Deserialize<OcrResponse>(await res.Content.ReadAsStringAsync());
            return ParseMinhChung(ocr.corrected_text, imagePath, idHoatDong);
        }

        private MinhChung ParseMinhChung(string text, string img, string id)
        {
            string mssv = "Chưa xác định";

            if (!string.IsNullOrWhiteSpace(text))
            {
                foreach (var rawLine in text.Split('\n'))
                {
                    var line = rawLine.Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    if (line.StartsWith("MSSV", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split(':', 2);
                        if (parts.Length == 2)
                            mssv = parts[1].Trim();
                    }
                }
            }

            return new MinhChung
            {
                MSSV = mssv,
                IDHoatDong = id,
                AnhTheSV = img,
                ThoiGianDiemDanh = DateTime.Now,
                TrangThaiHienThi = mssv == "Chưa xác định" ? "Thất bại" : "Chờ duyệt",
                LyDoThatBai = mssv == "Chưa xác định" ? "OCR trả về MSSV chưa xác định" : "Không có"
            };
        }



    }
}
