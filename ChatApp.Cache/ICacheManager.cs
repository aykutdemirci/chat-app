using System.Threading.Tasks;

namespace ChatApp.Cache
{
    public interface ICacheManager
    {
        void Set<T>(string key, T model);

        T Get<T>(string key);

        void Remove(string key);
    }
}
