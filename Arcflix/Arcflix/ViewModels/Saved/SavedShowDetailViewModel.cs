﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net.TMDb;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Arcflix.Models;
using Arcflix.Services;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Arcflix.ViewModels.Saved
{
    public class SavedShowDetailViewModel : BaseViewModel
    {
        #region Fields

        private string _genders;
        private string _toolBarItemIcon;

        #endregion

        #region Commands

        public ICommand SaveShowCommand => new Command(SaveShow);

        private void SaveShow(object obj)
        {
            try
            {
                var showFromDB = Arcflix.Services.DB.ArcflixDBContext.ShowDataBase.GetItems()
                    .FirstOrDefault(x => x.IDApi == ShowDetail.Id);
                if (showFromDB != null)
                {
                    ToolBarItemIcon = "ic_bookmark_24dp.png";
                    Arcflix.Services.DB.ArcflixDBContext.ShowDataBase.DeleteItem(showFromDB.ID);
                    App.ShowToast(ToastNotificationType.Success, "Arcflix", "show removed!", 3);

                }
                else
                {
                    ToolBarItemIcon = "ic_bookmark_white_24dp.png";
                    var showModel = ShowModel.ShowApiToShowModel(ShowDetail);
                    Arcflix.Services.DB.ArcflixDBContext.ShowDataBase.SaveItem(showModel);
                    App.ShowToast(ToastNotificationType.Success, "Arcflix", "show saved!", 3);
                    
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Properties

        public Show ShowDetail { get; private set; }

        public string ToolBarItemIcon
        {
            get => _toolBarItemIcon;
            set => SetProperty(ref _toolBarItemIcon, value);
        }

        public string Genders
        {
            get => _genders;
            set => SetProperty(ref _genders, value);
        }

        #endregion

        #region Constructor

        public SavedShowDetailViewModel(Show show)
        {
            Title = "Show Details";
            ShowDetail = show;
            ToolBarItemIcon = "ic_bookmark_24dp.png";
            Task.Run(async () => await LoadDataAsync());
            SetIconToolBar();
        }

        #endregion

        #region Methods


        private void SetIconToolBar()
        {
            try
            {
                var showFromDB = Services.DB.ArcflixDBContext.ShowDataBase.GetItems()
                    .FirstOrDefault(x => x.IDApi == ShowDetail.Id);
                ToolBarItemIcon = showFromDB == null ? "ic_bookmark_24dp.png" : "ic_bookmark_white_24dp.png";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public async Task LoadDataAsync()
        {
            try
            {
                ShowDetail = await ShowDataStore.GetItemAsync(ShowDetail.Id, true);
                if (ShowDetail.Genres != null)
                {
                    var builder = new StringBuilder();
                    var genderList = ShowDetail.Genres.Select(x => x.Name).ToArray();
                    var length = genderList.Length;
                    for (var i = 0; i < length; i++)
                    {
                        if (i == length - 1)
                            builder.Append(genderList[i] + ".");
                        else
                            builder.Append(genderList[i] + ", ");

                    }
                    Genders = builder.ToString();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion
    }
}