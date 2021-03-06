﻿using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameLifeWpf.Classes
{
    class CellValueDto
    {
        public bool Value { get; set; }
        public CellValueDto(bool value = false)
        {
            Value = value;
        }
    }
    class LifeCreator
    {
        //private static GameController gameSettings = GameController.getInstance();
        // Макс кол-во строк в канвасе
        private int _numberofCellInWidth = 40;
        // Макс кол-во столбцов в канвасе
        private int _numberofCellInHeight = 40;

        public CellValueDto[,] Cells { get; private set; }

        private bool _isRandom ;
        public bool? IsRandom { get { return _isRandom; }
            set {
                _isRandom = value??false;
                var random = new Random();
                for (int i = 0; i < _numberofCellInHeight; i++)
                {
                    for (int j = 0; j < _numberofCellInWidth; j++)
                    {
                        if (_isRandom)
                        {
                            Cells[i, j].Value = (random.Next(0, 3) == 1) ? true : false;
                        }
                        else Cells[i, j].Value = false;
                    }
                }
            } }

        public LifeCreator()
        {
            Cells = new CellValueDto[_numberofCellInWidth, _numberofCellInHeight];
            // Заполняем игровое поле 
            for (int i = 0; i < _numberofCellInHeight; i++)
            {
                for (int j = 0; j < _numberofCellInWidth; j++)
                {
                    Cells[i, j] = new CellValueDto();
                }
            }
        }


        public void CreateNextGeneration()
        {
            int[,] numberOfNeighbors = new int[_numberofCellInHeight, _numberofCellInWidth];

            for (int i = 0; i < _numberofCellInHeight; i++)
            {
                for (int j = 0; j < _numberofCellInWidth; j++)
                {
                    int neighboor = 0;
                    int heigOnY = i - 1;
                    int bottomOnY = i + 1;
                    int leftOnX = j - 1;
                    int righOnX = j + 1;

                    /* Задаем переходы проверки соседей по бесконечному полю,
                     Если клетка находится в угловых рядах поля, то сравниваются соседи из противоположных угловых рядов */
                    if (heigOnY < 0) { heigOnY = _numberofCellInHeight - 1; }
                    if (bottomOnY >= _numberofCellInHeight) { bottomOnY = 0; }
                    if (leftOnX < 0) { leftOnX = _numberofCellInWidth - 1; }
                    if (righOnX >= _numberofCellInWidth) { righOnX = 0; }

                    neighboor = (Cells[heigOnY, leftOnX].Value ? 1 : 0)
                        + (Cells[heigOnY, j].Value ? 1 : 0        )
                        + (Cells[heigOnY, righOnX].Value ? 1 : 0  )
                        + (Cells[i, leftOnX].Value ? 1 : 0        )
                        + (Cells[i, righOnX].Value ? 1 : 0        )
                        + (Cells[bottomOnY, leftOnX].Value ? 1 : 0)
                        + (Cells[bottomOnY, j].Value ? 1 : 0      )
                        + (Cells[bottomOnY, righOnX].Value ? 1 : 0); 


                    numberOfNeighbors[i, j] = neighboor;
                }
            }
            // Создаем новое поколение клеток на основании количества соседей
            for (int i = 0; i < _numberofCellInHeight; i++)
            {
                for (int j = 0; j < _numberofCellInWidth; j++)
                {
                    if (numberOfNeighbors[i, j] < 2 || numberOfNeighbors[i, j] > 3)
                    {
                        Cells[i, j].Value = false;
                    }
                    else if (numberOfNeighbors[i, j] == 3)
                        Cells[i, j].Value = true;

                }
            }
        }
    }
}
