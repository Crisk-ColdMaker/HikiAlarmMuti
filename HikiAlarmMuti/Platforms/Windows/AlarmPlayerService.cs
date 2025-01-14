using HikiAlarmMuti.Services;
using System;

namespace HikiAlarmMuti.Platforms.Windows;

public class AlarmPlayerService : IAlarmPlayerService
{
    public event Action PlaybackStopped;

    public void Play(string name)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}
