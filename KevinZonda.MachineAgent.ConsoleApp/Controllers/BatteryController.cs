using Batteryno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

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
