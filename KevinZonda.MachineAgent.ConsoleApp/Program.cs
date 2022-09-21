
using KevinZonda.MachineAgent.ConsoleApp;
using KevinZonda.MachineAgent.ConsoleApp.Controllers;

var rst = await ScriptEngineController.Exec(new[]
{
    "var x = 11;",
    "x = \"a\";",
    "return x;"
});

Console.WriteLine("Script:" + rst.Script);
Console.WriteLine("Return:" + rst.Result);
Console.WriteLine("Ex    :" + rst.Exception);
rst.Variables?.ForEach(x => Console.WriteLine(x));
return;
Console.WriteLine(AboutController.GetAboutMessage());
return;
var r = await IPController.GetIPInfo();

Console.WriteLine(r.Location);