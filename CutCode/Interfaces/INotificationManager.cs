﻿using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CutCode
{
    public interface INotificationManager
    {
        event EventHandler ShowNotification;
        void CreateNotification(string message, int delay);
        void CloseNotification(NotifyObject notification);
        event EventHandler OnCloseNotification;
    }

    public class NotificationManager : INotificationManager
    {
        public event EventHandler ShowNotification;
        public void CreateNotification(string message, int delay)
        {
            var notify = new NotifyObject(){ Message = message, Delay= delay};
            ShowNotification?.Invoke(notify, EventArgs.Empty);
        }

        public event EventHandler OnCloseNotification;
        public void CloseNotification(NotifyObject notification) => OnCloseNotification?.Invoke(notification, EventArgs.Empty);
    }
}
