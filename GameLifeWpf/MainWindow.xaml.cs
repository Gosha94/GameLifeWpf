using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GameLifeWpf.Classes;

namespace GameLifeWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        private static GameController settings = GameController.getInstance();
        private LifeCreator _lifeCreator = new LifeCreator();

        public MainWindow()
        {
            InitializeComponent();
            settings.dispatcherTimer.Tick += DispatcherTimer_Tick;            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            chkBx_RandomState.SetBinding(CheckBox.IsCheckedProperty,
                new System.Windows.Data.Binding(nameof(LifeCreator.IsRandom))
                {
                    Source = _lifeCreator,
                    Mode = System.Windows.Data.BindingMode.TwoWay,
                    UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
                });
        }

        private void TestEntityFrameworkCodeFirstOneToMany()
        {
            //using (SoccerContext db = new SoccerContext())
            //{
            //    // создание и добавление моделей
            //    Team t1 = new Team { Name = "Барселона" };
            //    Team t2 = new Team { Name = "Реал Мадрид" };
            //    db.Teams.Add(t1);
            //    db.Teams.Add(t2);
            //    db.SaveChanges();
            //    Player pl1 = new Player { Name = "Роналду", Age = 31, Position = "Нападающий", Team = t2 };
            //    Player pl2 = new Player { Name = "Месси", Age = 28, Position = "Нападающий", Team = t1 };
            //    Player pl3 = new Player { Name = "Хави", Age = 34, Position = "Полузащитник", Team = t1 };
            //    db.Players.AddRange(new List<Player> { pl1, pl2, pl3 });
            //    db.SaveChanges();

            //    // вывод 
            //    foreach (Player pl in db.Players.Include(p => p.Team))
            //        Debug.WriteLine("{0} - {1}", pl.Name, pl.Team != null ? pl.Team.Name : "" + Environment.NewLine );
            //    foreach (Team t in db.Teams.Include(t => t.Players))
            //    {
            //        Debug.WriteLine("Команда: {0}", t.Name);
            //        foreach (Player pl in t.Players)
            //        {
            //            Debug.WriteLine("{0} - {1}", pl.Name, pl.Position);
            //        }
            //        Debug.WriteLine(Environment.NewLine); 
            //    }
            //    // удаление игрока
            //    Player pl_toDelete = db.Players.First(p => p.Name == "Роналду");
            //    db.Players.Remove(pl_toDelete);
            //    // удаление команды     
            //    Team t_toDelete = db.Teams.First();
            //    db.Teams.Remove(t_toDelete);
            //    db.SaveChanges();
            //}

        }

        private void EmptyCell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = (Rectangle)sender;
            var value = (bool)(cell.Tag);
            var newValue = !value;
            cell.Tag = newValue;
            cell.Fill = newValue? Brushes.Red : Brushes.DarkOrange;
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            settings.isStartedTimer = false;
            settings.dispatcherTimer.Stop();
            SetAutoGenerationButtonState();
            CreateView();
        }

        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _lifeCreator.CreateNextGeneration();
            UpdateView();
            
        }

        private void UpdateView()
        {
            /*
            При каждом обновлении по таймеру необходимо перекрашивать все ячейки, 
            в соответствии с их статусами, полученными через Dto            
            */

            foreach (Rectangle cell in mainLifeGrid.Children)
            {
                //cell.GetBindingExpression(Rectangle.TagProperty).UpdateTarget();
                cell.Fill = (bool)cell.Tag ? Brushes.Red : Brushes.DarkOrange;                
            }
        }

        private void CreateView()
        {
            // Получаем размерность поля игры
            var width = _lifeCreator.Cells.GetLength(1);
            var height = _lifeCreator.Cells.GetLength(0);
            // Очищаем все элементы из игрового поля
            mainLifeGrid.Children.Clear();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle emptyCell = new Rectangle
                    {
                        // Задаем отступы между клетками 2px
                        Width = mainLifeGrid.ActualWidth / width - 2.0,
                        Height = mainLifeGrid.ActualWidth / height - 2.0
                    };                    

                    // Выполняем привязку для двухстороннего обмена с формой
                    var t = emptyCell.SetBinding(Rectangle.TagProperty, new System.Windows.Data.Binding(nameof(CellValueDto.Value))
                    {
                        Source = _lifeCreator.Cells[i, j],
                        Mode = System.Windows.Data.BindingMode.TwoWay,
                        UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged,
                        NotifyOnTargetUpdated = true,
                        
                    });

                    mainLifeGrid.Children.Add(emptyCell);
                    Canvas.SetLeft(emptyCell, j * mainLifeGrid.ActualWidth / width);
                    Canvas.SetTop(emptyCell, i * mainLifeGrid.ActualWidth / height);
                    // Делаем каждую ячейку кликабельной, подписывая на событие 
                    emptyCell.MouseDown += EmptyCell_MouseDown;                    
                    emptyCell.Fill = (bool)emptyCell.Tag ? Brushes.Red : Brushes.DarkOrange;
                }
            }
        }             

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (settings.isStartedTimer)
            {
                settings.dispatcherTimer.Stop();               
                settings.isStartedTimer = false;
                CheckActiveBtnSaveGame();
            }
            else
            {
                settings.dispatcherTimer.Start();                
                settings.isStartedTimer = true;
                CheckActiveBtnSaveGame();
            }

            SetAutoGenerationButtonState();
        }

       /// <summary>
       /// Метод активации\деактивации Сохранения игры
       /// </summary>
        private void CheckActiveBtnSaveGame()
        {
            if (settings.isStartedTimer)
                btn_SaveGame.Visibility = Visibility.Hidden;
            else
                btn_SaveGame.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Метод изменения названия кнопки Запуска/Остановки генерации поколений
        /// </summary>
        private void SetAutoGenerationButtonState()
        {
            btn_StartStop.IsEnabled = true;
            btn_StartStop.Content = "Запуск генерации";
            if (settings.isStartedTimer == true) btn_StartStop.Content = "Остановка генерации";
        }       

        private void btn_SaveGame_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
