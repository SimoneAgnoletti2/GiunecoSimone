using GiunecoSimone.Enumerator;
using GiunecoSimone.Interfaces;
using GiunecoSimone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiunecoSimone.Class
{
    public class ServiceTasks : ITasks
    {
        List<Tasks> tasks = new List<Tasks>();
        Tasks task = new Tasks();

        public Tasks GetTasks(string idTask)
        {
            using (giuneco_Entities eu = new giuneco_Entities())
            {
                task = eu.Tasks.Where(x => x.Id.Equals(idTask)).FirstOrDefault();
            }
            return task;
        }

        public IEnumerable<Tasks> GetAllTasks()
        {
            using (giuneco_Entities eu = new giuneco_Entities())
            {
                tasks = eu.Tasks.ToList();
            }
            return tasks;
        }

        public IEnumerable<Tasks> GetTasks(State state, string idUser)
        {
            using (giuneco_Entities eu = new giuneco_Entities())
            {
                var user = eu.UsersTasks.Where(x => x.IdUser.ToString().Equals(idUser)).Select(x => x.IdTask).ToList();
                tasks = eu.Tasks.Where(x => user.Contains(x.Id) 
                && x.State.Equals((int)state)).ToList();
            }
            return tasks;
        }
    }
}