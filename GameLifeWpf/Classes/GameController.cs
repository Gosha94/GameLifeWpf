using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameLifeWpf.Classes
{
    public class GameController
    {
     
        // Таймер для отсчета времени между поколениями
        public DispatcherTimer dispatcherTimer;
        // Признак старта отсчета таймера
        public bool isStartedTimer = false;



        // Реализуем паттерн Singletone для получения доступа к Контроллеру игры
        private static GameController instance;
        /// <summary>
        /// Конструктор класса является закрытым, его можно использовать только через метод getInstance()
        /// </summary>
        private GameController()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
          //  dispatcherTimer.Tick += DispatcherTimer_Tick;

        }

        public static GameController getInstance()
        {
            if (instance == null)
                instance = new GameController();
            return instance;
        }
        //------- Singletone END -----------

    }
}
