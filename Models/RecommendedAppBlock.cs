using System.Windows.Input;

public class RecommendedAppBlock
{
    public string Label {get;set;}
    public ICommand Command{get;set;}

    public RecommendedAppBlock(string label, ICommand command)
    {
        Label=label;
        Command=command;
    }
}