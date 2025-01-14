using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HikiAlarmMuti.Models;

public class BilibiliLiveInfo
{
    [JsonPropertyName("data")]
    public LiveData Data { get; set; }
}

public class LiveData
{
    [JsonPropertyName("live_status")]
    public int LiveStatus { get; set; }
}

