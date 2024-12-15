using SQLite;


namespace FitnessApp2.Models
{
    public class Refeicao
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NomeAlimento { get; set; } = string.Empty;
        public double Calorias { get; set; } // calorias ingeridas
        public DateTime DataHora { get; set; }
    }
}
