using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Text;

namespace KevinZonda.MachineAgent.ConsoleApp.Controllers;

internal class ScriptEngineController
{
    private static readonly ScriptResult EmptyResult = new ScriptResult
    {
        Script = "",
        Result = null,
        Variables = null,
        Exception = null
    };

    public static async Task<ScriptResult> Exec(string[] code)
    {
        if (code == null || code.Length < 1) return EmptyResult;

        var sb = new StringBuilder();
        ScriptState state = null;

        Script script = null;
        Exception insideExp = null;

        foreach (var line in code)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            sb.AppendLine(line);
            try
            {
                if (script == null || state == null)
                {
                    script = CSharpScript.Create(line);
                    state = await script.RunAsync();
                }
                else
                {
                    state = await state.ContinueWithAsync(line);
                }
            }
            catch (Exception ex)
            {
                insideExp = ex;
            }

            if (state == null || state.Exception != null || insideExp != null) break;
        }
        if (state == null) return EmptyResult;

        return new ScriptResult
        {
            Script = sb.ToString(),
            Result = state.ReturnValue,
            Variables = state.Variables.Select(v => new VariableModel { Name = v.Name, Value = v.Value }).ToArray(),
            Exception = state.Exception ?? insideExp
        };
    }

    public class ScriptResult
    {
        public string Script { get; set; }
        public object Result { get; set; }
        public VariableModel[] Variables { get; set; }
        public Exception? Exception { get; set; }

    }

    public class VariableModel
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
