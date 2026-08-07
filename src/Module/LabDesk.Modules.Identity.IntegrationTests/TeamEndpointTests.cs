using System.Net;
using System.Net.Http.Json;
using LabDesk.Modules.Identity.Infrastructure.Persistence;
// (Nhớ using thêm namespace chứa entity Organization của bạn, ví dụ: using LabDesk.Modules.Identity.Domain.Organizations;)
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LabDesk.Modules.Identity.IntegrationTests;

public class UserEndpointTests : IClassFixture<IdentityIntegrationTestWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly IdentityIntegrationTestWebAppFactory _factory;

    public UserEndpointTests(IdentityIntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_WithValidPayload_ShouldSaveToDatabaseAndReturn201()
    {
        // 1. ARRANGE: Tạo sẵn một Organization trong DB test để tránh lỗi "Tổ chức không tồn tại"
        var organizationId = Guid.NewGuid();

        using var arrangeScope = _factory.Services.CreateScope();
        var arrangeDbContext = arrangeScope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        // Thêm Organization giả lập vào DB (Bạn điều chỉnh lại thuộc tính cho khớp với Entity Organization của bạn nhé)
        // arrangeDbContext.Organizations.Add(new Organization { Id = organizationId, Name = "Test Organization" });
        // await arrangeDbContext.SaveChangesAsync();

        var command = new
        {
            Email = "test@labdesk.com",
            Username = "testuser",
            Password = "Password123!",
            FullName = "Test User",
            OrganizationId = organizationId // Dùng ID vừa tạo ở trên
        };

        // 2. ACT: Gọi API
        var response = await _client.PostAsJsonAsync("/api/v1/identity/users", command);

        // Nếu lỗi BadRequest, in ra nội dung lỗi từ server để dễ nhìn
        if (response.StatusCode != HttpStatusCode.Created)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"API trả về lỗi {response.StatusCode}: {errorContent}");
        }

        // 3. ASSERT: Kiểm tra HTTP 201 Created
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        // 4. ASSERT DATABASE: Kiểm tra bảng Users trong DB
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == "test@labdesk.com");
        Assert.NotNull(userInDb);
    }
}