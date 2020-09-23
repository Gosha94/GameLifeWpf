using GameLifeWpf.GameStateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLifeWpf.Classes
{
    class DatabaseHelper
    {
        public DatabaseHelper()
        {
            using (GameStateContext gameStateContxt = new GameStateContext())
            {
                //// создание и добавление моделей
                //Generation generation = new Generation { GenerationNumber = 1, GenerationBirthDate = 1990 };
                
                //gameStateContxt.Generations.Add(generation);                
                //gameStateContxt.SaveChanges();

                //Cell pl1 = new Player { Name = "Роналду", Age = 31, Position = "Нападающий", Team = t2 };
                //Player pl2 = new Player { Name = "Месси", Age = 28, Position = "Нападающий", Team = t1 };
                //Player pl3 = new Player { Name = "Хави", Age = 34, Position = "Полузащитник", Team = t1 };
                //db.Players.AddRange(new List<Player> { pl1, pl2, pl3 });
                //db.SaveChanges();                
            }
        }
        public void SaveCustomUserState()
        {

        }
        public void CheckDublicateStates()
        {

        }
    }
}
