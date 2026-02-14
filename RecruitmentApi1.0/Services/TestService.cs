using Serilog;

namespace RecruitmentApi1._0.Services;

public class TestService : ITestService
{
    public string GetMessage()
    {
        Log.Information("TestService.GetMessage викликано");
        return "Повідомлення з сервісу";
    }
}