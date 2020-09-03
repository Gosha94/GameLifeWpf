using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameLifeWpf.Classes
{
    class LifeCreator
    {
        private static GameController gameSettings = GameController.getInstance();
        public Rectangle[,] playArea = gameSettings.fields;


        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            CreateNextGeneration();            
        }        
        
        public static void CreateFirstLife(bool isRandomLife, Canvas lifeGrid)
        {
            // Заполняем игровое поле пустыми ячейками
            for (int i = 0; i < gameSettings.numberofCellInHeight; i++)
            {
                for (int j = 0; j < gameSettings.numberofCellInWidth; j++)
                {
                    Rectangle emptyCell = new Rectangle
                    {
                        // Задаем отступы между клетками 2px
                        Width = lifeGrid.ActualWidth / gameSettings.numberofCellInWidth - 2.0,
                        Height = lifeGrid.ActualWidth / gameSettings.numberofCellInHeight - 2.0
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
                    lifeGrid.Children.Add(emptyCell);
                    Canvas.SetLeft(emptyCell, j * lifeGrid.ActualWidth / gameSettings.numberofCellInWidth);
                    Canvas.SetTop(emptyCell, i * lifeGrid.ActualWidth / gameSettings.numberofCellInHeight);
                    // Делаем каждую ячейку кликабельной, подписывая на событие 
                    emptyCell.MouseDown += EmptyCell_MouseDown;
                    // Заполняем массив для второго поколения клеток
                    gameSettings.fields[i, j] = emptyCell;
                }
            }
        }

        private static void EmptyCell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = 
                (((Rectangle)sender).Fill == Brushes.DarkOrange) ? Brushes.Red : Brushes.DarkOrange;
        }

        public static void CreateNextGeneration()
        {
            int[,] numberOfNeighbors = new int[gameSettings.numberofCellInHeight, gameSettings.numberofCellInWidth];

            for (int i = 0; i < gameSettings.numberofCellInHeight; i++)
            {
                for (int j = 0; j < gameSettings.numberofCellInWidth; j++)
                {
                    int neighboor = 0;
                    int heigOnY = i - 1;
                    int bottomOnY = i + 1;
                    int leftOnX = j - 1;
                    int righOnX = j + 1;

                    /* Задаем переходы проверки соседей по бесконечному полю,
                     Если клетка находится в угловых рядах поля, то сравниваются соседи из противоположных угловых рядов */
                    if (heigOnY < 0) { heigOnY = gameSettings.numberofCellInHeight - 1; }
                    if (bottomOnY >= gameSettings.numberofCellInHeight) { bottomOnY = 0; }
                    if (leftOnX < 0) { leftOnX = gameSettings.numberofCellInWidth - 1; }
                    if (righOnX >= gameSettings.numberofCellInWidth) { righOnX = 0; }
                    // Проверяем наличие соседей с каждой из 8 сторон от клетки
                    // V основная клетка, для которой проверяем соседей
                    // Х клетка, в которой проверяем соседа

                    /*
                        X**
                        *V*
                        ***
                     */
                    if (gameSettings.fields[heigOnY, leftOnX].Fill == Brushes.Red) { neighboor++; }

                    /*
                        *X*
                        *V*
                        ***
                     */
                    if (gameSettings.fields[heigOnY, j].Fill == Brushes.Red) { neighboor++; }

                    /*
                        **Х
                        *V*
                        ***
                     */
                    if (gameSettings.fields[heigOnY, righOnX].Fill == Brushes.Red) { neighboor++; }

                    /*
                        ***
                        XV*
                        ***
                     */
                    if (gameSettings.fields[i, leftOnX].Fill == Brushes.Red) { neighboor++; }

                    /*
                        ***
                        *VX
                        ***
                     */
                    if (gameSettings.fields[i, righOnX].Fill == Brushes.Red) { neighboor++; }

                    /*
                        ***
                        *V*
                        X**
                     */
                    if (gameSettings.fields[bottomOnY, leftOnX].Fill == Brushes.Red) { neighboor++; }

                    /*
                        ***
                        *V*
                        *X*
                     */
                    if (gameSettings.fields[bottomOnY, j].Fill == Brushes.Red) { neighboor++; }

                    /*
                        ***
                        *V*
                        **X
                     */
                    if (gameSettings.fields[bottomOnY, righOnX].Fill == Brushes.Red) { neighboor++; }

                    numberOfNeighbors[i, j] = neighboor;
                }
            }
            // Создаем новое поколение клеток на основании количества соседей
            for (int i = 0; i < gameSettings.numberofCellInHeight; i++)
            {
                for (int j = 0; j < gameSettings.numberofCellInWidth; j++)
                {
                    if (numberOfNeighbors[i, j] < 2 || numberOfNeighbors[i, j] > 3)
                    {
                        gameSettings.fields[i, j].Fill = Brushes.DarkOrange;
                    }
                    else if (numberOfNeighbors[i, j] == 3)
                        gameSettings.fields[i, j].Fill = Brushes.Red;
                }
            }
        }
    }
}
