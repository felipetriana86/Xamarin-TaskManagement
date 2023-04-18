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
    public partial class AssignedTasksPage : ContentPage
    {
        Session ses = new Session();
        public AssignedTasksPage(Session session)
        {
            ses = session;
            InitializeComponent();
            GetAllTasksAssigned();
        }

       private async void GetAllTasksAssigned()
        {
            var tasks = await API.GetTasksAssignedTo(ses.AuthorizationToken);
            DataSource.allTasks = tasks;
            listTasksAssigned.ItemsSource = DataSource.allTasks;
        }
        private void btnSave_Clicked(object sender, EventArgs e)
        {

        }

        private void listTasksAssigned_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var taskDetail = new UpdateTaskPage((TaskModel)e.SelectedItem, ses);
            taskDetail.BindingContext = (TaskModel)e.SelectedItem;
            Navigation.PushAsync(taskDetail);

        }
    }
}