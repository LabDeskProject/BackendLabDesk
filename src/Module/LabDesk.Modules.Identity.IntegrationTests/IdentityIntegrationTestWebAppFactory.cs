using LabDesk.Modules.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LabDesk.Modules.Identity.IntegrationTests;

public class IdentityIntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Trỏ trực tiếp vào Local SQL Server trên máy bạn (hoặc dùng LocalDB: Server=(localdb)\mssqllocaldb;)
    private const string ConnectionString = "Data Source=.\\SQLEXPRESS;Database=LabDeskDb;Integrated Security = True; Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Xóa DbContext cũ cấu hình trong app chính
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<IdentityDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // 2. Cấu hình trỏ vào DB Test dưới local máy bạn
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(LabDesk.Modules.Identity.Application.Commands.CreateUser.CreateUserCommand).Assembly);
            });

            // 👉 3. BỔ SUNG THÊM: Đăng ký các dịch vụ / handlers của module Identity 
            // (Hãy thay dòng dưới bằng hàm extension thực tế trong dự án của bạn, ví dụ: services.AddIdentityModule() hoặc tương tự)
            // services.AddApplication(); // Hoặc đăng ký thủ công MediatR nếu chưa có extension tổng
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        // Xóa sạch DB cũ (nếu có) và tạo mới hoàn toàn sạch sẽ trước mỗi lần chạy test suite
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        // Dọn dẹp xóa luôn DB test sau khi test xong (tùy chọn)
        await dbContext.Database.EnsureDeletedAsync();
    }
}