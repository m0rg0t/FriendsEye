﻿using FriendsEyeAssist_w8.Common;
using FriendsEyeAssist_w8.Data;
using FriendsEyeAssist_w8.Model;
using FriendsEyeAssist_w8.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// Шаблон элемента страницы элементов задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234232

namespace FriendsEyeAssist_w8
{
    /// <summary>
    /// Страница, на которой отображаются сведения об отдельном элементе внутри группы.
    /// </summary>
    public sealed partial class ItemPage : Page
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

        public ItemPage()
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
            try {
                var item = ViewModelLocator.MainStatic.NearestPhotoItems.FirstOrDefault(c => c.ObjectId == (String)e.NavigationParameter);
                if (item != null)
                {
                    this.DefaultViewModel["Item"] = item;

                    await ((AssistsPhoto)item).LoadAnswers();
                    //itemGridView.ItemsSource = ((AssistsPhoto)item).AssistsAnswersItems;
                }
                else
                {
                    try
                    {
                        var item2 = ViewModelLocator.MainStatic.PhotoItems.FirstOrDefault(c => c.ObjectId == (String)e.NavigationParameter);
                        if (item2 != null)
                        {
                            this.DefaultViewModel["Item"] = item2;

                            await ((AssistsPhoto)item2).LoadAnswers();
                            //itemGridView.ItemsSource = ((AssistsPhoto)item2).AssistsAnswersItems;
                        };
                    }
                    catch { };
                };
            }
            catch { };

            
            //var item = await SampleDataSource.GetItemAsync((String)e.NavigationParameter);
            //this.DefaultViewModel["Item"] = item;
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
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}