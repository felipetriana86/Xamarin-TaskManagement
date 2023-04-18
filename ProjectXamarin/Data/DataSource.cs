using ProjectXamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectXamarin.Data
{
    class DataSource
    { 
       public static ObservableCollection<TaskModel> allTasks { get; set; } = new ObservableCollection<TaskModel>();
        public static ObservableCollection<UserModel> allUsers { get; set; } = new ObservableCollection<UserModel>();
    }









       
       
    }

