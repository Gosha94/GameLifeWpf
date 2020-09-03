using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using GameLifeWpf.Classes;

namespace GameLifeWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        private static GameController settings = GameController.getInstance();
        private LifeCreator lifeCreator = new LifeCreator();

        public MainWindow()
        {
            InitializeComponent();
            chkBx_RandomState.DataContext = lifeCreator.IsRandom;
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
            var isRandomLife = CheckRandomState();

            for (int i = 0; i < lifeCreator.Fields.GetLength(0); i++)
            {
                for (int j = 0; j < lifeCreator.Fields.GetLength(1); j++)
                {
                    Rectangle emptyCell = new Rectangle
                    {
                        // Задаем отступы между клетками 2px
                        Width = mainLifeGrid.ActualWidth / gameSettings._numberofCellInWidth - 2.0,
                        Height = mainLifeGrid.ActualWidth / gameSettings._numberofCellInHeight - 2.0
                    };

                    // Если выбрана опция "Случайная расстановка"
                    if (isRandomLife)
                    {
                        // Закрашиваем случайную клетку цветом
                        emptyCell.Fill = (gameSettings.random.Next(0, 2) == 1) ? Brushes.DarkOrange : Brushes.Red;
                    }
                    else
                    {
                        // Закрашиваем стандартным для пустых ячеек цветом
                        emptyCell.Fill = Brushes.DarkOrange;
                    }
                    mainLifeGrid.Children.Add(emptyCell);
                    Canvas.SetLeft(emptyCell, j * mainLifeGrid.ActualWidth / gameSettings._numberofCellInWidth);
                    Canvas.SetTop(emptyCell, i * mainLifeGrid.ActualWidth / gameSettings._numberofCellInHeight);
                    // Делаем каждую ячейку кликабельной, подписывая на событие 
                    emptyCell.MouseDown += EmptyCell_MouseDown;
                    // Заполняем массив для второго поколения клеток
                    gameSettings.fields[i, j] = emptyCell;
                }
            }

            LifeCreator.CreateFirstLife(  CheckRandomState(),  mainLifeGrid);            
        }

        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            CreateNextGeneration();
        }

        /// <summary>
        /// Метод 
        /// </summary>
        private void SetAutoGenerationButtonState()
        {
            btn_StartStop.IsEnabled = true;
            btn_StartStop.Content = "Запуск генерации";
            if (settings.isStartedTimer == true) btn_StartStop.Content = "Остановка генерации";
        }

        private bool CheckRandomState()
        {
            if (chkBx_RandomState.IsChecked == true) return true;
            else return false;
        }        

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (settings.isStartedTimer)
            {
                settings.dispatcherTimer.Stop();
                settings.isStartedTimer = false;
            }
            else
            {
                settings.dispatcherTimer.Start();
                settings.isStartedTimer = true;
            }
            
            SetAutoGenerationButtonState();
        }               
    }
}
