using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Caching;

namespace ApiCaching.Services;

public class CacheService : ICacheService
{
   private ObjectCache _memoryCache = MemoryCache.Default;
    public T GetData<T>(string key)
    {
        try
        {
            T item = (T)_memoryCache.Get(key);
            return item;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public object Remove(string key)
    {
        var res = true;
        try
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                var result = _memoryCache.Remove(key);
            } else
            {
                res = false;
            }
        }
        catch (Exception)
        {
            res = false;
        }

        return res;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        bool res = true;

        try
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                _memoryCache.Set(key, value, expirationTime);
            } else
            {
                res = false;
            }
        }
        catch (Exception)
        {

            res = false;
        }

        return res;
    }
}
