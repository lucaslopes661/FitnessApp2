using SQLite;

namespace FitnessApp2.Models
{
    public class Meta
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double PesoAtual { get; set; }
        public double PesoObjetivo { get; set; }
        public DateTime DataFinal { get; set; }
        public double TMB { get; set; } // Taxa Metabólica Basal
    }
}
