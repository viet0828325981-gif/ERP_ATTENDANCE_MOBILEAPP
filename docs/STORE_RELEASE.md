# Phát hành Moon Tính Công

Workflow `.github/workflows/store-release.yml` build ứng dụng .NET MAUI và có thể upload:

- Android App Bundle lên Google Play (`vn.moon.tinhcong`).
- IPA lên App Store Connect/TestFlight (`vn.moon.tinhcong`, Apple app ID `6798483055`).

Workflow chỉ chạy thủ công. Input `deploy=false` chỉ tạo artifact để kiểm tra; `deploy=true` mới gửi binary ra store. Nên chạy Android ở track `internal`, trạng thái `draft`, trước khi dùng track production.

## GitHub environment

Tạo environment `store-release`. Nếu tổ chức hỗ trợ protected environments, cấu hình required reviewer để mỗi lần upload store cần được duyệt.

Repository/environment secrets:

| Tên | Nội dung |
| --- | --- |
| `ANDROID_KEYSTORE_BASE64` | Keystore upload Android được mã hóa base64 một dòng |
| `ANDROID_KEY_ALIAS` | Alias của upload key |
| `ANDROID_KEY_PASSWORD` | Mật khẩu private key Android |
| `ANDROID_KEYSTORE_PASSWORD` | Mật khẩu keystore Android |
| `FIREBASE_GOOGLE_SERVICES_JSON` | Toàn bộ file `google-services.json` của Firebase Android app `vn.moon.tinhcong` |
| `GOOGLE_PLAY_SERVICE_ACCOUNT_JSON` | Toàn bộ JSON key của service account có quyền trên app Moon Tính Công |
| `APPSTORE_CERTIFICATES_FILE_BASE64` | Apple Distribution `.p12` được mã hóa base64 một dòng |
| `APPSTORE_CERTIFICATES_PASSWORD` | Mật khẩu export file `.p12` |
| `APPSTORE_API_PRIVATE_KEY` | Toàn bộ nội dung file `AuthKey_<KEY_ID>.p8` |

Environment variables:

| Tên | Giá trị |
| --- | --- |
| `APPSTORE_ISSUER_ID` | `24511f19-1bf8-48bc-a64e-fdd9a8d6d3e2` |
| `APPSTORE_API_KEY_ID` | Key ID ứng với file `.p8` |

## Chuẩn bị Google Play

1. Dùng một service account riêng cho CI và cấp quyền chỉ trên app `Moon Tính Công`.
2. Quyền tối thiểu cần có là xem thông tin app và quản lý release ở testing track được chọn.
3. Bật Google Play Developer API cho Google Cloud project chứa service account.
4. Giữ Android upload keystore và mật khẩu ở nơi backup ngoài GitHub. Không tạo upload key mới cho các bản cập nhật sau.
5. Lần đầu nên phát hành lên Internal Testing. Play Console còn yêu cầu hoàn tất các mục thiết lập app trước khi mở production.

File `Platforms/Android/google-services.json` trong lịch sử repo thuộc package ID cũ. Workflow luôn ghi đè file này từ secret `FIREBASE_GOOGLE_SERVICES_JSON` và dừng ngay nếu secret chưa được cấu hình, tránh phát hành bản có Firebase push sai app.

## Chuẩn bị App Store Connect

1. Tạo hoặc dùng App Store Connect API key có role `App Manager` và lưu file `.p8` ngay khi tạo; Apple chỉ cho tải file này một lần.
2. Tạo Apple Distribution certificate có private key, export thành `.p12`, rồi tạo provisioning profile loại App Store cho `vn.moon.tinhcong`.
3. API key được dùng để tải provisioning profile và upload IPA. Distribution certificate dùng để ký IPA.
4. Upload workflow đưa build vào App Store Connect/TestFlight. Việc gửi App Review chỉ thực hiện được sau khi metadata, screenshots, privacy, age rating và review information đã hoàn tất trong App Store Connect.

## Chạy workflow

Vào GitHub Actions → **Store release** → **Run workflow**:

1. Chọn `platform`.
2. Nhập `version_name`; lần đầu dùng `1.0` để khớp version đã tạo trên App Store Connect.
3. Chạy lần đầu với `deploy=false` để kiểm tra AAB/IPA.
4. Chạy lại với `deploy=true`; Android chọn `internal` + `draft` cho lần upload đầu.

`github.run_number` được dùng làm Android version code và iOS build number, nên mỗi workflow run tạo một số build tăng dần.

Workflow pin .NET SDK `9.0.314`, workload set `9.0.314.3` và Xcode `26.5` để bộ .NET for iOS khớp đúng phiên bản Xcode trên runner `macos-26`.
