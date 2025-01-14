using HikiAlarmMuti.Models;
using HikiAlarmMuti.Services;

namespace HikiAlarmMuti.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private CancellationTokenSource? _cancellationTokenSource;
    private IAlarmPlayerService? _outputDevice;
    private INotificationService _notificationService;
    public string HikiroomLink { get; } = "https://live.bilibili.com/27495696?broadcast_type=1&is_room_feed=1&spm_id_from=333.999.live_users_card.0.click&live_from=86001";

    [ObservableProperty]
    private bool isRunning = false;

    [ObservableProperty]
    private bool autoJump = false;
    [ObservableProperty]
    private bool needVoice = false;

    [ObservableProperty]
    private bool needNotice = true;

    [ObservableProperty]
    private int count = 0;

    public MainViewModel(IAlarmPlayerService alarmPlayer, INotificationService notificationService)
    {
        LoadPreferences();
        _outputDevice = alarmPlayer;
        _notificationService = notificationService;
    }

    private void PlayNotice(CancellationTokenSource _cancellationTokenSource)
    {
        string path;
        //if (HikiMode)
        //{
        //    path = Path.Combine("HiKiAlarm.mp3");
        //}
        //else
        //{
        //    path = Path.Combine("NoticeMusic.mp3");
        //}
        //_audioFile = new AudioFileReader(path);
        //_outputDevice = new WaveOutEvent();
        //_outputDevice.Init(_audioFile);

        //TODO
        path = Path.Combine("NoticeMusic.mp3");
        _outputDevice.PlaybackStopped += () =>
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _outputDevice.Stop();
            }
        };
        _outputDevice.Play(path);
    }

    [RelayCommand]
    private async Task MonitorAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        //var roomId = "1852504554";
        var roomId = "27495696";
        IsRunning = true;

        try
        {
            await Task.Run(async () =>
            {
                using HttpClient client = new();
                string url = $"https://api.live.bilibili.com/room/v1/Room/get_info?room_id={roomId}";
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        BilibiliLiveInfo liveInfo = JsonSerializer.Deserialize(responseBody, BilibiliLiveInfoContext.Default.BilibiliLiveInfo);
                        bool liveStatus = false;
                        if (liveInfo != null && liveInfo.Data != null)
                        {
                            liveStatus = liveInfo.Data.LiveStatus == 1;
                        }
                        else
                        {
                            Console.WriteLine("Deserialization failed or Data is null.");
                        }
                        if (liveStatus)
                        {
                            if (NeedVoice)
                            {
                                PlayNotice(_cancellationTokenSource);
                            }
                            Count++;
                            SavePreferences();
                            //if (AutoJump)
                            //{
                            //    OpenUrl(HikiroomLink);
                            //}
                            if (NeedNotice)
                            {
                                _notificationService.ShowNotification("Hiki Alarm", "Hiki 开始直播");
                            }
                            await Task.Delay(TimeSpan.FromMinutes(5), _cancellationTokenSource.Token);
                            Stop();
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"请求错误: {e.Message}");
                    }
                    await Task.Delay(TimeSpan.FromSeconds(30), _cancellationTokenSource.Token); // 等待一段时间再检测
                }
            }, _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生错误: {ex.Message}");
        }
        finally
        {
            IsRunning = false;
        }
    }

    [RelayCommand]
    private void Stop()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _outputDevice?.Stop();
            IsRunning = false;
        }
    }
    static void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"无法打开网页: {ex.Message}");
        }
    }

    private void SavePreferences()
    {
        var catchedCount = new { this.Count };
        var json = JsonSerializer.Serialize(catchedCount);
        Preferences.Set("count", json);
    }


    private void LoadPreferences()
    {
        if (Preferences.ContainsKey("count"))
        {
            var json = Preferences.Get("count", string.Empty);
            var preferences = JsonSerializer.Deserialize<CatchedCount>(json);
            if (preferences != null)
            {
                this.Count = preferences.Count;
            }
        }
    }


    private class CatchedCount
    {
        public int Count { get; set; }
    }
}
