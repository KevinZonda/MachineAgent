namespace KevinZonda.MachineAgent.ConsoleApp.Controllers.Models;


public class ScriptResult
{
    public string Script { get; set; }
    public object Result { get; set; }
    public VariableModel[] Variables { get; set; }
    public Exception? Exception { get; set; }
    public bool IsOk => Exception == null;

    public override string ToString()
    {
        return
            $"Run Result\n" +
            $"==========\n" +
            $"IsOk  : {IsOk}\n" +
            $"Return: {Result}\n" +
            $"Except: {ExToStr(Exception)}";
    }
    private static string ExToStr(Exception? e)
    {
        if (e == null) return "null";
        return e.Message;
    }
}

