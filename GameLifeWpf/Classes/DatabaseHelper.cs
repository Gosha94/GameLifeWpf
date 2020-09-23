using GameLifeWpf.GameStateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLifeWpf.Classes
{
    class DatabaseHelper
    {
        public static void SaveGame(LifeCreator actualLifeCreatorObject)
        {
            using (GameStateContext gameStateContxt = new GameStateContext())
            {
                // Создание и добавление моделей
                Generation generation = new Generation { GenerationNumber = actualLifeCreatorObject.GenerationNumber, GenerationBirthDate = actualLifeCreatorObject.BirthGenerationDate };

                gameStateContxt.Generations.Add(generation);
                gameStateContxt.SaveChanges();

                // Получаем размерность массива ячеек игрового поля
                var width = actualLifeCreatorObject.Cells.GetLength(1);
                var height = actualLifeCreatorObject.Cells.GetLength(0);

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Cell cell = new Cell { CoordX = i, CoordY = j, Generation = generation, CellValue = actualLifeCreatorObject.Cells[i,j].Value };
                        gameStateContxt.Cells.Add ( cell );
                    }
                }
                gameStateContxt.SaveChanges();                             
                
            }
        }
    }
}
