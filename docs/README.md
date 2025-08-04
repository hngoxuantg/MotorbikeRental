# Hệ Thống Quản Lý Cửa Hàng Cho Thuê Xe Máy

## Tổng Quan Dự Án

### Mục Tiêu
Hệ thống giúp cửa hàng xe máy quản lý quá trình thuê – trả xe, hợp đồng, khách hàng, nhân viên và doanh thu. Dự án được phát triển với kiến trúc Clean Architecture và công nghệ hiện đại .NET 8, Vue 3.

### Tính Năng Chính

#### Quản Lý Xe Cho Thuê
- Quản lý loại xe, thương hiệu, tình trạng xe
- Lịch sử bảo trì và xử lý sự cố
- Hệ thống định giá theo giờ/ngày với khả năng giảm giá linh hoạt

#### Quản Lý Hợp Đồng & Khách Hàng
- Tạo hợp đồng thuê tự động
- Quản lý CCCD, phân loại khách hàng (mới/cũ), thanh toán

#### Quản Lý Nhân Viên & Phân Quyền
- Phân quyền: Admin, Manager, Receptionist
- Xác thực & phân quyền sử dụng JWT Token

#### Thống Kê & Báo Cáo
- Dashboard thống kê thời gian thực
- Báo cáo doanh thu, xe thuê nhiều nhất, trạng thái xe

## Kiến Trúc Dự Án

### Backend – Clean Architecture

```
MotorbikeRental/
├── Domain/               # Nghiệp vụ thuần (Entities, Interfaces, Enums)
├── Application/          # Use Cases (Services, DTOs, Validators, Mappers)
├── Infrastructure/       # Database, Repositories, Background Services
└── API/                  # REST API Controllers, Middlewares, Config
```

#### Chi Tiết Các Tầng

- **Domain**: Chứa business logic, định nghĩa các thực thể như `Motorbike`, `Contract`, `Discount`,... và các interface repository.
- **Application**: Tầng orchestrator, xử lý nghiệp vụ thông qua services, validate với FluentValidation và mapping dữ liệu bằng AutoMapper.
- **Infrastructure**: Kết nối database bằng Entity Framework Core, triển khai repository pattern, các dịch vụ nền như gửi email hoặc background job (`DiscountCleanupService`). 
- **API**: Cung cấp REST API, xử lý request-response, xác thực và ghi log.

Áp dụng đầy đủ nguyên lý **SOLID** và tuân thủ **Dependency Inversion Principle**.

### Frontend – Vue 3 + TypeScript

```
MotorbikeRental.UI/
├── api/             # API services
├── components/      # Vue Components chia theo vai trò
├── composables/     # Logic sử dụng lại (Vue Composition API)
├── router/          # Cấu hình routes và guards
├── stores/          # State management bằng Pinia
├── utils/           # Hàm tiện ích
└── views/           # Các trang chính (Admin, Receptionist,...)
```

Frontend đảm nhiệm giao diện người dùng, gọi API qua Axios, sử dụng Pinia để quản lý trạng thái.

## Công Nghệ Sử Dụng

### Backend
- ASP.NET Core 8 Web API
- Entity Framework Core 9
- SQL Server
- AutoMapper
- FluentValidation
- JWT Authentication
- SendGrid (Email)
- BackgroundService

### Frontend
- Vue 3 (Composition API)
- TypeScript + Vite
- Pinia, Vue Router
- Axios

## Cài Đặt & Chạy Dự Án

### Backend (.NET)
```bash
# Cập nhật appsettings.json với chuỗi kết nối SQL Server và JWT settings
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend (Vue 3)
```bash
npm install
npm run dev
```

## License

Distributed under the MIT License.