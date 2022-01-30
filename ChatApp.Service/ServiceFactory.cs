using ChatApp.Business.Helpers.EntityHelpers;
using ChatApp.Business.Interfaces;
using ChatApp.Business.Repositories;
using ChatApp.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Service
{
    public class ServiceFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private IServiceScope ServiceScope { get { return _serviceScopeFactory.CreateScope(); } }

        public ServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ServiceFactory>();
            services.AddScoped(typeof(IRepository<,>), typeof(ChatDbRepository<,>));
            services.AddScoped<ChatRoomHelper>();
        }

        public IRepository<ChatRoom, ChatRoomViewModel> ChatRooms
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<IRepository<ChatRoom, ChatRoomViewModel>>();
            }
        }

        public IRepository<User, UserViewModel> Users
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<IRepository<User, UserViewModel>>(); ;
            }
        }

        public IRepository<Message, MessageViewModel> Messages
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<IRepository<Message, MessageViewModel>>();
            }
        }

        public ChatRoomHelper ChatRoomHelper
        {
            get
            {
                return ServiceScope.ServiceProvider.GetRequiredService<ChatRoomHelper>();
            }
        }
    }
}
