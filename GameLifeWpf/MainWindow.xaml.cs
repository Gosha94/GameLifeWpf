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

        public MainWindow()
        {
            InitializeComponent();
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
            LifeCreator.CreateFirstLife(  CheckRandomState(),  mainLifeGrid);            
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
