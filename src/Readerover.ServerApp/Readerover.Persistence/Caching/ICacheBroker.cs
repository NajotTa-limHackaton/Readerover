namespace Readerover.Persistence.Caching;

public interface ICacheBroker
{
    /// <summary>
    /// key bo'yicha cachedan get qilish
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    ValueTask<T> GetAsync<T>(string key);

    /// <summary>
    /// cache ga saqlash , key va value bo'yicha
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    ValueTask SetAsync<T>(string key, T value);


    /// <summary>
    /// key orqali keshdan o'chirish
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ValueTask DeleteAsync(string key);


    /// <summary>
    /// cachedan tekshirib bo'lsa cachedan oladi
    /// aks holda cachega saqlab keyin qaytaradi
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    ValueTask<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory);
}
