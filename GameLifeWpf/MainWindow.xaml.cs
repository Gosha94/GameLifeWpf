using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
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
            // Устанавливаем привязку поля случайная расстановка к форме
            chkBx_RandomState.SetBinding(CheckBox.IsCheckedProperty,
                new Binding(nameof(LifeCreator.IsRandom))
                {
                    Source = _lifeCreator,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                });

            //// Устанавливаем привязку поля с номером поколения к форме
            //lbl_GenerationNumber.SetBinding(Label.ContentProperty,
            //    new Binding(nameof(LifeCreator.GenerationNumber))
            //    {
            //        Source = _lifeCreator,
            //        Mode = BindingMode.TwoWay,
            //        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            //    });
        }
        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            settings.isStartedTimer = false;
            settings.dispatcherTimer.Stop();
            SetAutoGenerationButtonState();
            CheckActiveBtnSaveGame();
            CreateView();
        }
        private void EmptyCell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = (Rectangle)sender;
            var value = (bool)(cell.Tag);
            var newValue = !value;
            cell.Tag = newValue;
            cell.Fill = newValue? Brushes.Red : Brushes.DarkOrange;
        }
        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _lifeCreator.CreateNextGeneration();            
            UpdateView();
            lbl_GenerationNumber.Content = Convert.ToString(_lifeCreator.GenerationNumber);
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

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

        private void btn_SaveGame_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.SaveGame(_lifeCreator);
        }
    }
}
