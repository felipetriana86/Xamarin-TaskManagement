using Newtonsoft.Json;
using ProjectXamarin.Models;
using ProjectXamarin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectXamarin.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            UserModel user = new UserModel
            {
                password = txtPassword.Text,
                email = txtEmail.Text,
                name = txtName.Text

            };
            var result = await API.SignUp(user);
            Dictionary<string, object> obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(result));
            if (result.Contains("error"))
            {
                await DisplayAlert("Alert", obj["error"].ToString(), "OK");
                
            }

            else
            {
                await DisplayAlert("Alert", $"User  '{ txtName.Text}' was created!", "OK");
                await Navigation.PushAsync(new MainPage());
                
            }
         }
            
            
                
            
        }
    }
