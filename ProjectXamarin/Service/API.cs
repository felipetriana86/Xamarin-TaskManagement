using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectXamarin.Data;
using ProjectXamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectXamarin.Service
{


    public class CreateTaskRequest
    {

        public string description { get; set; }
        public string assignedToUid { get; set; }
        public CreateTaskRequest(string description, string assignedToUid)
        {
            this.description = description;
            this.assignedToUid = assignedToUid;
        }
    }

    public class UpdateTaskRequest
    {
        public bool done { get; set; }

        public UpdateTaskRequest(bool done)
        {
            this.done = done;
        }
    }
    class API
    {
        public static ObservableCollection<TaskModel> mytasks = new ObservableCollection<TaskModel>();
        public static string BaseUrl = "https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/";
        public static string loginEndPoint = "users/login";
        public static string signInEndPoint = "users/signup";
        public static string getAllUsers = "users/all";
        public static string createTask = "tasks";
        public static string getTasksCreatedBy = "tasks/createdby";
        public static string getTasksAssignedTo = "tasks/assignedto";

        public static string logoutAPI = "";

        
        public static async Task<string> SignUp(UserModel usr)
        {

            Uri RequestUri = new Uri(BaseUrl + signInEndPoint);

            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(usr);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(RequestUri, contentJson);
            string content = await response.Content.ReadAsStringAsync();
            var values = JObject.Parse(content);
            return values.ToString();



        }
        public static async Task<string> SignIn(UserModel usr)
        {

            Uri RequestUri = new Uri(BaseUrl + loginEndPoint);

            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(usr);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(RequestUri, contentJson);
            string content = await response.Content.ReadAsStringAsync();
            var values = JObject.Parse(content);
            return values.ToString();


        }

        public static async Task<ObservableCollection<TaskModel>> GetTasksCreatedBy(string token)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("x-access-token", token);
            Uri RequestUri = new Uri(BaseUrl + getTasksCreatedBy);
            var response = await client.GetAsync(RequestUri);

            if ((int)response.StatusCode == 308)
            {
                var newLocation = response.Headers.Location;
                var newRequest = new HttpRequestMessage(HttpMethod.Get, newLocation);
                response = await client.SendAsync(newRequest);
            }

            var content = await response.Content.ReadAsStringAsync();
            var details = JObject.Parse(content)["allTasks"];
            var tasks = details.Select(t => new TaskModel { taskUid = t["taskUid"].ToString(), description = t["description"].ToString(), assignedToName = t["assignedToName"].ToString() }).ToList();
            mytasks = new ObservableCollection<TaskModel>(tasks);
            return mytasks;
        }

        public static async Task<ObservableCollection<TaskModel>> GetTasksAssignedTo(string token)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("x-access-token", token);
            Uri RequestUri = new Uri(BaseUrl + getTasksAssignedTo);
            var response = await client.GetAsync(RequestUri);

            if ((int)response.StatusCode == 308)
            {
                var newLocation = response.Headers.Location;
                var newRequest = new HttpRequestMessage(HttpMethod.Get, newLocation);
                response = await client.SendAsync(newRequest);
            }

            var content = await response.Content.ReadAsStringAsync();
            var details = JObject.Parse(content)["allTasks"];
            var tasks = details.Select(t => new TaskModel { taskUid = t["taskUid"].ToString(), description = t["description"].ToString(), createdByName = t["createdByName"].ToString() , done = Convert.ToBoolean(t["done"]) }).ToList();
            mytasks = new ObservableCollection<TaskModel>(tasks);
            return mytasks;
        }

        public static async Task<ObservableCollection<UserModel>> GetAllUsers(string token)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("x-access-token", token);
            Uri RequestUri = new Uri(BaseUrl + getAllUsers);
            var response = await client.GetAsync(RequestUri);

            if ((int)response.StatusCode == 308)
            {
                var newLocation = response.Headers.Location;
                var newRequest = new HttpRequestMessage(HttpMethod.Get, newLocation);
                response = await client.SendAsync(newRequest);
            }

            var content = await response.Content.ReadAsStringAsync();
            var details = JObject.Parse(content)["allUsers"];
            var username = details.Select(u => new UserModel { name = u["name"].ToString(), uid = u["uid"].ToString() }).ToList();
            ObservableCollection<UserModel> myUsernames = new ObservableCollection<UserModel>(username);
            return myUsernames;
        }

        public static async Task<bool> DeleteTask(string taskUid, string token)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("x-access-token", token);
            Uri RequestUri = new Uri($"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/{taskUid}");
            var response = await client.DeleteAsync(RequestUri);

            if ((int)response.StatusCode == 308)
            {
                var newLocation = response.Headers.Location;
                var newRequest = new HttpRequestMessage(HttpMethod.Get, newLocation);
                response = await client.SendAsync(newRequest);
            }

            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                foreach (TaskModel task in mytasks)
                {
                    if (task.taskUid == taskUid)
                    {
                        mytasks.Remove(task);
                        return true;

                    }
                }

            }

            return false;

        }


        public static async Task<string> CreateTask(string description, string uid, string name, string token)
        {

            CreateTaskRequest update = new CreateTaskRequest(description, uid);
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            HttpClient client = new HttpClient(handler);
            StringContent jsonContent = new StringContent(
            JsonConvert.SerializeObject(update),
            Encoding.UTF8,
            "application/json"
            );
            client.DefaultRequestHeaders.Add("x-access-token", token);
            Uri uri = new Uri($"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks");
            var response = await client.PostAsync(uri, jsonContent);
            string content = await response.Content.ReadAsStringAsync();



            if ((int)response.StatusCode == 200)
            {
                var value = JObject.Parse(content)["id"];
                mytasks.Add(new TaskModel(value.ToString(), name, description));
                value = JObject.Parse(content);
                return value.ToString();
            }
            else
            {
                var value = JObject.Parse(content);
                return value.ToString();
            }


        }
        public static async Task<HttpResponseMessage> PatchAsync(HttpClient client, string RequestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), RequestUri);
            request.Content = content;
            return await client.SendAsync(request);
        }

        public static async Task<string> UpdateTask(string taskUid, string token, bool done)
        {

            
            UpdateTaskRequest update = new UpdateTaskRequest(done);
            StringContent jsonContent = new StringContent(
               JsonConvert.SerializeObject(update),
               Encoding.UTF8,
               "application/json"
               );
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            HttpClient client = new HttpClient(handler);

            
            client.DefaultRequestHeaders.Add("x-access-token", token);
            Uri RequestUri = new Uri($"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/{taskUid}");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), RequestUri)
            {
                Content = jsonContent
            };
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating task");
            }
            string content = await response.Content.ReadAsStringAsync();
            var values = JObject.Parse(content);
            return values.ToString();
        }



    }
}
      










        







