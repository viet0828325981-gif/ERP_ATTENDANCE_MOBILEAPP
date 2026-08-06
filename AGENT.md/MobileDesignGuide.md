# Viet Attendance Mobile Design Guide

> Mục tiêu: giao diện mobile khoa học, gọn gàng, sạch sẽ, đẹp mắt và hiện đại.
> Thiết kế ưu tiên màn hình dọc điện thoại, thao tác nhanh bằng ngón tay, dữ liệu dễ quét, không trang trí thừa.

Tài liệu này là guide dành riêng cho UI mobile của dự án ERP Payroll Mobile. Khi thiết kế hoặc sửa màn hình mobile, vẫn giữ tinh thần của design system gốc: Forest Green là màu thương hiệu, card-based, nhiều khoảng thở, dữ liệu rõ ràng, trạng thái nghiệp vụ nhất quán.

---

## 1. Nguyên tắc Mobile App

- Luôn thiết kế cho màn hình dọc điện thoại.
- Header tối đa 15% chiều cao màn hình; body là vùng làm việc chính, tối thiểu khoảng 85%.
- Thông tin quan trọng nhất phải xuất hiện trong viewport đầu tiên.
- Mỗi màn hình chỉ nên có 1 hành động chính rõ ràng.
- Không nhồi nhiều bảng/cột trên mobile; ưu tiên card, list, row summary, bottom sheet hoặc dialog.
- Tap target tối thiểu 44px chiều cao/rộng.
- Tránh text quá dài trong button/chip; nếu cần thì dùng icon, tooltip hoặc rút gọn label.
- Layout phải tương thích nhiều kích cỡ điện thoại bằng padding/gap linh hoạt, không cố định theo một model máy cụ thể.

---

## 2. Color Palette

### Primary: Forest Green

| Token | Hex | Dùng cho |
|---|---|---|
| `--color-primary` | `#1B4332` | Button chính, active nav, dark card, điểm nhấn thương hiệu |
| `--color-primary-dark` | `#122B21` | Hover, pressed state, header tối |
| `--color-primary-mid` | `#2D6A4F` | Icon, chart bar đậm, secondary emphasis |
| `--color-primary-accent` | `#52B788` | Highlight, badge active, progress fill |
| `--color-primary-light` | `#95D5B2` | Chart/progress nhẹ |
| `--color-primary-pale` | `#D8F3DC` | Progress track, tag background |
| `--color-primary-bg` | `#EAF7EE` | Row hover, chip background |
| `--color-primary-subtle` | `#F3FBF5` | Section background, tint rất nhẹ |

Quy tắc:
- Toàn app dùng `#1B4332` làm màu primary duy nhất.
- Accent `#52B788` dùng cho highlight, chart, trạng thái đang chọn.
- Không tự thêm màu brand mới.
- Nền page không dùng trắng thuần; ưu tiên `#F6F8F5` để có cảm giác dịu và sạch.

### Neutral

| Token | Hex | Dùng cho |
|---|---|---|
| `--color-gray-900` | `#111111` | Heading chính |
| `--color-gray-700` | `#333333` | Body text, value |
| `--color-gray-500` | `#666666` | Label, secondary text |
| `--color-gray-400` | `#999999` | Hint, disabled |
| `--color-gray-200` | `#E0E0E0` | Border, divider |
| `--color-gray-100` | `#F5F5F5` | Background phụ |
| `--color-gray-50` | `#FAFAFA` | Alternating row, subtle area |
| `--color-white` | `#FFFFFF` | Card, input, sheet |

### Semantic

Semantic colors không đổi theo brand vì người dùng nhận diện trạng thái theo phản xạ.

| Token | Hex | Dùng cho |
|---|---|---|
| `--color-success` | `#2E7D32` | Approved, on-time, positive |
| `--color-success-bg` | `#E8F5E9` | Success badge background |
| `--color-warning` | `#E65100` | Pending, late, warning |
| `--color-warning-bg` | `#FFF3E0` | Warning badge background |
| `--color-danger` | `#C62828` | Rejected, absent, error |
| `--color-danger-bg` | `#FFEBEE` | Danger badge background |
| `--color-info` | `#01579B` | Informational |
| `--color-info-bg` | `#E1F5FE` | Info badge background |

### Attendance Status Dots

| Token | Hex | Ý nghĩa |
|---|---|---|
| `--dot-ontime` | `#52B788` | Đúng giờ |
| `--dot-late` | `#F4A261` | Đi trễ |
| `--dot-absent` | `#BDBDBD` | Vắng |
| `--dot-leave` | `#74C69D` | Nghỉ phép |
| `--dot-registered` | `#D8F3DC` | Đã đăng ký ca |
| `--dot-holiday` | `#1B4332` | Ngày lễ/làm thêm |

---

## 3. Typography Mobile

