﻿using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameLifeWpf.Classes
{
    class val
    {
        public bool value { get; set; }
        public val(bool val = false)
        {
            value = val;
        }
    }
    class LifeCreator
    {
        //private static GameController gameSettings = GameController.getInstance();
        // Макс кол-во строк в канвасе
        private int _numberofCellInWidth = 40;
        // Макс кол-во столбцов в канвасе
        private int _numberofCellInHeight = 40;

        public val[,] Fields { get; private set; }

        private bool _isRandom;
        public bool IsRandom { get { return _isRandom; }
            set {
                _isRandom = value;
                for (int i = 0; i < _numberofCellInHeight; i++)
                {
                    for (int j = 0; j < _numberofCellInWidth; j++)
                    {
                        if (_isRandom)
                        {
                            var random = new Random();
                            Fields[i, j].value = (random.Next(0, 1) == 1) ? true : false;
                        }
                        else Fields[i, j].value = false;
                    }
                }
            } }

        public LifeCreator()
        {
            Fields = new val[_numberofCellInWidth, _numberofCellInHeight];
            // Заполняем игровое поле 
            for (int i = 0; i < _numberofCellInHeight; i++)
            {
                for (int j = 0; j < _numberofCellInWidth; j++)
                {
                    Fields[i, j] = new val();
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

                    if (Fields[heigOnY, leftOnX].value || Fields[heigOnY, j].value
                        || Fields[heigOnY, righOnX].value || Fields[i, leftOnX].value
                        || Fields[i, righOnX].value || Fields[bottomOnY, leftOnX].value
                        || Fields[bottomOnY, j].value || Fields[bottomOnY, righOnX].value) 
                        neighboor++; 

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
                        Fields[i, j].value = false;
                    }
                    else if (numberOfNeighbors[i, j] == 3)
                        Fields[i, j].value = true;
                }
            }
        }
    }
}
