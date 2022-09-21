
using KevinZonda.MachineAgent.ConsoleApp.Controllers;

Console.WriteLine(AboutController.GetAboutMessage());
return;
var r = await IPController.GetIPInfo();

Console.WriteLine(r.Location);