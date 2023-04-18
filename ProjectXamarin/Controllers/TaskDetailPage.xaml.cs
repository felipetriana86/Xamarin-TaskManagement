using ProjectXamarin.Data;
using ProjectXamarin.Models;
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
    public partial class TaskDetailPage : ContentPage
    {
        Session ses = new Session();
        TaskModel tasks = new TaskModel();
        public TaskDetailPage(TaskModel tsk, Session session)
        {
            InitializeComponent();
            ses = session;
            tasks = tsk;
            
           // dpDate.Date = Convert.ToDateTime(tsk.deadLine);
            GetUsers();
          // int index = DataSource.allUsers.FindIndex(item => item.name == tsk.assignedToName);
          // pickerUser.SelectedIndex = index;
            
        }
        private async void GetUsers()
        {
            var users = await API.GetAllUsers(ses.AuthorizationToken);
            DataSource.allUsers = users;
            pickerUser.ItemsSource = DataSource.allUsers;
            List<UserModel> userList = DataSource.allUsers.ToList();
            int index = userList.FindIndex(item => item.name == tasks.assignedToName);
            pickerUser.SelectedIndex = index;

        }


        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            if(await API.DeleteTask(tasks.taskUid, ses.AuthorizationToken))
            {
                await DisplayAlert("Alert", "Task Successfully deleted", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("ALert", "Task could not be deleted", "OK");
            }
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}