using Microsoft.Extensions.Logging;
using Moq;
using NotificationSystem;
using NotificationSystem.Models;
using NotificationSystem.Services.Interfaces;
using NotificationSystem.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaNotificationSystem.Tests
{
    public class MainServiceTest
    {
        private readonly Mock<ILogger<MainService>> _mockLogger;
        private readonly Mock<INotification> _mockNotification;
        private readonly MainService _mainService;

        public MainServiceTest()
        {
            _mockLogger = new Mock<ILogger<MainService>>();
            _mockNotification = new Mock<INotification>();
            _mainService = new MainService(_mockLogger.Object, _mockNotification.Object);
        }
        [Fact]
        public async Task Run_ShouldNotifySubscribersCorrectly()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "ThanhNS", FollowedUserIds = new List<int> { 2 } };
            var user2 = new User { Id = 2, Name = "TamLH", FollowedUserIds = new List<int> { 1 } };

            var users = new List<User> { user1, user2 };

            var post = new Post { Id = 1, UserId = 1, Content = "Hello World!", Timestamp = DateTime.Now };
            var posts = new List<Post> { post };

            var user1Notifications = new List<string>();
            var user2Notifications = new List<string>();

            _mockNotification.Setup(n => n.Subscribe(It.Is<int>(id => id == user1.Id), It.IsAny<Action<Post>>()))
                            .Callback<int, Action<Post>>((id, callback) =>
                            {
                                callback(post);
                                user1Notifications.Add(post.Content);
                            });

            _mockNotification.Setup(n => n.Subscribe(It.Is<int>(id => id == user2.Id), It.IsAny<Action<Post>>()))
                             .Callback<int, Action<Post>>((id, callback) =>
                             {
                                 callback(post);
                                 user2Notifications.Add(post.Content);
                             });

            // Act
            await _mainService.Run();

            // Assert
            Assert.Single(user1Notifications);
            Assert.Equal("Hello World!", user1Notifications.First());

            Assert.Empty(user2Notifications);
        }
    }
}
