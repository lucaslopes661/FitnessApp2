using SQLite;

namespace FitnessApp2.Models
{
    public class Exercicio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Duracao { get; set; } // em minutos
        public string Intensidade { get; set; } = string.Empty; // leve, moderada, intensa
        public double Calorias { get; set; } // calorias gastas
        public DateTime Data { get; set; }
    }
}
