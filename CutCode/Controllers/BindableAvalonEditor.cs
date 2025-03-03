﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CutCode
{
    public class BindableAvalonEditor : ICSharpCode.AvalonEdit.TextEditor, INotifyPropertyChanged
    {
        /// <summary>
        /// A bindable Text property
        /// </summary>

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Text = Document.Text; 
            
            double vertOffset = (this.TextArea.TextView.DefaultLineHeight) * this.Document.LineCount;
            this.ScrollToVerticalOffset(vertOffset);
        }
        public new string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                RaisePropertyChanged("Text");
            }
        }

        /// <summary>
        /// The bindable text property dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(BindableAvalonEditor),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = "",
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnDependencyPropertyChanged
    }
            );

        protected static void OnDependencyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = (BindableAvalonEditor)obj;

            if (target.Document != null)
            {
                var caretOffset = target.CaretOffset;
                var newValue = args.NewValue;

                if (newValue == null)
                {
                    newValue = "";
                }

                target.Document.Text = (string)newValue;
                target.CaretOffset = Math.Min(caretOffset, newValue.ToString().Length);
            }
        }

        /// <summary>
        /// Raises a property changed event
        /// </summary>
        /// <param name="property">The name of the property that updates</param>
        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
