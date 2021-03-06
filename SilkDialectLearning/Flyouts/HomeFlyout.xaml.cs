﻿using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace SilkDialectLearning.Flyouts
{
    /// <summary>
    /// Interaction logic for HomeFlyout.xaml
    /// </summary>
    [ContentProperty("OverlayContent")]
    public partial class HomeFlyout : Flyout, IUriContext, INotifyPropertyChanged
    {
        MainViewModel mainWindowViewModel;
        public bool IsBackEntry { get; set; }
        public HomeFlyout()
        {
            InitializeComponent();
            this.Loaded += HomeFlyout_Loaded;
        }
        private bool loaded;
        void HomeFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded && !DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                PageStatus = new ObservableCollection<object>();
                mainWindowViewModel = (sender as Flyout).DataContext as MainViewModel;
                if (mainWindowViewModel != null)
                    this.Navigate(new Navigation.LanguagesPage(this, mainWindowViewModel));
                uiPageStatus.DataContext = this;
                PART_Frame.Navigated += PART_Frame_Navigated;
                PART_Frame.Navigating += PART_Frame_Navigating;
                PART_Frame.NavigationFailed += PART_Frame_NavigationFailed;
                PART_Frame.NavigationProgress += PART_Frame_NavigationProgress;
                PART_Frame.NavigationStopped += PART_Frame_NavigationStopped;
                PART_Frame.LoadCompleted += PART_Frame_LoadCompleted;
                PART_Frame.FragmentNavigation += PART_Frame_FragmentNavigation;
                PART_BackButton.Click += PART_BackButton_Click;
                loaded = true;
            }
        }

        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanGoForward)
                GoForward();
        }

        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_FragmentNavigation(object sender, FragmentNavigationEventArgs e)
        {
            if (FragmentNavigation != null)
                FragmentNavigation(this, e);
        }
        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (LoadCompleted != null)
                LoadCompleted(this, e);
        }

        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationStopped(object sender, NavigationEventArgs e)
        {
            if (NavigationStopped != null)
                NavigationStopped(this, e);
        }
        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationProgress(object sender, NavigationProgressEventArgs e)
        {
            if (NavigationProgress != null)
                NavigationProgress(this, e);
        }
        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (NavigationFailed != null)
                NavigationFailed(this, e);
        }
        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (Navigating != null)
                Navigating(this, e);
        }
        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanGoBack)
                GoBack();
        }

        //[System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_Navigated(object sender, NavigationEventArgs e)
        {  
            PART_Title.Content = ((Page)PART_Frame.Content).Title;
            mainWindowViewModel.Name = null;
            mainWindowViewModel.Description = null;
            (this as IUriContext).BaseUri = e.Uri;

            PageContent = PART_Frame.Content;
            PART_BackButton.IsEnabled = CanGoBack ? true : false;

            //This statiment finds ListBox's from content and sets null to selected item
            if (IsBackEntry)
            {
                if (PART_Frame.Content != null)
                {
                    var page = PART_Frame.Content as Page;
                    ListBox listBox = page.FindChildren<ListBox>().FirstOrDefault();
                    listBox.SelectedIndex = -1;
                }
                IsBackEntry = false;
            }

            if (Navigated != null)
                Navigated(this, e);
            
        }

        public static readonly DependencyProperty OverlayContentProperty = DependencyProperty.Register("OverlayContent", typeof(object), typeof(HomeFlyout));

        public object OverlayContent
        {
            get { return GetValue(OverlayContentProperty); }
            set { SetValue(OverlayContentProperty, value); }
        }

        public static readonly DependencyProperty PageContentProperty = DependencyProperty.Register("PageContent", typeof(object), typeof(HomeFlyout));

        public object PageContent
        {
            get { return GetValue(PageContentProperty); }
            private set { SetValue(PageContentProperty, value); }
        }

        /// <summary>
        /// Gets an IEnumerable that you use to enumerate the entries in back navigation history for a NavigationWindow.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.ForwardStack"/>
        public IEnumerable ForwardStack { get { return PART_Frame.ForwardStack; } }
        /// <summary>
        /// Gets an IEnumerable that you use to enumerate the entries in back navigation history for a NavigationWindow.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.BackStack"/>
        public IEnumerable BackStack { get { return PART_Frame.BackStack; } }

        /// <summary>
        /// Gets the NavigationService that is used by this MetroNavigationWindow to provide navigation services to its content.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.NavigationService"/>
        public NavigationService NavigationService { get { return PART_Frame.NavigationService; } }
        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in back navigation history.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.CanGoBack"/>
        public bool CanGoBack { get { return PART_Frame.CanGoBack; } }
        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in forward navigation history.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.CanGoForward"/>
        public bool CanGoForward { get { return PART_Frame.CanGoForward; } }
        /// <summary>
        /// Gets or sets the base uniform resource identifier (URI) of the current context.
        /// </summary>
        /// <see cref="IUriContext.BaseUri"/>
        Uri IUriContext.BaseUri { get; set; }

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) of the current content, or the URI of new content that is currently being navigated to.  
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Source"/>
        public Uri Source { get { return PART_Frame.Source; } set { PART_Frame.Source = value; } }

        /// <summary>
        /// Adds an entry to back navigation history that contains a CustomContentState object.
        /// </summary>
        /// <param name="state">A CustomContentState object that represents application-defined state that is associated with a specific piece of content.</param>
        /// <see cref="System.Windows.Navigation.NavigationWindow.AddBackEntry"/>
        //[System.Diagnostics.DebuggerNonUserCode]
        public void AddBackEntry(CustomContentState state)
        {
            PART_Frame.AddBackEntry(state);
        }
        /// <summary>
        /// Removes the most recent journal entry from back history.
        /// </summary>
        /// <returns>The most recent JournalEntry in back navigation history, if there is one.</returns>
        /// <see cref="System.Windows.Navigation.NavigationWindow.RemoveBackEntry"/>
        //[System.Diagnostics.DebuggerNonUserCode]
        public JournalEntry RemoveBackEntry()
        {
            return PART_Frame.RemoveBackEntry();
        }

        /// <summary>
        /// Navigates to the most recent item in back navigation history.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.GoBack"/>
        //[System.Diagnostics.DebuggerNonUserCode]
        public void GoBack()
        {
            IsBackEntry = true;
            PART_Frame.GoBack();
        }

        /// <summary>
        /// Navigates to the most recent item in forward navigation history.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.GoForward"/>
        //[System.Diagnostics.DebuggerNonUserCode]
        public void GoForward()
        {
            PART_Frame.GoForward();
        }

        //public Dictionary<Page, object> PageStatus = new Dictionary<Page, object>();

        public ObservableCollection<object> PageStatus { get; set; }
        /// <summary>
        /// Navigates asynchronously to content that is contained by an object.
        /// </summary>
        /// <param name="content">An Object that contains the content to navigate to.</param>
        /// <returns>true if a navigation is not canceled; otherwise, false.</returns>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigate(Object)"/>
        //[System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Object content)
        {   
            return PART_Frame.Navigate(content);
        }
        /// <summary>
        /// Navigates asynchronously to content that is specified by a uniform resource identifier (URI).
        /// </summary>
        /// <param name="source">A Uri object initialized with the URI for the desired content.</param>
        /// <returns>true if a navigation is not canceled; otherwise, false.</returns>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigate(Uri)"/>
        //[System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Uri source)
        {
            return PART_Frame.Navigate(source);
        }
        /// <summary>
        /// Navigates asynchronously to content that is contained by an object, and passes an object that contains data to be used for processing during navigation.
        /// </summary>
        /// <param name="content">An Object that contains the content to navigate to.</param>
        /// <param name="extraData">A Object that contains data to be used for processing during navigation.</param>
        /// <returns>true if a navigation is not canceled; otherwise, false.</returns>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigate(Object, Object)"/>
        //[System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Object content, Object extraData)
        {
            return PART_Frame.Navigate(content, extraData);
        }
        /// <summary>
        /// Navigates asynchronously to source content located at a uniform resource identifier (URI), and pass an object that contains data to be used for processing during navigation.
        /// </summary>
        /// <param name="source">A Uri object initialized with the URI for the desired content.</param>
        /// <param name="extraData">A Object that contains data to be used for processing during navigation.</param>
        /// <returns>true if a navigation is not canceled; otherwise, false.</returns>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigate(Uri, Object)"/>
        // [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Uri source, Object extraData)
        {
            return PART_Frame.Navigate(source, extraData);
        }
        /// <summary>
        /// Stops further downloading of content for the current navigation request.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.StopLoading"/>
        //[System.Diagnostics.DebuggerNonUserCode]
        public void StopLoading()
        {
            PART_Frame.StopLoading();
        }

        /// <summary>
        /// Occurs when navigation to a content fragment begins, which occurs immediately, if the desired fragment is in the current content, or after the source XAML content has been loaded, if the desired fragment is in different content.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.FragmentNavigation"/>
        public event FragmentNavigationEventHandler FragmentNavigation;
        
        /// <summary>
        /// Occurs when a new navigation is requested.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigating"/>
        public event NavigatingCancelEventHandler Navigating;
       
        /// <summary>
        /// Occurs when an error is raised while navigating to the requested content.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.NavigationFailed"/>
        public event NavigationFailedEventHandler NavigationFailed;
        
        /// <summary>
        /// Occurs periodically during a download to provide navigation progress information.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.NavigationProgress"/>
        public event NavigationProgressEventHandler NavigationProgress;
        
        /// <summary>
        /// Occurs when the StopLoading method is called, or when a new navigation is requested while a current navigation is in progre
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.NavigationStopped"/>
        public event NavigationStoppedEventHandler NavigationStopped;
       
        /// <summary>
        /// Occurs when the content that is being navigated to has been found, and is available from the PageContent property, although it may not have completed loading
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigated"/>
        public event NavigatedEventHandler Navigated;
        
        /// <summary>
        /// Occurs when content that was navigated to has been loaded, parsed, and has begun rendering.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.LoadCompleted"/>
        public event LoadCompletedEventHandler LoadCompleted;

        private void PART_AddButton_Click(object sender, RoutedEventArgs e)
        {
            MyPopup.IsOpen = true;
        }

        private void uiPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = (sender as Grid);
            RoutedEventHandler affirmativeHandler = null;
            KeyEventHandler affirmativeKeyHandler = null;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Focus();
                PART_Name.Focus();
            }));


            affirmativeKeyHandler = (s, ea) =>
            {
                if (ea.Key == Key.Enter)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        PART_SaveButton.Focus();
                    }));
                }
            };

            affirmativeHandler = (s, ea) =>
            {
                MyPopup.IsOpen = false;
                ea.Handled = true;
            };

            PART_SaveButton.KeyDown += affirmativeKeyHandler;
            PART_Name.KeyDown += affirmativeKeyHandler;
            PART_Description.KeyDown += affirmativeKeyHandler;

            grid.KeyDown += HomeFlyout_KeyDown;
        }

        private void HomeFlyout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MyPopup.IsOpen = false;
            }
        }
        
        private void PART_SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MyPopup.IsOpen = false;
        }

        #region Notify

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class TitleToToolTipConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            string titles = (value as string);
            string title = titles.Substring(0, titles.Length - 1);
            return string.Format("Add {0}", title);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
