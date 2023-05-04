using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Graphics.Text;
using Todolist.Model;

namespace Todolist.ViewModel;

public class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public ICommand CreateCommand {set; get; }
    public ICommand DeleteCommand { set; get; }
    public ICommand EditCommand { get; set; }
    public ObservableCollection<Model.Model> ListTasks { get; set; } =
        new ObservableCollection<Model.Model>();
    private Model.Model task;
    public Model.Model Selected { 
        get=> task;
        set
        {
            task = value;
                if(task != null)
                    Edit();
                OnPropertyChanged(nameof(Selected));
        }
    }

    private string[] TrimString(string str)
    {
        var value = str.LastIndexOf(" ");
        if (int.TryParse(str[value..], out int result)){}
        else result = 1;
            
        if (value > 0)
            if (string.IsNullOrWhiteSpace(str[..value]))
                return new string[] { null, null };
        return new string[] { str[..value], result.ToString() };
    }
    
    private void Create(string entrytext)
    {
        var result = TrimString(entrytext);
        if (result == null)
            return;
        else
            ListTasks.Add(new Model.Model() { Text = result[0], Finished = false, Value = int.Parse(result[1]) });
    }

    private void Delete(Model.Model CurrentTask)
    {
        ListTasks.Remove(CurrentTask);
    }
    
    public ViewModel()
    {
        task = new Model.Model();
        CreateCommand = new Command<string>(Create, (current) =>  current != null ? !string.IsNullOrEmpty(TrimString(current)[0]) : false);
        DeleteCommand = new Command<Model.Model>(Delete);
            
    }
    public string Texttask
    {
        get=>task.Text;
        set 
        {
            task.Text = value;
            OnPropertyChanged(nameof(Texttask));
        }
    }

    public async void Edit()
    {
        if (!Selected.Finished)
        {
            string text = await Application.Current?.MainPage?.DisplayPromptAsync("Edit", "Current task", initialValue: Selected.Text);
            if (string.IsNullOrWhiteSpace(text))
            {
                if (text is null)
                    Selected = null;
                else
                {
                    await Application.Current?.MainPage?.DisplayAlert(title: "Edit task", message: "You didn't enter anything", cancel: "Cancel");
                    Selected = null;
                }

            }
            else
            {
                var str = TrimString(text);
                if (str[0] != null)
                {
                    Selected.Text = str[0];
                    Selected.Value = int.Parse(str[1]);
                }

                Selected = null;
            }
        }
        else
            Selected = null;
    }
    
    protected void OnPropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

    
}