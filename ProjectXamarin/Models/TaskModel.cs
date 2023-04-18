using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectXamarin
{
    public class TaskModel
    {
        public TaskModel()
        {

        }

        public TaskModel(string taskUid, string assignedToName, string description)
        {
            this.taskUid = taskUid;
            this.assignedToName = assignedToName;
            this.description = description;
        }

        public string taskUid { get; set; }
        public string assignedToName { get; set; }
        public string assignedToUid { get; set; }
        public string createdByName { get; set; }
        public string createdByUid { get; set; }
        public string description { get; set; }
        public DateTime deadLine { get; set; }
        public bool done { get; set; }



    }
}







