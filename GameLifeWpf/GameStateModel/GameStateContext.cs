using System.Data.Entity;

namespace GameLifeWpf.GameStateModel
{
    /// <summary>
    /// Наследуемый класс DbContext определяет контекст данных, используемый для взаимодействия с базой данных.
    /// </summary>
    class GameStateContext : DbContext
    {
        /// <summary>
        /// Конструктор класса, в котором вызывается конструктор базового класса, 
        /// в который передается строка "GameStateData" - это имя строки подключения к базе данных,
        /// соответствует тегу <connectionStrings><add =name  из файла App.config
        /// </summary>
        public GameStateContext () : base ( "GameStateData" )
        { }

        public DbSet < Cell > Cells { get; set; }
        public DbSet < Generation > Generations { get; set; }
    }
}
