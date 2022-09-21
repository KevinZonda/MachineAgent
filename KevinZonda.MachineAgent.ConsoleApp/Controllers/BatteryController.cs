using Batteryno;
using System.Runtime.Versioning;

namespace KevinZonda.MachineAgent.ConsoleApp.Controllers;

internal class BatteryController
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

    public class BatteryInfo
    {
        public int Percentage;
        public bool IsOk;
        public BatteryStatus Status;

        public enum BatteryStatus
        {
            Charging,
            Discharging,
            Unknown
        }

        public static BatteryStatus Parse(Batteryno.BatteryStatus s)
        {
            return s switch
            {
                Batteryno.BatteryStatus.Charging => BatteryStatus.Charging,
                Batteryno.BatteryStatus.Discharging => BatteryStatus.Discharging,
                _ => BatteryStatus.Unknown
            };
        }
    }
}
