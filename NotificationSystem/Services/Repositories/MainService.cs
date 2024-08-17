using Microsoft.Extensions.Logging;
using NotificationSystem.Models;
using NotificationSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Services.Repositories
{
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly INotification _notification;

        public MainService(ILogger<MainService> logger, INotification notification)
        {
            _logger = logger;
            _notification = notification;
        }

        public async Task Run()
        {
            _logger.LogInformation("______________START____________");

            var users = new List<User>
            {
                new User { Id = 1, Name = "ThanhNS", FollowedUserIds = new List<int> { 2 } },
                new User { Id = 2, Name = "TamLH", FollowedUserIds = new List<int> { 1 } }
            };

            var posts = new List<Post>
            {
                new Post { Id = 1, UserId = 1, Content = "Hello World!", Timestamp = DateTime.Now }
            };

            foreach (var user in users)
            {
                await _notification.Subscribe(user.Id, (post) =>
                {
                    _logger.LogInformation($"User {user.Name} received notification: {post.Content} by {users.FirstOrDefault(u => u.Id == post.UserId).Name}");
                });
            }

            foreach (var post in posts)
            {
                await _notification.Publish(post);
            }
            _logger.LogInformation("______________END____________");
        }
    }
}
