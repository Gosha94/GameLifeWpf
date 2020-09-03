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

        private void EmptyCell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = (Rectangle)sender;
            var value = (bool)cell.GetValue();
            var newValue = !value;
            cell.SetValue(, newvalue);
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
            var isRandomLife = CheckRandomState();
            var width = lifeCreator.Fields.GetLength(1);
            var height = lifeCreator.Fields.GetLength(0);

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
                    emptyCell.DataContext = lifeCreator.Fields[i, j];
                    mainLifeGrid.Children.Add(emptyCell);
                    Canvas.SetLeft(emptyCell, j * mainLifeGrid.ActualWidth / width);
                    Canvas.SetTop(emptyCell, i * mainLifeGrid.ActualWidth / height);
                    // Делаем каждую ячейку кликабельной, подписывая на событие 
                    emptyCell.MouseDown += EmptyCell_MouseDown;

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
