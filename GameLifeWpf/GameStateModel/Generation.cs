using System;
using System.Collections.Generic;

namespace GameLifeWpf.GameStateModel
{
    public class Generation
    {
        public int Id { get; set; }
        public int GenerationNumber { get; set; }
        public DateTime GenerationBirthDate { get; set; }
        public ICollection < Cell > Cells { get; set; }
        public Generation ()
        {
            Cells = new List < Cell > ();
        }
    }
}
