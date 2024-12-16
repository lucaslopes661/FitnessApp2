using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FitnessApp2.Models
{
    public class Alimento
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public double CaloriasPorGrama { get; set; }

        public override string ToString()
        {
            return $"{Nome} - {CaloriasPorGrama:F2} kcal";
        }
    }
}

