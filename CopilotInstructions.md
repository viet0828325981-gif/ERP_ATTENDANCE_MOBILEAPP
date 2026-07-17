# Copilot Instructions — Kiến trúc dự án

## Nguyên tắc cốt lõi

Dự án này **KHÔNG** có tầng Service xử lý business logic ở phía client.
Toàn bộ business logic (tính toán, validate nghiệp vụ, truy vấn dữ liệu) đã nằm ở **API** (stored procedure / backend).

Phía client chỉ có 2 việc:
1. **UI** — hiển thị, nhận input từ người dùng.
2. **API caller** — gọi API và map kết quả trả về ra UI.

Không tự ý tạo thêm `XxxService.cs` để xử lý logic nghiệp vụ. Nếu cần xử lý gì đó, hỏi lại trước khi thêm.


## Cấu trúc thư mục UI thực tế 

wwwroot/css/app-tokens.css        ← import 1 lần duy nhất ở App.razor/_Host, chứa toàn bộ màu + font
Components/Shared/
  AppTopBar.razor  (+.cs, +.css)  ← header dùng chung cho cả 4 tab
  AppBottomNav.razor (+.cs, +.css)← bottom nav dùng chung, tự set active theo trang
  CalendarWidget.razor (+.cs, +.css) ← đã có từ trước
Components/Employee/
  YeuCauCong.razor                ← chỉ chứa phần nội dung riêng (filter, request-card)
  DangKyCa.razor                  ← chỉ chứa phần nội dung riêng (calendar, info-card)

/Components
    /Layout                 -> Layout dùng chung (MainLayout, NavMenu...)
    /Pages
        /Employee           -> Các trang dành cho role Employee
		   /Dali            -> Các màn nhỏ trong role Employee
		   /Request
		   /...
        /Hr                 -> Các trang dành cho role HR
		   /Dali            -> Các màn nhỏ trong role HR
		   /Request
		   /...
        /Manager            -> Các trang dành cho role Manager
		   /Dali            -> Các màn nhỏ trong role Manager
		   /Request
		   /...
        /Setting            -> Các trang cấu hình hệ thống
        /Shared             -> Component/trang dùng chung giữa nhiều role
        Error.razor
        Home.razor
        Login.razor
        NotFound.razor
    _Imports.razor
    App.razor
    RedirectToLogin.razor
    Routes.razor

Quy tắc khi thêm trang mới trong /Pages:


Trang chỉ phục vụ 1 role cụ thể → đặt vào đúng thư mục role đó (Employee, Hr, Manager, Setting).
Trang/component dùng chung từ 2 role trở lên → đặt vào Shared, không copy ra nhiều nơi.
Mỗi trang .razor trong các thư mục trên chỉ chứa UI + gọi RequestBuilder/ApiClient như luồng chuẩn bên dưới, không tự chứa logic nghiệp vụ.
Không tạo thêm cấp thư mục role mới nếu chưa xác nhận với người phụ trách kiến trúc.

## Cấu trúc thư mục bắt buộc


```text
/Api
	XxxApiClient.cs        -> Chỉ gọi HTTP (GET/POST/PUT/DELETE), không xử lý logic gì thêm
							   ngoài serialize/deserialize và bắt lỗi HTTP.

/Models
	XxxRequest.cs          -> DTO gửi lên API (input)
	XxxResponse.cs         -> DTO nhận về từ API (output)
	(đặt tên khớp với DTO bên TinhLuong.Api / backend tương ứng)

/Pages  hoặc  /Components
	XxxPage.razor          -> UI thuần: hiển thị, binding, sự kiện người dùng.
							   KHÔNG chứa logic tính toán hay điều kiện nghiệp vụ ở đây.

/RequestBuilders
	XxxRequestBuilder.cs   -> File DUY NHẤT chịu trách nhiệm: lấy dữ liệu từ đâu
							   (UI state, cache, session, ngày hiện tại...) và ráp
							   thành Request DTO để truyền vào API.
							   Đây là nơi duy nhất được phép có "logic" kiểu:
							   - lấy EmployeeId từ session
							   - lấy tháng/năm đang chọn trên UI
							   - gộp filter, set default value
							   Không được tính toán nghiệp vụ (lương, công...) ở đây,
							   chỉ chuẩn bị input.
```

## Luồng xử lý chuẩn (bắt buộc theo đúng thứ tự)

```text
UI (Page/Component)
   -> gọi RequestBuilder để build input
	  -> RequestBuilder trả về XxxRequest (Model)
   -> gọi ApiClient.CallXxxAsync(request)
	  -> ApiClient gọi HTTP tới TinhLuong.Api
	  -> nhận về Response (Model)
   -> UI bind Response ra màn hình
```
## Quy tắc xây dựng UI
- khôgn fix cứng giá trị trong css mà luôn viết để đap ứng được tất cả kích cỡ màn
- Mục tiêu cuối cùng là xây UI app mobile nên luôn xây theo kích cỡ màn dọc điện thoại
- phần age Header chỉ được chiếm tối đa 15% chiều cao màn hình, phần body chiếm tối đa 85% chiều cao màn hình
- ưu tiên hiển thị và tiện dụng



## Quy tắc đặt tên & phân chia trách nhiệm

| Thư mục | Được làm | KHÔNG được làm |
|---|---|---|
| `/Api` | Gọi HttpClient, serialize/deserialize, bắt lỗi HTTP | Tính toán nghiệp vụ, điều kiện if theo nghiệp vụ |
| `/Models` | Định nghĩa property DTO, attribute JSON | Có method xử lý logic |
| `/Pages` `/Components` | Binding, hiển thị, bắt sự kiện UI | Gọi trực tiếp HttpClient, tính toán dữ liệu |
| `/RequestBuilders` | Lấy dữ liệu input từ UI/state, ráp thành Request DTO | Tính lương, tính công, gọi thêm API khác |

## Khi Copilot gợi ý code


- Nếu cần thêm 1 API mới: tạo method trong `ApiClient` tương ứng + `Request`/`Response` trong `/Models`.
- Nếu cần chuẩn bị dữ liệu trước khi gọi API: viết trong `RequestBuilder`, KHÔNG viết trực tiếp trong code-behind của `.razor`.
- Nếu Copilot thấy code-behind `.razor` đang có logic tính toán phức tạp (không phải build request) → cảnh báo và đề xuất chuyển logic đó lên API/stored procedure, không tạo Service mới ở client để "chứa tạm".
- Không tự sinh thêm tầng `Repository`, `Service`, `Manager`... trừ khi được yêu cầu rõ ràng.

## Áp dụng

File này là hướng dẫn mặc định cần được ưu tiên đọc và tuân thủ trong mỗi lần hỗ trợ ở workspace này.

## yêu càu

luôn chat và lập plan bằng tiếng việt, không tự động thêm mới những thứ khong được yêu cầu
