using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectXamarin.Controllers;
using ProjectXamarin.Data;
using ProjectXamarin.Models;
using ProjectXamarin.Service;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectXamarin
{
    public partial class MainPage : ContentPage
    {
        static UserModel selectedUser { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }
           
        
        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
           
            UserModel user = new UserModel
            {
                password = txtPassword.Text,
                email = txtEmail.Text,

            };
            var result = await API.SignIn(user);
            Dictionary<string, object> obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(result));
            if (result.Contains("error"))
            {
                await DisplayAlert("Alert", obj["error"].ToString(), "OK");

            }

            else
            {
                var userName = JObject.Parse(result)["loggedUser"];
               Session session = new Session(obj["token"].ToString());
                
                var token = obj["token"].ToString();
                var sess = new Session
                {
                    AuthorizationToken = token
                };
                AssignedTasksPage assignedTasks = new AssignedTasksPage(session);
                await DisplayAlert("Welcome!", "You are successfully LoggedIn", "OK");
                await Navigation.PushAsync(new UserHomePage(session, userName["name"].Value<string>()));
                

            }
        }
    }
    }

        
        

       
    







    
          
