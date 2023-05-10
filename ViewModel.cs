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
            if (task == value)
                return;
            task = value;
            OnPropertyChanged(nameof(Selected));
            if (task != null)
            {
                Edit();
            }

           
           
            //Selected = null;
            
        }
    }

    private string[] TrimString(string str)
    {
        var result = -1;
        var value = str.LastIndexOf(" ");
        if(value != -1)
            if (int.TryParse(str[value..], out result)){}
        
        if (!string.IsNullOrWhiteSpace(str))
            if(result > 0)
                return new string[] { str[..value], result.ToString() };
            else
                return new string[] { str, null};
        return new string[] { null, null };
    }
    
    private void Create(string entrytext)
    {
        var value = -1;
        var result = TrimString(entrytext);
        
        if (int.TryParse(result[1], out value))
            ListTasks.Add(new Model.Model() { Text = result[0], Finished = false, Value = int.Parse(result[1]) });
        else
            ListTasks.Add(new Model.Model() { Text = result[0], Finished = false, Value = 1 });
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
        EditCommand = new Command(Edit);

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
                    Selected.Text = str[0];
                if (str[1] != null)
                    Selected.Value = int.Parse(str[1]);

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