```css
font-family: 'Inter', 'Be Vietnam Pro', -apple-system, BlinkMacSystemFont, sans-serif;
```

| Role | Size | Weight | Line-height | Dùng cho |
|---|---:|---:|---:|---|
| Screen Title | 20px | 700 | 1.3 | Tiêu đề màn |
| Section Title | 17px | 700 | 1.35 | Tiêu đề card/section |
| Card Title | 16px | 600 | 1.4 | Card nhỏ |
| Body | 15px | 400 | 1.55 | Nội dung chính |
| Label | 13px | 500 | 1.4 | Label, caption |
| Helper | 12px | 400 | 1.35 | Hint, mô tả phụ |
| Stat Number Large | 32px | 800 | 1.1 | KPI nổi bật |
| Stat Number | 22px | 700 | 1.15 | Số liệu trong card |
| Badge/Chip | 12px | 500 | 1 | Trạng thái |

Quy tắc:
- Không in hoa toàn bộ, trừ badge rất ngắn.
- Không dùng quá 2 font weight trong cùng 1 component.
- Số liệu lương/công dùng `font-variant-numeric: tabular-nums;`.
- Mỗi màn chỉ có 1 tiêu đề chính.
- Không scale font theo viewport width.

---

## 4. Spacing Mobile

Hệ spacing dùng bội số 4px, nhưng nên khai báo bằng token linh hoạt để tự co giãn theo kích cỡ màn hình.

| Size | Dùng cho |
|---:|---|
| 4px | Khoảng cách rất nhỏ giữa icon và text |
| 8px | Gap giữa control nhỏ |
| 12px | Padding chip/badge/input compact |
| 16px | Padding card mobile, gap tiêu chuẩn |
| 20px | Padding section/card lớn |
| 24px | Gap giữa các card/section |
| 28px | Khoảng cách lớn trên màn điện thoại cao/rộng hơn |

### Fluid Spacing Tokens

```css
:root {
    --space-page-x: clamp(12px, 4vw, 20px);
    --space-page-y: clamp(12px, 3vh, 20px);
    --space-card: clamp(14px, 4vw, 20px);
    --space-section: clamp(16px, 5vw, 24px);
    --space-list-gap: clamp(8px, 2.5vw, 14px);
    --space-control-gap: clamp(8px, 2vw, 12px);
}
```

Quy tắc:
- Page padding dùng `--space-page-x`, không fix cứng 16px cho mọi màn.
- Card padding dùng `--space-card`, không fix cứng nếu card chứa nhiều dữ liệu.
- Gap giữa các section dùng `--space-section`.
- Gap trong list/form dùng `--space-list-gap` hoặc `--space-control-gap`.
- Không dùng giá trị lẻ như 7px, 13px, 18px.
- Danh sách nhiều dòng nên dùng divider nhẹ thay vì tăng gap quá lớn.
- Khi màn thấp, giảm khoảng cách dọc trước khi giảm kích thước chữ.

---

## 5. Border, Shape, Shadow

### Radius

| Radius | Dùng cho |
|---:|---|
| 4px | Tag nhỏ, indicator |
| 8px | Button, input, badge, row hover |
| 12px | Card nhỏ, bottom sheet item |
| 16px | Card tiêu chuẩn |
| 20px | Panel lớn, dark card |
| 50% | Avatar, status dot, icon circle |

### Border và Shadow

```css
border: 1px solid #F0F0F0;
box-shadow: 0 2px 12px rgba(0, 0, 0, 0.06);
```

Quy tắc:
- Chỉ dùng 3 mức elevation: flat, card, dialog/sheet.
- Không dùng shadow đậm hoặc nhiều lớp.
- Nếu background/border đã đủ phân tách, không thêm shadow.
- Divider trong card dùng `#F0F0F0` hoặc `#F5F5F5`.

---

## 6. Layout Mobile

### Page Structure

```text
Screen
  Header / top bar         <= tối đa 15% chiều cao màn hình
  Content scroll area      <= vùng chính
  Bottom nav / action      <= nếu có, luôn cố định và dễ chạm
```

```css
.mobile-screen {
    min-height: 100dvh;
    padding-left: max(var(--space-page-x), env(safe-area-inset-left));
    padding-right: max(var(--space-page-x), env(safe-area-inset-right));
    padding-top: max(var(--space-page-y), env(safe-area-inset-top));
    padding-bottom: max(var(--space-page-y), env(safe-area-inset-bottom));
}
```

### Layout Rules

