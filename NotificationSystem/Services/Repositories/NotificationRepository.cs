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
    public class NotificationRepository : INotification
    {
        private readonly ILogger<NotificationRepository> _logger;
        private Dictionary<int, List<Action<Post>>> _subscribers = new Dictionary<int, List<Action<Post>>>();

        public NotificationRepository(ILogger<NotificationRepository> logger)
        {
            _logger = logger;
        }

        public async Task Subscribe(int userId, Action<Post> notificationHandler)
        {
            try
            {
                _logger.LogInformation("Start NotificationSytemService/Subscribe");
                if (!_subscribers.ContainsKey(userId))
                {
                    _subscribers[userId] = new List<Action<Post>>();
                }
                _subscribers[userId].Add(notificationHandler);
            }
            catch (Exception ex)
            {
                _logger.LogError($"NotificationSytemService/Subscribe err: {ex.Message}");
                throw;
            }
        }
        public async Task Publish(Post post)
        {
            try
            {
                _logger.LogInformation("Start NotificationSytemService/Publish");
                foreach (var userId in _subscribers.Keys)
                {
                    if (_subscribers[userId].Any())
                    {
                        foreach (var handler in _subscribers[userId])
                        {
                            handler(post);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"NotificationSytemService/Publish err: {ex.Message}");
                throw;
            }

        }
    }
}
