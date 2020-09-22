namespace GameLifeWpf.GameStateModel
{
    public class Cell
    {
        public int Id { get; set; }
        public int? GenerationId { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public bool CellValue { get; set; }

        public Generation Generation { get; set; }
    }
}