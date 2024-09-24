namespace Demo.Interfaces
{
    public interface IRedisCacheService
    {
        Task<string> GetValueAsync(string key);
        Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null);
        Task Clear(string key);
        void ClearAll();
        Task<bool> SetAddAsync(string key, string value);
        Task<IEnumerable<string>> SetMembersAsync(string key);
    }
}