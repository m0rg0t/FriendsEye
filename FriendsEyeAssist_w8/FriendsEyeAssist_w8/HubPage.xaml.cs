using Bing.Maps;
using FriendsEyeAssist_w8.Common;
using FriendsEyeAssist_w8.Controls;
using FriendsEyeAssist_w8.Data;
using FriendsEyeAssist_w8.Model;
using FriendsEyeAssist_w8.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента страницы концентратора задокументирован по адресу http://go.microsoft.com/fwlink/?LinkID=321224

namespace FriendsEyeAssist_w8
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper используется на каждой странице для облегчения навигации и 
        /// управление жизненным циклом процесса
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Эту настройку можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации.  Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы и
        /// словарь состояний, сохраненных этой страницей в ходе предыдущего
        /// сеанса.  Это состояние будет равно NULL при первом посещении страницы.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-4");
            this.DefaultViewModel["Section3Items"] = sampleDataGroup;

            try
            {
                if (ViewModelLocator.MainStatic.PhotoItems.Count() < 1)
                {
                    await ViewModelLocator.MainStatic.LoadData();
                };

                try
                {
                    foreach (var item in ViewModelLocator.MainStatic.NearestPhotoItems)
                    {
                        var pushpin = new Pushpin();

                        MapLayer.SetPosition(pushpin, new Location(item.Lat, item.Lon));
                        pushpin.Name = item.ObjectId.ToString();
                        pushpin.Tag = item.ObjectId.ToString();
                        //pushpin.Tapped += pushpinTapped;
                        CurrentMap.Children.Add(pushpin);
                    };
                }
                catch { };
            }
            catch { };
        }

        /// <summary>
        /// Вызывается при нажатии заголовка HubSection.
        /// </summary>
        /// <param name="sender">Концентратор, который содержит элемент HubSection, заголовок которого щелкнул пользователь.</param>
        /// <param name="e">Данные о событии, описывающие, каким образом было инициировано нажатие.</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            var group = section.DataContext;
            this.Frame.Navigate(typeof(SectionPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Вызывается при нажатии элемента внутри раздела.
        /// </summary>
        /// <param name="sender">Представление сетки или списка
        /// в котором отображается нажатый элемент.</param>
        /// <param name="e">Данные о событии, описывающие нажатый элемент.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Переход к соответствующей странице назначения и настройка новой страницы
                // путем передачи необходимой информации в виде параметра навигации
                var itemId = ((AssistsPhoto)e.ClickedItem).ObjectId;
                this.Frame.Navigate(typeof(ItemPage), itemId);
            }
            catch { };
        }
        #region Регистрация NavigationHelper

        /// Методы, предоставленные в этом разделе, используются исключительно для того, чтобы
        /// NavigationHelper для отклика на методы навигации страницы.
        /// 
        /// Логика страницы должна быть размещена в обработчиках событий для 
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// и <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// Параметр навигации доступен в методе LoadState 
        /// в дополнение к состоянию страницы, сохраненному в ходе предыдущего сеанса.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
            navigationHelper.OnNavigatedFrom(e);
        }

        void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            try
            {
                var viewAboutPage = new SettingsCommand("", "Об авторе", cmd =>
                {
                    Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
                    settings.Width = 500;
                    //settings.HeaderBackground = new SolidColorBrush(App.VisualElements.BackgroundColor);
                    //.HeaderForeground = new SolidColorBrush(Colors.Black);
                    settings.Title = "Об авторе"; //string.Format("{0} Custom 2", App.VisualElements.DisplayName);
                    settings.IconSource = new BitmapImage(Windows.ApplicationModel.Package.Current.Logo);
                    settings.Content = new About();
                    settings.Show();
                });
                args.Request.ApplicationCommands.Add(viewAboutPage);

                var viewAboutMalukahPage = new SettingsCommand("", "Политика конфиденциальности", cmd =>
                {
                    Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
                    settings.Width = 500;
                    //settings.HeaderBackground = new SolidColorBrush(App.VisualElements.BackgroundColor);
                    //.HeaderForeground = new SolidColorBrush(Colors.Black);
                    settings.Title = "Политика конфиденциальности"; //string.Format("{0} Custom 2", App.VisualElements.DisplayName);
                    settings.IconSource = new BitmapImage(Windows.ApplicationModel.Package.Current.Logo);
                    settings.Content = new Privacy();
                    settings.Show();
                });
                args.Request.ApplicationCommands.Add(viewAboutMalukahPage);
            }
            catch { };
        }

        #endregion

        private void map_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
            e.Complete();
        }

        private void map_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }

        Map CurrentMap;

        private void map_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentMap = (sender as Map);
                foreach (var item in ViewModelLocator.MainStatic.NearestPhotoItems)
                {
                    var pushpin = new Pushpin();

                    MapLayer.SetPosition(pushpin, new Location(item.Lat, item.Lon));
                    pushpin.Name = item.ObjectId.ToString();
                    pushpin.Tag = item.ObjectId.ToString();
                    //pushpin.Tapped += pushpinTapped;
                    CurrentMap.Children.Add(pushpin);
                };
            }
            catch { };
        }

        private void LoginArea_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                MessageDialog item = new MessageDialog("Авторизация пользователя");
                item.ShowAsync();
            }
            catch { };
        }

        private void SingUpArea_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                MessageDialog item = new MessageDialog("Регистрация пользователя");
                item.ShowAsync();
            }
            catch { };
        }
    }
}
