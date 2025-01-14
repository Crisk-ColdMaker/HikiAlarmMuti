using HikiAlarmMuti.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(BilibiliLiveInfo))]
internal partial class BilibiliLiveInfoContext : JsonSerializerContext
{
}
