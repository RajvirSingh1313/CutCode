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

namespace CutCode
{
    public class MainViewModel : Screen
    {
        //private List<Button> leftBarBtns;
        private List<Object> Pages;
        private readonly IThemeService _themeService;
        private IWindowManager windowManager;
        public ObservableCollection<SideBarBtnModel> sideBarBtns { get; set; }
        public MainViewModel(IWindowManager _windowManager, IThemeService themeService)
        {
            _themeService = themeService;
            windowManager = _windowManager;
            _themeService.ThemeChanged += ThemeChanged;
            _themeService.IsLightTheme = true;

            sideBarBtns = new ObservableCollection<SideBarBtnModel>();

            // there should be some kind of condition here
            sideBarBtns.Add(new SideBarBtnModel("Home", _themeService));
            sideBarBtns.Add(new SideBarBtnModel("Add", _themeService));
            sideBarBtns.Add(new SideBarBtnModel("Favourite", _themeService));
            sideBarBtns.Add(new SideBarBtnModel("Settings", _themeService));


            Pages = new List<Object>() { new HomePage(), new AddPage(), new FavPage(), new SettingView() };
            currentPage = Pages[0];
        }
        private void ThemeChanged(object sender, EventArgs e)
        {

            backgroundColor = _themeService.IsLightTheme ? (Color)ColorConverter.ConvertFromString("#FCFCFC") : (Color)ColorConverter.ConvertFromString("#36393F");
            titleBarColor = _themeService.IsLightTheme ? (Color)ColorConverter.ConvertFromString("#E3E5E8") : (Color)ColorConverter.ConvertFromString("#202225");
            SideBarColor = _themeService.IsLightTheme ? (Color)ColorConverter.ConvertFromString("#D6D7DB") : (Color)ColorConverter.ConvertFromString("#2A2E33");
            mainTextColor = _themeService.IsLightTheme ? (Color)ColorConverter.ConvertFromString("#0B0B13") : (Color)ColorConverter.ConvertFromString("#94969A");

            exitImage = _themeService.IsLightTheme ? "../Resources/Images/Icons/exit_black.png" : "../Resources/Images/Icons/exit_white.png";
            minImage = _themeService.IsLightTheme ? "../Resources/Images/Icons/min_black.png" : "../Resources/Images/Icons/min_white.png";
            maxImage = _themeService.IsLightTheme ? "../Resources/Images/Icons/max_black.png" : "../Resources/Images/Icons/max_white.png";

            titlebarBtnsHoverColor = _themeService.IsLightTheme ? (Color)ColorConverter.ConvertFromString("#D0D1D2") : (Color)ColorConverter.ConvertFromString("#373737");
        }

        private Object _currentPage;
        public Object currentPage
        {
            get => _currentPage;
            set { SetAndNotify(ref _currentPage, value); }
        }

        public void ChangePageCommand(string selected_item)
        {
            //_themeService.IsLightTheme = _themeService.IsLightTheme ? false : true;
            int ind = 0;
            foreach (var btn in sideBarBtns)
            {
                if (btn.toolTipText != selected_item) btn.background = (Color)ColorConverter.ConvertFromString("#00FFFFFF");
                else ind = sideBarBtns.IndexOf(btn);
            }
            sideBarBtns[ind].background = _themeService.IsLightTheme ? (Color)ColorConverter.ConvertFromString("#FCFCFC") : (Color)ColorConverter.ConvertFromString("#36393F");
            currentPage = windowManager.ShowWindow(Pages[ind]);
            if (currentPage != Pages[ind]) currentPage = Pages[ind];
        }

        private Color _backgroundColor;
        public Color backgroundColor
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

        private Color _titleBarColor;
        public Color titleBarColor
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

        private Color _sideBarColor;
        public Color SideBarColor
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

        private Color _mainTextColor;
        public Color mainTextColor
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

        private Color _titlebarBtnsHoverColor;
        public Color titlebarBtnsHoverColor
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
    }
        
}