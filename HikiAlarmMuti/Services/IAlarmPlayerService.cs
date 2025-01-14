using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikiAlarmMobile.Services;

public interface IAlarmPlayerService
{
    void Play(string name);
    void Stop();

    event Action PlaybackStopped;
}
