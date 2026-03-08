# Website Quản Lý Minh Chứng Sinh Viên

## 📖 Tổng quan
**Website Quản Lý Minh Chứng Sinh Viên** là một ứng dụng web được thiết kế để hỗ trợ sinh viên trong việc lưu trữ, quản lý và phân loại các minh chứng về thành tích học tập cũng như hoạt động ngoại khóa một cách kỹ thuật số. Hệ thống giúp tối ưu hóa quy trình đánh giá của nhà trường bằng cách cung cấp một nền tảng tập trung để nộp và xác minh các bằng chứng.<br>
**Link website:** https://hdsv.azurewebsites.net/

## 🚀 Các tính năng chính
* **Quản lý tập trung:** Lưu trữ và tổ chức hồ sơ sinh viên, danh mục minh chứng và lịch sử hoạt động hiệu quả.
* **Tự động hóa quy trình:** Tích hợp **Google Apps Script** để tự động tạo và đồng bộ Google Forms/Sheets mỗi khi có hoạt động mới.
* **Lưu trữ an toàn:** Chức năng upload file bảo mật, hỗ trợ lưu trữ minh chứng dạng ảnh và tài liệu.
* **Phân quyền người dùng:** Hệ thống xác thực và phân quyền chặt chẽ sử dụng **ASP.NET Core Identity** (Admin/Sinh viên).
* **Hiệu suất cao:** Tối ưu hóa truy vấn dữ liệu giúp hệ thống vận hành mượt mà với lượng lớn bản ghi.

## 🛠️ Công nghệ sử dụng
* **Backend:** ASP.NET Core MVC, Entity Framework Core.
* **Cơ sở dữ liệu:** SQL Server.
* **Xác thực:** ASP.NET Core Identity.
* **Tự động hóa:** Google Apps Script.
* **Frontend:** HTML5, CSS3, JavaScript, Bootstrap.

## ⚙️ Hướng dẫn cài đặt

### Yêu cầu hệ thống
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) trở lên.
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB hoặc Express).
* Visual Studio 2022 hoặc Visual Studio Code.

### Các bước cài đặt
1.  **Clone dự án**
    ```bash
    git clone [https://github.com/Fong62/Website_Luu_Tru_Minh_Chung_Sinh_Vien.git](https://github.com/Fong62/Website_Luu_Tru_Minh_Chung_Sinh_Vien.git)
    cd Website_Luu_Tru_Minh_Chung_Sinh_Vien
    ```

2.  **Cấu hình Cơ sở dữ liệu**
    * Mở tệp `appsettings.json`, tìm và cập nhật chuỗi kết nối `DefaultString` phù hợp với máy.
    * Chạy lệnh sau để tạo database:
        ```bash
        dotnet ef database update
        ```

3.  **Chạy ứng dụng**
    ```bash
    dotnet run
    ```
    Truy cập `https://localhost:44369` (hoặc cổng hiển thị trên terminal) để bắt đầu sử dụng.

### 4. 🔐 Hướng dẫn Đăng nhập (Tài khoản mẫu)

Sau khi khởi chạy, hệ thống sẽ có sẵn các tài khoản sau để bạn kiểm thử:

#### 1. 👨‍💻 Tài khoản Quản trị viên (Admin)
* **Email:** `admin`
* **Mật khẩu:** `admin@123`
* **Quyền hạn:** Quản lý toàn bộ hệ thống, duyệt minh chứng, tạo hoạt động mới.

#### 2. 🎓 Tài khoản Sinh viên
* **Email:** `48.01.104.106`
* **Mật khẩu:** `Sv@123456`
* **Quyền hạn:** Đăng ký hoạt động, upload minh chứng, xem kết quả rèn luyện.

#### 3. 🧑‍🏫 Tài khoản Giảng viên & Ban cán sự
* **Giảng viên:**
    * Email: `LMT`
    * Pass: `Gv@123456`
* **Ban cán sự:**
    * Email: `48.01.104.059`
    * Pass: `Sv@123456`
* **Quyền hạn chung:** Theo dõi tiến độ của lớp và hỗ trợ quản lý minh chứng theo phân quyền.

## 📂 Cấu trúc thư mục
```text
Website_Minh_Chung
├── Controllers       # Xử lý luồng dữ liệu & Gọi Service
├── Models            # Entity & ViewModels
├── Data              # DbContext & Migrations
├── Services
├── Pages             $ Giao diện (Razor Page)
│   ├── TaoUpdateGGForm.cs  # Service giao tiếp với Google Apps Script (Tạo/Sửa Form)
│   └── ...                 # Các service khác
├── Views             # Giao diện
├── wwwroot           # Static files
└── appsettings.json  # Cấu hình hệ thống (Chứa Web App URL)
```

## 📞 Liên hệ
**Nguyễn Hoàng Phong**
* **Email:** nguyenhoangphongsupham@gmail.com
* **LinkedIn:** [Nguyễn Hoàng Phong](https://www.linkedin.com/in/fong62/)
* **GitHub:** [Fong62](https://github.com/Fong62)
