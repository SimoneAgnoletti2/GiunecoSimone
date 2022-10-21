using GiunecoSimone.Enumerator;
using GiunecoSimone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiunecoSimone.Interfaces
{
    public interface ITasks
    {
        Tasks GetTasks(string id);
        IEnumerable<Tasks> GetAllTasks();

        IEnumerable<Tasks> GetTasks(State state, string idUser);
    }
}