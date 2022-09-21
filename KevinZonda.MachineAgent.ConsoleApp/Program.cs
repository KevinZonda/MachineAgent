
using KevinZonda.MachineAgent.ConsoleApp.Controllers;

var r = await IPController.GetIPInfo();

Console.WriteLine(r.Location);