namespace QrCodeGenerator.Services;
using Blazored.LocalStorage;
using QrCodeGenerator.Models;

public class GlobalStateService
{
    private readonly ILocalStorageService _localStorageService;

    public GlobalStateService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<User> GetUserDataAsync()
    {
        return await _localStorageService.GetItemAsync<User>("user");
    }
    public async Task<string?> GetApiKeyAsync()
    {
        var user = await _localStorageService.GetItemAsync<User>("user");
        return user?.ApiKey;
    }
    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
