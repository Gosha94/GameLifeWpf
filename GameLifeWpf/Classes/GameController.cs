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
        // Макс кол-во строк в канвасе
        public int numberofCellInWidth = 40;
        // Макс кол-во столбцов в канвасе
        public int numberofCellInHeight = 40;
        // Набор клеток на поле в виде массива
        public Rectangle[,] fields;
        // Таймер для отсчета времени между поколениями
        public DispatcherTimer dispatcherTimer;
        // Признак старта отсчета таймера
        public bool isStartedTimer = false;
        // Поле необходимое для случайного заполнения поля
        public Random random;
        
        private void DispatcherTimer_Tick( object sender, EventArgs e )
        {
            LifeCreator.CreateNextGeneration();
        }


        // Реализуем паттерн Singletone для получения доступа к Контроллеру игры
        private static GameController instance;
        /// <summary>
        /// Конструктор класса является закрытым, его можно использовать только через метод getInstance()
        /// </summary>
        private GameController()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            fields = new Rectangle[numberofCellInWidth, numberofCellInHeight];
            random = new Random();
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
