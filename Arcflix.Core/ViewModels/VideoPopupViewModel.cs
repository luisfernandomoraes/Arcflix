using System;
using Arcflix.Controls.ChillPlayer;
using Octane.Xam.VideoPlayer;

namespace Arcflix.ViewModels
{
    public class VideoPopupViewModel:BaseViewModel
    {
        #region Fields

        private VideoSource _youtubeSource;

        #endregion

        #region Properties

        public VideoSource YoutubeSource
        {
            get => _youtubeSource;
            set => SetProperty(ref _youtubeSource,value);
        }

        #endregion
        #region Constructor

        public VideoPopupViewModel(string youtubeId)
        {
            YoutubeSource = YouTubeVideoIdExtension.Convert(youtubeId);
        }

        #endregion

    }
}