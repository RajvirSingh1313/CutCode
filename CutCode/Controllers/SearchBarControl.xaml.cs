﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CutCode
{
    /// <summary>
    /// Interaction logic for SearchBarControl.xaml
    /// </summary>
    public partial class SearchBarControl : UserControl
    {
        public SearchBarControl()
        {
            InitializeComponent();

            SearchCommand?.Execute(null);

            searchBox.SetBinding(TextProperty, new Binding("Text"));
            searchBtn.Visibility = Visibility.Hidden;
            searchBtn.Click += ClearClicked;
            searchBox.TextChanged += TextChanged;

        }

        #region Text property
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SearchBarControl),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TextPropertyChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set 
            {
                SetValue(TextProperty, value);
            } 
        }
        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SearchBarControl ctrl || e.NewValue is not string) return;
            ctrl.searchBox.Text = (string)e.NewValue;
        }
        #endregion

        #region Placeholder Text property
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(SearchBarControl),
                new PropertyMetadata("", PlaceholderPropertyChanged));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        private static void PlaceholderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SearchBarControl ctrl || e.NewValue is not string) return;
            ctrl.placeholder.Text = (string)e.NewValue;
        }
        #endregion

        #region PlaceholderText color property
        public static readonly DependencyProperty PlaceholderTextColorProperty =
            DependencyProperty.Register("PlaceholderTextColor", typeof(SolidColorBrush), typeof(SearchBarControl),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), PlaceholderTextColorPropertyChanged));

        public SolidColorBrush PlaceholderTextColor
        {
            get => (SolidColorBrush)GetValue(PlaceholderTextColorProperty);
            set => SetValue(PlaceholderTextColorProperty, value);
        }
        private static void PlaceholderTextColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SearchBarControl ctrl || e.NewValue is not SolidColorBrush) return;
            ctrl.placeholder.Foreground = (SolidColorBrush)e.NewValue;
        }
        #endregion

        #region Background color property
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(SearchBarControl),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), BackgroundColorPropertyChanged));

        public SolidColorBrush BackgroundColor
        {
            get => (SolidColorBrush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        private static void BackgroundColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SearchBarControl ctrl || e.NewValue is not SolidColorBrush) return;
            ctrl.searchBarFrame.Background = (SolidColorBrush)e.NewValue;
        }
        #endregion

        #region Text color property
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(SearchBarControl),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), TextColorPropertyChanged));

        public SolidColorBrush TextColor
        {
            get => (SolidColorBrush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        private static void TextColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SearchBarControl ctrl || e.NewValue is not SolidColorBrush) return;
            ctrl.searchBox.Foreground = (SolidColorBrush)e.NewValue;
        }
        #endregion

        #region ButtonHover color property
        public static readonly DependencyProperty ButtonHoverColorProperty =
            DependencyProperty.Register("ButtonHoverColor", typeof(SolidColorBrush), typeof(SearchBarControl),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ButtonHoverColorPropertyChanged));

        public SolidColorBrush ButtonHoverColor
        {
            get => (SolidColorBrush)GetValue(ButtonHoverColorProperty);
            set => SetValue(ButtonHoverColorProperty, value);
        }
        private static void ButtonHoverColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SearchBarControl ctrl || e.NewValue is not SolidColorBrush) return;
            ctrl.searchBtn.BorderBrush = (SolidColorBrush)e.NewValue;
        }
        #endregion

        #region Theme property
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(bool), typeof(SearchBarControl),
                new PropertyMetadata(true, ThemePropertyChanged));

        public bool Theme
        {
            get => (bool)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        private static void ThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SearchBarControl ctrl || e.NewValue is not bool) return;
            ctrl.exitBtnImage.Source = new BitmapImage(new Uri((bool)e.NewValue ? @"../Resources/Images/Icons/exit_black.png" : @"../Resources/Images/Icons/exit_white.png", UriKind.Relative));
        }
        #endregion

        #region Search Command property
        
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchBarControl),
                new PropertyMetadata(null));

        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }
        #endregion

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = searchBox.Text;
            if (string.IsNullOrEmpty(searchBox.Text)) searchBtn.Visibility = Visibility.Hidden;
            else searchBtn.Visibility = Visibility.Visible;
        }

        private void ClearClicked(object sender, EventArgs e)
        {
            searchBox.Text = "";
        }
    }
}