- Một cột là mặc định trên mobile.
- KPI summary đặt trên đầu màn, nhưng không chiếm quá nhiều chiều cao.
- Bộ lọc nên dùng compact row, segmented control, chips hoặc bottom sheet.
- Calendar và chart phải có chiều cao ổn định, tránh nhảy layout khi đổi dữ liệu.
- Không đặt card trong card.
- Không đặt chiều cao cố định lớn cho card; dùng `min-height`, `max-height`, `aspect-ratio` và scroll nội bộ khi thật sự cần.
- Dùng `minmax(0, 1fr)`, `flex-wrap`, `overflow-wrap: anywhere` để tránh text/value làm vỡ layout.
- Vùng nội dung có bottom nav phải chừa khoảng cách dưới bằng chiều cao nav cộng safe-area.

### Mobile Size Bands

| Width | Hành vi |
|---:|---|
| `< 360px` | Điện thoại hẹp: giảm gap dọc, rút gọn label dài, ưu tiên 1 stat mỗi hàng |
| `360-399px` | Điện thoại phổ biến: layout 1 cột, stat nhỏ có thể 2 cột nếu đủ chỗ |
| `400-479px` | Điện thoại lớn: tăng nhẹ padding/gap bằng token linh hoạt |
| `>= 480px` | Điện thoại rất rộng/gập mở: vẫn giữ mobile app layout, chỉ tăng độ thoáng và chiều rộng control |

### Flexible Layout Patterns

```css
.mobile-stack {
    display: flex;
    flex-direction: column;
    gap: var(--space-section);
}

.mobile-row {
    display: flex;
    align-items: center;
    gap: var(--space-control-gap);
    min-width: 0;
}

.mobile-row > * {
    min-width: 0;
}

.mobile-stat-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(136px, 1fr));
    gap: var(--space-list-gap);
}
```

---

## 7. Components Mobile

### Card

```css
.mobile-card {
    background: #FFFFFF;
    border: 1px solid #F0F0F0;
    border-radius: 16px;
    padding: var(--space-card);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.06);
    min-width: 0;
}
```

Nguyên tắc:
- Mỗi card chỉ truyền đạt 1 nhóm thông tin.
- Tiêu đề rõ ở trên cùng.
- Value quan trọng đặt trước, chi tiết đặt sau.
- Tránh nhồi quá nhiều action trong một card.

### Stat Card

- Label 13px, muted.
- Số liệu 22-32px, đậm, dùng tabular nums.
- Nếu có trend/status, dùng badge nhỏ cạnh value.
- Trên mobile ưu tiên grid 2 cột cho stat nhỏ, 1 cột cho stat quan trọng.

### Button

```css
.btn-primary {
    min-height: 44px;
    border-radius: 8px;
    padding: 10px 16px;
    background: #1B4332;
    color: #FFFFFF;
    font-size: 14px;
    font-weight: 600;
}
```

Quy tắc:
- Button chính nên full-width khi là action cuối form/dialog.
- Không đặt 2 primary button cạnh nhau.
- Icon button phải có vùng chạm tối thiểu 44px.
- Danger action không đặt quá gần primary action nếu dễ bấm nhầm.

### Input / Form

- Label nằm trên input, không inline.
- Input cao tối thiểu 44px.
- Required dùng `*` màu danger.
- Error text đặt ngay dưới input, 12-13px.
- Form dài nên chia section hoặc dùng step/dialog.

```css
.input {
    width: 100%;
    min-height: 44px;
    border: 1px solid #E0E0E0;
    border-radius: 8px;
    padding: 10px 14px;
    font-size: 14px;
    background: #FFFFFF;
}

.input:focus {
    outline: none;
    border-color: #2D6A4F;
    box-shadow: 0 0 0 3px rgba(45, 106, 79, 0.12);
}
```

### Badge / Status Chip

```css
.badge {
    display: inline-flex;
    align-items: center;
    min-height: 24px;
    padding: 4px 10px;
    border-radius: 999px;
    font-size: 12px;
    font-weight: 500;
    white-space: nowrap;
}
```

- Trạng thái nghiệp vụ phải dùng semantic color.
- Badge không thay thế nội dung chính; chỉ là nhãn trạng thái.
- Trên mobile, badge dài cần rút gọn text.

### List Dữ Liệu

Dữ liệu nhiều trường phải hiển thị dạng list/card thay vì nhồi nhiều cột ngang.

```text
Card/List item
  Dòng 1: Tên / ngày / mã chính + trạng thái
  Dòng 2: Thông tin phụ
  Dòng 3: Số liệu hoặc action liên quan
```

Quy tắc:
- Không quá 3 nhóm thông tin trong một item.
- Số liệu căn phải nếu nằm cùng hàng với label.
- Status luôn nằm gần thông tin mà nó mô tả.
- Nếu cần nhiều chi tiết, mở bottom sheet/detail page.
- Text dài dùng `overflow-wrap: anywhere` hoặc xuống dòng có kiểm soát.
- Giá trị quan trọng không được bị che bởi icon/action.

### Progress Bar

