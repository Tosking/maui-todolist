using System.ComponentModel;

namespace Todolist.Model;

public class Model : INotifyPropertyChanged 
{
    private string text;
    private bool finished;
    private int value1;
    public event PropertyChangedEventHandler PropertyChanged;

    public string Text
    {
        get
        { return text; }
        set
        {
            text = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
        }
    }

    public bool Finished
    {
        get { return finished; }
        set
        {
            finished = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Finished)));
        }
    }

    public int Value
    {
        get => value1; 
        set
    {
        value1 = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
    }
    }
}