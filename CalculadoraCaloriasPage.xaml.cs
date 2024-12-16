using System.Collections.ObjectModel;
using FitnessApp2.Models;
using FitnessApp2.Services;
using SQLite;

namespace FitnessApp2;

public partial class CalculadoraCaloriasPage : ContentPage
{
    private readonly ObservableCollection<Alimento> ResultadosBusca = new ObservableCollection<Alimento>();
    private readonly List<Alimento> alimentosAdicionados = new List<Alimento>();
    private SQLiteAsyncConnection _database;

    public CalculadoraCaloriasPage()
    {
        InitializeComponent();
        InicializarBancoDeDados();
        ResultadosListView.ItemsSource = ResultadosBusca;
    }

    private async void InicializarBancoDeDados()
    {
        _database = DatabaseService.GetConnection();
        await CarregarAlimentosAsync(); // Carrega alimentos na inicialização
    }

    // Carrega alimentos do banco para a busca inicial
    private async Task CarregarAlimentosAsync()
    {
        ResultadosBusca.Clear();

        var alimentos = await _database.Table<Alimento>().ToListAsync();
        foreach (var alimento in alimentos)
        {
            ResultadosBusca.Add(alimento);
        }
    }

    // Evento de texto alterado na barra de pesquisa
    private async void OnPesquisaTextChanged(object sender, TextChangedEventArgs e)
    {
        string query = e.NewTextValue?.ToLower();

        ResultadosBusca.Clear();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var resultados = await _database.Table<Alimento>()
                               .Where(a => a.Nome.ToLower().Contains(query))
                               .ToListAsync();

            foreach (var alimento in resultados)
            {
                ResultadosBusca.Add(alimento);
            }
        }
    }

    // Evento para abrir o modal de cadastro
    private async void OnAbrirCadastroAlimentoClicked(object sender, EventArgs e)
    {
        string nome = await DisplayPromptAsync("Cadastrar Alimento", "Digite o nome do alimento:");
        if (string.IsNullOrWhiteSpace(nome)) return;

        string caloriasStr = await DisplayPromptAsync("Cadastrar Alimento", "Digite as calorias por grama (Ex: 1.5):", keyboard: Keyboard.Numeric);
        if (!double.TryParse(caloriasStr, out double calorias) || calorias <= 0)
        {
            await DisplayAlert("Erro", "Calorias inválidas!", "OK");
            return;
        }

        // Adiciona o novo alimento ao banco de dados
        var novoAlimento = new Alimento { Nome = nome, CaloriasPorGrama = calorias };
        await _database.InsertAsync(novoAlimento);

        await DisplayAlert("Sucesso", "Alimento cadastrado com sucesso!", "OK");

        // Atualiza os resultados da busca
        await CarregarAlimentosAsync();
    }

    private async void OnAbrirRemoverAlimentoClicked(object sender, EventArgs e)
    {
        string nome = await DisplayPromptAsync("Remover Alimento", "Digite o nome do alimento que deseja remover:");

        if (string.IsNullOrWhiteSpace(nome))
            return;

        bool confirmacao = await DisplayAlert("Confirmar Remoção", $"Deseja realmente remover '{nome}'?", "Sim", "Não");

        if (confirmacao)
        {
            await RemoverAlimentoPorNomeAsync(nome);
        }
    }

    // Função para remover o alimento do banco de dados
    private async Task RemoverAlimentoPorNomeAsync(string nomeAlimento)
    {
        try
        {
            var alimento = await DatabaseService.GetConnection()
                .Table<Alimento>()
                .Where(a => a.Nome == nomeAlimento)
                .FirstOrDefaultAsync();

            if (alimento != null)
            {
                await DatabaseService.GetConnection().DeleteAsync(alimento);
                await DisplayAlert("Sucesso", $"Alimento '{nomeAlimento}' removido com sucesso!", "OK");
            }
            else
            {
                await DisplayAlert("Erro", $"Alimento '{nomeAlimento}' não encontrado.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao remover o alimento: {ex.Message}", "OK");
        }
    }

    // Evento ao selecionar um alimento
    private void OnAlimentoSelecionado(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Alimento alimentoSelecionado)
        {
            PesquisaEntry.Text = alimentoSelecionado.Nome;
            ResultadosListView.SelectedItem = null;
        }
    }

    // Evento para adicionar alimento à lista
    private async void OnAdicionarAlimentoClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PesquisaEntry.Text) || string.IsNullOrWhiteSpace(QuantidadeEntry.Text))
        {
            await DisplayAlert("Erro", "Preencha todos os campos.", "OK");
            return;
        }

        if (!double.TryParse(QuantidadeEntry.Text, out double quantidade))
        {
            await DisplayAlert("Erro", "Quantidade inválida.", "OK");
            return;
        }

        var alimento = await _database.Table<Alimento>().FirstOrDefaultAsync(a => a.Nome == PesquisaEntry.Text);

        if (alimento != null)
        {
            alimentosAdicionados.Add(new Alimento
            {
                Nome = $"{alimento.Nome} ({quantidade}g)",
                CaloriasPorGrama = alimento.CaloriasPorGrama * quantidade
            });

            ListaAlimentosView.ItemsSource = null;
            ListaAlimentosView.ItemsSource = alimentosAdicionados;

            AtualizarTotalCalorias();
        }
        else
        {
            await DisplayAlert("Erro", "Alimento não encontrado no banco de dados.", "OK");
        }
    }

    private void AtualizarTotalCalorias()
    {
        double totalCalorias = alimentosAdicionados.Sum(a => a.CaloriasPorGrama);
        ResultadoLabel.Text = $"Total de Calorias: {totalCalorias:F2} kcal";
    }
}
