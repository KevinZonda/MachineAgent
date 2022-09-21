namespace KevinZonda.MachineAgent.ConsoleApp.Controllers.Models;

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
