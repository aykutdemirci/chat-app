using ChatApp.Business.Helpers.EntityHelpers;
using ChatApp.Business.Interfaces;
using ChatApp.Business.Repositories;
using ChatApp.Cache;
using ChatApp.Dto;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Business.Factories
{
    public class ServiceFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private IServiceScope ServiceScope { get { return _serviceScopeFactory.CreateScope(); } }

        public ServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        
        /// <summary>
        /// Temel servisler dışında uygulama içerisinde kullanılan diğer servisleri kaydeder
        /// </summary>
        /// <param name="services">IServiceCollection örneği</param>
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ServiceFactory>();
            services.AddScoped(typeof(IRepository<,>), typeof(ChatDbRepository<,>));
            services.AddScoped<ChatRoomHelper>();
            services.AddScoped<MessageHelper>();
            services.AddScoped<RedisCacheService>();
        }

        /// <summary>
        /// ChatRooms Repository
        /// </summary>
        public IRepository<ChatRoom, ChatRoomViewModel> ChatRooms
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<IRepository<ChatRoom, ChatRoomViewModel>>();
            }
        }

        /// <summary>
        /// Messages Repository
        /// </summary>
        public IRepository<Message, MessageViewModel> Messages
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<IRepository<Message, MessageViewModel>>();
            }
        }

        /// <summary>
        /// ChatRoom ile ilgili işlemler yapılır.
        /// </summary>
        public ChatRoomHelper ChatRoomHelper
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<ChatRoomHelper>();
            }
        }

        /// <summary>
        /// Message ile ilgili işlemler yapılır.
        /// </summary>
        public MessageHelper MessageHelper
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<MessageHelper>();
            }
        }

        /// <summary>
        /// Uygulama içi ön bellek nesnesi
        /// </summary>
        public IDistributedCache Cache
        {
            get
            {
                return ServiceScope.ServiceProvider.GetService<IDistributedCache>();
            }
        }

        /// <summary>
        /// Uygulama içi ön bellekleme işlemlerini yapar.
        /// </summary>
        public RedisCacheService RedisCacheService
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<RedisCacheService>();
            }
        }
    }
}
