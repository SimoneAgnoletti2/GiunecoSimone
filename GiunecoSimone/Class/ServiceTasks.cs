using GiunecoSimone.Enumerator;
using GiunecoSimone.Interfaces;
using GiunecoSimone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var id = Guid.Parse(idTask);
            using (giuneco_Entities eu = new giuneco_Entities())
            {
                task = eu.Tasks.Where(x => x.Id.Equals(id)).FirstOrDefault();
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
                if ((int)state != 4)
                {
                    var user = eu.UsersTasks.Where(x => x.IdUser.ToString().Equals(idUser)).Select(x => x.IdTask).ToList();
                    tasks = eu.Tasks.Where(x => user.Contains(x.Id)
                    && x.State.Equals((int)state)).ToList();
                }
                else
                {
                    var user = eu.UsersTasks.Where(x => x.IdUser.ToString().Equals(idUser)).Select(x => x.IdTask).ToList();
                    tasks = eu.Tasks.Where(x => user.Contains(x.Id)).ToList();
                }
            }
            return tasks;
        }

        public bool DeleteTask(string idTask)
        {
            try
            {
                using (giuneco_Entities eu = new giuneco_Entities())
                {
                    var id = Guid.Parse(idTask);
                    var userTasks = eu.UsersTasks.Where(x => x.IdTask.Equals(id)).ToList();
                    foreach (var userTask in userTasks)
                    {
                        eu.UsersTasks.Remove(userTask);
                    }

                    var task = eu.Tasks.Find(id);
                    eu.Tasks.Remove(task);
                    eu.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public int GetTotalWorkedHour(string idTask)
        {
            try
            {
                var id = Guid.Parse(idTask);
                var total = 0;
                using (giuneco_Entities eu = new giuneco_Entities())
                {
                    total = Convert.ToInt32(eu.UsersTasks.Where(x => x.IdTask.Equals(id))
                        .Select(x => x.WorkedHour).ToList().Sum());
                }
                return total;
            }
            catch
            {
                return 0;
            }
        }

        public IEnumerable<Comments> GetComments(string idTask)
        {
            var id = Guid.Parse(idTask);
            var comment = new Comments();
            var comments = new List<Comments>();

            using (giuneco_Entities eu = new giuneco_Entities())
            {
                List<string> comm = new List<string>();
                List<DateTime?> dat = new List<DateTime?>();

                comm = eu.UsersTasks.Where(x => x.IdTask.Equals(id)
                && !string.IsNullOrEmpty(x.Comment))
                    .OrderByDescending(x => x.CommentDate)
                    .Select(x => x.Comment).ToList();

                dat = eu.UsersTasks.Where(x => x.IdTask.Equals(id)
                && !string.IsNullOrEmpty(x.Comment))
                    .OrderByDescending(x => x.CommentDate)
                    .Select(x => x.CommentDate).ToList();

                for(int i = 0; i < comm.Count; i++)
                {
                    comment.date = dat[i];
                    comment.comment = comm[i];

                    comments.Add(comment);
                }
            }
            return comments;
        }
    }
}