```css
.progress-track {
    height: 8px;
    border-radius: 999px;
    background: #D8F3DC;
    overflow: hidden;
}

.progress-fill {
    height: 100%;
    border-radius: inherit;
    background: #52B788;
    transition: width 0.25s ease;
}
```

- Progress phải có label/value rõ ràng, không chỉ hiển thị màu.
- Khi đạt ngưỡng quan trọng, dùng `#2D6A4F`.
- Warning thấp dùng `#E65100`.

### Bottom Navigation

- Dùng cho các tab chính.
- Tối đa 5 item.
- Icon 22-24px, label 11-12px.
- Active dùng primary hoặc primary background.
- Có hỗ trợ safe-area ở cạnh dưới.

```css
.bottom-nav {
    padding-bottom: max(8px, env(safe-area-inset-bottom));
}
```

### Dialog / Bottom Sheet

- Bottom sheet phù hợp cho chọn ca, lọc dữ liệu, chọn tháng/năm.
- Dialog phù hợp cho xác nhận hoặc form ngắn.
- Action chính đặt cuối sheet/dialog, dễ chạm.
- Nội dung scroll trong sheet, action giữ cố định nếu form dài.

---

## 8. Content Hierarchy

```text
Level 1: Screen Title
Level 2: Section/Card Title
Level 3: Stat Label/List Header
Level 4: Body/Value
Level 5: Helper/Hint
```

Quy tắc:
- Mỗi màn có đúng 1 Level 1.
- Label muted, value rõ và đậm hơn.
- Không dùng cùng font size cho 2 cấp khác nhau trong cùng card.
- Không dùng text mô tả dài để giải thích UI; giao diện phải tự rõ nghĩa.

---

## 9. Iconography

- Dùng Material Icons qua MudBlazor.
- Không mix nhiều thư viện icon.
- Kích thước:
  - Inline: 18px
  - Button: 20px
  - Bottom nav: 22-24px
  - Feature icon/card icon: 28-32px
- Filled icon cho active state, outlined icon cho idle.
- Icon phải hỗ trợ nhận diện nhanh, không dùng để trang trí thừa.

---

## 10. Interaction & Animation

```css
transition: all 0.15s ease;
```

Quy tắc:
- UI action animation không quá 300ms.
- Press state có thể dùng `transform: scale(0.98)`.
- List loading dùng skeleton.
- Submit/sync dùng progress circular hoặc linear.
- Không animate mọi thứ nếu không giúp người dùng hiểu trạng thái.
- Sau thao tác thành công/lỗi phải có feedback rõ: snackbar, badge, inline message hoặc state change.

---

## 11. MudBlazor Mapping

| Design Token | MudBlazor Color | Hex |
|---|---|---|
| `--color-primary` | `Color.Primary` | `#1B4332` |
| `--color-primary-accent` | `Color.Secondary` | `#52B788` |
| `--color-success` | `Color.Success` | `#2E7D32` |
| `--color-warning` | `Color.Warning` | `#E65100` |
| `--color-danger` | `Color.Error` | `#C62828` |
| `--color-info` | `Color.Info` | `#01579B` |
| `--color-gray-500` | `Color.Default` | `#666666` |
| Dark card background | `Color.Dark` | `#1B4332` |

Ưu tiên dùng `Color.Primary`, `Color.Secondary`, `Color.Success`, `Color.Warning`, `Color.Error`, `Color.Info` thay vì hardcode hex trong Razor. Nếu cần CSS custom, dùng token trong `wwwroot/css/app-tokens.css`.

---

## 12. Checklist Trước Khi Hoàn Thành Màn Mobile

- [ ] Màn hình được thiết kế theo mobile dọc trước.
- [ ] Header không chiếm quá 15% chiều cao màn.
- [ ] Nội dung quan trọng nằm trong viewport đầu.
- [ ] Có đúng 1 screen title.
- [ ] Button/action có tap target tối thiểu 44px.
- [ ] Padding/gap dùng token linh hoạt hoặc `clamp()`, không fix theo một kích cỡ máy.
- [ ] Có xử lý safe-area cho header/bottom nav/action cố định.
- [ ] Dữ liệu nhiều trường được chuyển thành list/card dễ quét.
- [ ] Empty/loading/error states rõ ràng.
- [ ] Màu dùng token hoặc MudBlazor theme, không hardcode màu lạ.
- [ ] Spacing là bội số 4px.
- [ ] Text không tràn, không đè lên UI khác ở màn nhỏ.
- [ ] Dữ liệu số dùng tabular nums nếu cần so sánh.
- [ ] Component mới tuân theo cấu trúc UI + API caller, không chứa business logic client.

---

File này là nguồn tham chiếu UI mobile cho dự án. Khi thiết kế hoặc sửa giao diện, ưu tiên trải nghiệm app mobile dọc và khả năng thích ứng nhiều kích cỡ điện thoại.
