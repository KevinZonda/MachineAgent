namespace KevinZonda.MachineAgent.ConsoleApp.Controllers.Models;


public class ScriptResult
{
    public string Script { get; set; }
    public object Result { get; set; }
    public VariableModel[] Variables { get; set; }
    public Exception? Exception { get; set; }
    public bool IsFailed => Exception != null;
}

