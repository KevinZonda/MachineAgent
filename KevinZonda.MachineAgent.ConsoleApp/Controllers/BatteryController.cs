using Batteryno;
using KevinZonda.MachineAgent.ConsoleApp.Controllers.Models;
using System.Runtime.Versioning;

namespace KevinZonda.MachineAgent.ConsoleApp.Controllers;

internal partial class BatteryController
{
    [SupportedOSPlatform("linux")]
    public static BatteryInfo GetBatteryInfo()
    {
        var bat = Power.GetBatteries().FirstOrDefault();
        if (bat == null) return new BatteryInfo() { IsOk = false };
        return new BatteryInfo()
        {
            IsOk = true,
            Percentage = bat.Capacity,
            Status = BatteryInfo.Parse(bat.Status)
        };
    }

    public class BatteryStatusChangedEventArgs
    {
        public BatteryInfo.BatteryStatus PrevStatus { get; }
        public BatteryInfo.BatteryStatus CurrentStatus { get; }
        public BatteryStatusChangedEventArgs(BatteryInfo.BatteryStatus prevStatus, BatteryInfo.BatteryStatus currentStatus)
        {
            PrevStatus = prevStatus;
            CurrentStatus = currentStatus;
        }
    }

    [SupportedOSPlatform("linux")]
    public static void MonitorBatteryChange(int interval = 1000, Action<BatteryStatusChangedEventArgs> batteryStatusChanged = null)
    {
        BatteryInfo.BatteryStatus status = BatteryInfo.BatteryStatus.Unknown;
        while (true)
        {
            try
            {
                var info = GetBatteryInfo();
                if (info == null || !info.IsOk) break;
                var tmp = status;
                status = info.Status;
                if (status != tmp && batteryStatusChanged != null)
                    batteryStatusChanged(new(tmp, status));
            }
            catch
            {
                // TODO;
            }
            Thread.Sleep(interval);
        }
    }
}
