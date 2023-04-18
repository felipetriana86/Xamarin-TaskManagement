using ProjectXamarin.Data;
using ProjectXamarin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectXamarin.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class UpdateTaskPage : ContentPage
    {
       
        Session ses = new Session();
        TaskModel tasks = new TaskModel();
        public UpdateTaskPage(TaskModel tsk, Session session)
        {
            InitializeComponent();
            ses = session;
            tasks = tsk;
            lblTaskDescription.Text = tsk.description;
            lblCreatedBy.Text = tsk.createdByName;
            isTaskDone.IsToggled = tsk.done;

        }
      

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            isTaskDone.IsEnabled = tasks.done;
            var updatedTask = await API.UpdateTask(tasks.taskUid, ses.AuthorizationToken, tasks.done);
            if (updatedTask.Contains("error"))
            {
                await DisplayAlert("Alert", "Unfortuately, the task could not be updated", "OK");

            }

            else
            {
                
                await DisplayAlert("Success", "Task successfully updated!", "OK");
                await Navigation.PopAsync();


            }

        }
    }
}