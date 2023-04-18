using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectXamarin.Data;
using ProjectXamarin.Models;
using ProjectXamarin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectXamarin.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserHomePage : ContentPage
    {
        Session ses = new Session();
        public UserHomePage(Session session, string name)
            {
           
            InitializeComponent();
            ses = session;
            lblName.Text = name;
            GetTasks();
            
            
            
            

        }

       private async void GetTasks()
        {
            
             var tasks = await API.GetTasksCreatedBy(ses.AuthorizationToken);
            DataSource.allTasks = tasks;
            listTasks.ItemsSource = DataSource.allTasks;


        }

        private void listTasks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var taskDetail = new TaskDetailPage((TaskModel)e.SelectedItem, ses);
            taskDetail.BindingContext = (TaskModel)e.SelectedItem;
            Navigation.PushAsync(taskDetail);
            //listTasks.SelectedItem = null;
        }

        private async void btnAddTask_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTaskPage(ses));
        }
        
        private async void btnViewMyTasks_Clicked(object sender, EventArgs e)
        {
           
            await Navigation.PushAsync(new AssignedTasksPage(ses));
        }
    }
}