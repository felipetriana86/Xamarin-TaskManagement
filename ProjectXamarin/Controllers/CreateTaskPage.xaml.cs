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
    public partial class CreateTaskPage : ContentPage
    {
        Session ses = new Session();      
        public CreateTaskPage(Session session)
        {
            ses = session;
            InitializeComponent();
            GetUsers();
          
        }
        private async void GetUsers()
        {
            var users = await API.GetAllUsers(ses.AuthorizationToken);
            DataSource.allUsers = users;
            pickerUser.ItemsSource = DataSource.allUsers;
            pickerUser.SelectedIndex = 0;
           

        }

        private async void btnAddTask_Clicked(object sender, EventArgs e)
        {
            
            UserModel selectedUser = (UserModel)pickerUser.SelectedItem;
            string descriptionVal = txtTaskDescription.Text;
            string assignedToUidVal = selectedUser.uid;
            


            try
            {
                // Call the CreateTask method to create a new task
                string taskId = await API.CreateTask(descriptionVal,assignedToUidVal, selectedUser.name,ses.AuthorizationToken);

                // Display a success message
                await DisplayAlert("Success", $"Task created successfully!", "OK");
                await Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", $"Failed to create task: {ex.Message}", "OK");
            }

        }


        
        
    }
}