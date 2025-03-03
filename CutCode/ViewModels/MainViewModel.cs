﻿using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace CutCode
{
    public class MainViewModel : Screen
    {

        private readonly IThemeService _themeService;
        private readonly IWindowManager windowManager;
        private readonly IPageService pageService;
        private readonly INotificationManager notifyManager;
        private readonly IApiManager apiManager;

        public static List<System.Object> Pages;
        public ObservableCollection<SideBarBtnModel> sideBarBtns { get; set; }

        public MainViewModel(IWindowManager _windowManager,
                            IThemeService themeService, 
                            IPageService _pageService, 
                            IDataBase _dataBase, 
                            INotificationManager _notifyManager,
                            IApiManager _apiManager)
        {
            windowManager = _windowManager;

            notifyManager = _notifyManager;
            notifyManager.ShowNotification += showNotification;
            notifyManager.OnCloseNotification += exitNotification;
            notificationList = new ObservableCollection<NotifyObject>();

            _themeService = themeService;
            _themeService.ThemeChanged += ThemeChanged;
            _themeService.IsLightTheme = _dataBase.isLightTheme;

            pageService = _pageService;
            pageService.PageChanged += PageChanged;
            pageService.PageRemoteChanged += PageRemoteChanged;

            apiManager = _apiManager;

            sideBarBtns = new ObservableCollection<SideBarBtnModel>();

            sideBarBtns.Add(new SideBarBtnModel("Home", _themeService));
            sideBarBtns.Add(new SideBarBtnModel("Add", _themeService));
            sideBarBtns.Add(new SideBarBtnModel("Favourite", _themeService));
            sideBarBtns.Add(new SideBarBtnModel("Share", _themeService));
            sideBarBtns.Add(new SideBarBtnModel("Settings", _themeService));
            sideBarBtns[0].background = _themeService.IsLightTheme ? ColorCon.Convert("#FCFCFC") : ColorCon.Convert("#36393F");


            Pages = new List<System.Object>() {    new HomeViewModel(_themeService, pageService, _dataBase), 
                                            new AddViewModel(_themeService, pageService, _dataBase), 
                                            new FavViewModel(_themeService, pageService, _dataBase),
                                            new ShareViewModel(_themeService, pageService, _dataBase, apiManager, notifyManager),
                                            new SettingViewModel(_themeService, _dataBase, notifyManager) };
            pageService.Page = Pages[0];
        }

        private void ThemeChanged(object sender, EventArgs e)
        {
            backgroundColor = _themeService.IsLightTheme ? ColorCon.Convert("#FCFCFC") : ColorCon.Convert("#36393F");
            titleBarColor = _themeService.IsLightTheme ? ColorCon.Convert("#E3E5E8") : ColorCon.Convert("#202225");
            SideBarColor = _themeService.IsLightTheme ? ColorCon.Convert("#F2F3F5") : ColorCon.Convert("#2A2E33");
            mainTextColor = _themeService.IsLightTheme ? ColorCon.Convert("#0B0B13") : ColorCon.Convert("#94969A");

            exitImage = _themeService.IsLightTheme ? "../Resources/Images/Icons/exit_black.png" : "../Resources/Images/Icons/exit_white.png";
            minImage = _themeService.IsLightTheme ? "../Resources/Images/Icons/min_black.png" : "../Resources/Images/Icons/min_white.png";
            maxImage = _themeService.IsLightTheme ? "../Resources/Images/Icons/max_black.png" : "../Resources/Images/Icons/max_white.png";

            titlebarBtnsHoverColor = _themeService.IsLightTheme ? ColorCon.Convert("#D0D1D2") : ColorCon.Convert("#373737");
        }

        private System.Object _currentPage;
        public System.Object currentPage
        {
            get => _currentPage;
            set { SetAndNotify(ref _currentPage, value); }
        }

        private void PageChanged(object sender, EventArgs e) => currentPage = pageService.Page;

        private void PageRemoteChanged(object sender, EventArgs e)
        {
            var page = sender as string;
            ChangePageCommand(page);
        }

        public void ChangePageCommand(string selected_item)
        {
            int ind = 0;
            foreach (var btn in sideBarBtns)
            {
                if (btn.toolTipText != selected_item) btn.background = ColorCon.Convert("#00FFFFFF");
                else ind = sideBarBtns.IndexOf(btn);
            }

            sideBarBtns[ind].background = _themeService.IsLightTheme ? ColorCon.Convert("#FCFCFC") : ColorCon.Convert("#36393F");
            if (currentPage != Pages[ind]) 
            {
                pageService.Page = Pages[ind];
                if(selected_item == "Share")
                {
                    if (!apiManager.IsInternetAvailable())
                        notifyManager.CreateNotification("You need internet connection to share codes!", 10);
                }
            }
        }
        #region Color
        private SolidColorBrush _backgroundColor;
        public SolidColorBrush backgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (value != _backgroundColor)
                {
                    SetAndNotify(ref _backgroundColor, value);
                }
            }
        }

        private SolidColorBrush _titleBarColor;
        public SolidColorBrush titleBarColor
        {
            get => _titleBarColor;
            set
            {
                if (value != _titleBarColor)
                {
                    SetAndNotify(ref _titleBarColor, value);
                }
            }
        }

        private SolidColorBrush _sideBarColor;
        public SolidColorBrush SideBarColor
        {
            get => _sideBarColor;
            set
            {
                if (value != _sideBarColor)
                {
                    SetAndNotify(ref _sideBarColor, value);
                }
            }
        }

        private SolidColorBrush _mainTextColor;
        public SolidColorBrush mainTextColor
        {
            get => _mainTextColor;
            set
            {
                if (value != _mainTextColor)
                {
                    SetAndNotify(ref _mainTextColor, value);
                }
            }
        }

        private SolidColorBrush _titlebarBtnsHoverColor;
        public SolidColorBrush titlebarBtnsHoverColor
        {
            get => _titlebarBtnsHoverColor;
            set
            {
                if (value != _titlebarBtnsHoverColor)
                {
                    SetAndNotify(ref _titlebarBtnsHoverColor, value);
                }
            }
        }
        #endregion

        private string _exitImage;
        public string exitImage
        {
            get => _exitImage;
            set
            { SetAndNotify(ref _exitImage, value); }
        }

        private string _maxImage;
        public string maxImage
        {
            get => _maxImage;
            set
            { SetAndNotify(ref _maxImage, value); }
        }

        private string _minImage;
        public string minImage
        {
            get => _minImage;
            set
            { SetAndNotify(ref _minImage, value); }
        }

        #region NotificationDialogView
        private ObservableCollection<NotifyObject> _notificationList;
        public ObservableCollection<NotifyObject> notificationList
        {
            get => _notificationList;
            set => SetAndNotify(ref _notificationList, value);
        }

        private List<NotifyObject> WaitingNotifications = new List<NotifyObject>();
        private List<LiveNotification> liveNotifications = new List<LiveNotification>();
        private void showNotification(object sender, EventArgs e)
        {
            var notification = sender as NotifyObject;

            var notifcationViewModel = new NotificationDialogViewModel(_themeService, notifyManager, notification);
            notification.View = (Object)notifcationViewModel;

            if(notificationList.Count > 2)
            {
                WaitingNotifications.Add(notification);
            }
            else
            {
                notificationList.Add(notification);

                var closeTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(notification.Delay),
                    IsEnabled = true
                };
                liveNotifications.Add(new LiveNotification() { timer = closeTimer, notification = notification});
                closeTimer.Tick += CloseNotification;
            }
        }

        private void exitNotification(object sender, EventArgs e)
        {
            var notification = sender as NotifyObject;
            var liveNotification = new LiveNotification();
            foreach (var _liveNotification in liveNotifications)
            {
                if (_liveNotification.notification == notification)
                {
                    liveNotification = _liveNotification;
                    break;
                }
            }
            notificationList.Remove(notification);
            UpdateNotification();
            liveNotification.timer.Stop();
            liveNotifications.Remove(liveNotification);
        }

        private void CloseNotification(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            var liveNotification = new LiveNotification();
            foreach(var _liveNotification in liveNotifications)
            {
                if(_liveNotification.timer == timer)
                {
                    liveNotification = _liveNotification;
                    break;
                } 
            }

            notificationList.Remove(liveNotification.notification);
            liveNotifications.Remove(liveNotification);
            UpdateNotification();
            timer.Stop();
        }

        private void UpdateNotification()
        {
            if (WaitingNotifications.Count > 0)
            {
                for (int i = 0; i < (3 - notificationList.Count); i++)
                {
                    if (WaitingNotifications.Count == 0) break;

                    var notification = WaitingNotifications[i];
                    WaitingNotifications.RemoveAt(i);
                    notifyManager.CreateNotification(notification.Message, notification.Delay);
                }
            }
        }
        #endregion
    }
}
