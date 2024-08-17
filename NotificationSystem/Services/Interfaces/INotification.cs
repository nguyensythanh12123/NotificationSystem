using NotificationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Services.Interfaces
{
    public interface INotification
    {
        Task Subscribe(int userId, Action<Post> onPostPublished);
        Task Publish(Post post);
    }
}
