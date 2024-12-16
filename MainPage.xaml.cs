using FitnessApp2.Models;
using System.Diagnostics;


namespace FitnessApp2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeDatabase();

        }

        private async void InitializeDatabase()
        {
            await DatabaseService.InitializeAsync(); // Inicializa o banco
            CarregarResumo(); // Carrega os dados após inicializar
        }


        // Método para carregar dados iniciais na tela
        private async void CarregarResumo()
        {
            try
            {
                var db = DatabaseService.GetConnection();

                // Buscar a primeira meta salva no banco
                var meta = await db.Table<Meta>().FirstOrDefaultAsync();

                if (meta != null)
                {
                    // Exibir os valores na tela
                    Debug.WriteLine($"Peso Atual: {meta.PesoAtual}, TMB: {meta.TMB}"); // Log temporário
                    PesoAtualLabel.Text = $"{meta.PesoAtual} kg";
                    
                    MetaCaloriasLabel.Text = $"{meta.TMB} kcal";
                    
                    CaloriasLabel.Text = "Ingeridas: -- kcal | Gastas: -- kcal"; // Atualizaremos depois

                    AlturaLabel.Text = $"{meta.Altura} cm";
                    IdadeLabel.Text = $"{meta.Idade} anos";
                    SexoLabel.Text = $"{meta.Sexo}";
                }
                else
                {
                    // Caso não exista nenhuma meta
                    Debug.WriteLine("Nenhum dado encontrado na tabela Meta.");
                    PesoAtualLabel.Text = "-- kg";
                    MetaCaloriasLabel.Text = "-- kcal";
                    CaloriasLabel.Text = "Ingeridas: -- kcal | Gastas: -- kcal";
                    AlturaLabel.Text = "-- cm";
                    IdadeLabel.Text = "-- anos";
                    SexoLabel.Text = "--";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao carregar os dados: {ex.Message}", "OK");
            }
        }

        // Navegação ao clicar nos botões
        private void OnExerciciosClicked(object sender, EventArgs e)
        {
            DisplayAlert("Exercícios", "Navegar para tela de exercícios", "OK");
        }

        private void OnRefeicoesClicked(object sender, EventArgs e)
        {
            DisplayAlert("Refeições", "Navegar para tela de refeições", "OK");
        }

        private void OnRelatoriosClicked(object sender, EventArgs e)
        {
            DisplayAlert("Relatórios", "Navegar para tela de relatórios", "OK");
        }

        private async void OnConfigurarMetaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConfigurarMetaPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing(); // Chama o método padrão da classe base
            CarregarResumo();   // Recarrega os dados da tela principal
        }

        private async void OnCalculadoraCaloriasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CalculadoraCaloriasPage());
        }
    }
}
