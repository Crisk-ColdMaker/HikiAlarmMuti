using AndroidApp = Android.App.Application;
using MauiApp = Microsoft.Maui.Controls.Application;
using Android.Content;
using Android.Media;
using HikiAlarmMuti.Services;


namespace HikiAlarmMuti.Platforms.Android
{
    public class AlarmPlayerService : IAlarmPlayerService
    {
        public event Action PlaybackStopped;
        private MediaPlayer _mediaPlayer;

        public AlarmPlayerService()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Completion += (sender, e) => OnPlaybackStopped();
        }

        public void Play(string name)
        {
            var afd = AndroidApp.Context.Assets.OpenFd(name);
            _mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
            _mediaPlayer.Prepare();
            _mediaPlayer.Start();
        }

        public void Stop()
        {
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Reset();
                OnPlaybackStopped();
            }
        }

        protected virtual void OnPlaybackStopped()
        {
            PlaybackStopped?.Invoke();
        }
    }
}
