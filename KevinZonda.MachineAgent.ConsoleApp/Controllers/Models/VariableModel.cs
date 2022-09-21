namespace KevinZonda.MachineAgent.ConsoleApp.Controllers.Models;

public class VariableModel
{
    public string Name { get; }
    public object Value { get; }

    public VariableModel(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public override string? ToString()
    {
        return $"[Variable]\nName={Name}\nValue={Value}";
    }
}
