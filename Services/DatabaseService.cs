using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using FitnessApp2.Models;
using System.Diagnostics;

namespace FitnessApp2
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _database = null!;

        // Inicializa o banco de dados
        public static async Task InitializeAsync()
        {
            if (_database == null)
            {
                try
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitnessApp2.db");
                    _database = new SQLiteAsyncConnection(dbPath);
                    Debug.WriteLine($"Banco criado em: {dbPath}");
                    await CreateTablesAsync();
                    Debug.WriteLine("Tabelas criadas com sucesso.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro ao inicializar o banco: {ex.Message}");
                }
            }
        }

        // Cria as tabelas do banco
        private static async Task CreateTablesAsync()
        {
            await _database.CreateTableAsync<Meta>();
            await _database.CreateTableAsync<Exercicio>();
            await _database.CreateTableAsync<Refeicao>();
            await _database.CreateTableAsync<Alimento>();
            await CarregarDadosIniciais(); // Carrega os dados iniciais
        }

        private static async Task CarregarDadosIniciais()
        {
            try
            {
                var count = await _database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Alimento");
                if (count == 0) // Apenas insere se a tabela estiver vazia
                {
                    var alimentos = new List<Alimento>
            {
                new Alimento { Nome = "Arroz branco cozido", CaloriasPorGrama = 1.28 },
                new Alimento { Nome = "Arroz integral cozido", CaloriasPorGrama = 1.24 },
                new Alimento { Nome = "Farinha de mandioca torrada", CaloriasPorGrama = 3.65 },
                new Alimento { Nome = "Farinha de milho", CaloriasPorGrama = 3.51 },
                new Alimento { Nome = "Milho cozido (em grãos)", CaloriasPorGrama = 1.06 },
                new Alimento { Nome = "Pão francês", CaloriasPorGrama = 2.71 },
                new Alimento { Nome = "Macarrão cozido", CaloriasPorGrama = 1.58 },
                new Alimento { Nome = "Feijão preto cozido", CaloriasPorGrama = 0.76 },
                new Alimento { Nome = "Feijão carioca cozido", CaloriasPorGrama = 0.77 },
                new Alimento { Nome = "Lentilha cozida", CaloriasPorGrama = 0.93 },
                new Alimento { Nome = "Grão-de-bico cozido", CaloriasPorGrama = 1.19 },
                new Alimento { Nome = "Ervilha seca cozida", CaloriasPorGrama = 0.82 },
                new Alimento { Nome = "Batata inglesa cozida", CaloriasPorGrama = 0.52 },
                new Alimento { Nome = "Batata-doce cozida", CaloriasPorGrama = 0.77 },
                new Alimento { Nome = "Mandioca cozida (aipim)", CaloriasPorGrama = 1.25 },
                new Alimento { Nome = "Inhame cozido", CaloriasPorGrama = 0.97 },
                new Alimento { Nome = "Cará cozido", CaloriasPorGrama = 0.97 },
                new Alimento { Nome = "Carne bovina (patinho grelhado)", CaloriasPorGrama = 2.40 },
                new Alimento { Nome = "Frango grelhado (peito sem pele)", CaloriasPorGrama = 1.65 },
                new Alimento { Nome = "Peixe (tilápia grelhada)", CaloriasPorGrama = 0.96 },
                new Alimento { Nome = "Ovo de galinha cozido", CaloriasPorGrama = 1.46 },
                new Alimento { Nome = "Couve refogada", CaloriasPorGrama = 0.80 },
                new Alimento { Nome = "Alface (crua)", CaloriasPorGrama = 0.16 },
                new Alimento { Nome = "Tomate (cru)", CaloriasPorGrama = 0.18 },
                new Alimento { Nome = "Cenoura (crua)", CaloriasPorGrama = 0.34 },
                new Alimento { Nome = "Beterraba (cozida)", CaloriasPorGrama = 0.49 },
                new Alimento { Nome = "Banana prata", CaloriasPorGrama = 0.89 },
                new Alimento { Nome = "Maçã", CaloriasPorGrama = 0.52 },
                new Alimento { Nome = "Mamão papaya", CaloriasPorGrama = 0.43 },
                new Alimento { Nome = "Abacaxi", CaloriasPorGrama = 0.48 },
                new Alimento { Nome = "Laranja-pera (sem casca)", CaloriasPorGrama = 0.46 },
                new Alimento { Nome = "Abacate", CaloriasPorGrama = 1.60 },
                new Alimento { Nome = "Manga", CaloriasPorGrama = 0.60 },
                new Alimento { Nome = "Leite integral (líquido)", CaloriasPorGrama = 0.61 },
                new Alimento { Nome = "Leite desnatado (líquido)", CaloriasPorGrama = 0.36 },
                new Alimento { Nome = "Queijo muçarela", CaloriasPorGrama = 2.80 },
                new Alimento { Nome = "Queijo minas frescal", CaloriasPorGrama = 2.40 },
                new Alimento { Nome = "Óleo de soja", CaloriasPorGrama = 8.84 },
                new Alimento { Nome = "Manteiga", CaloriasPorGrama = 7.17 },
                new Alimento { Nome = "Açúcar refinado", CaloriasPorGrama = 3.87 },
                new Alimento { Nome = "Suco de laranja natural", CaloriasPorGrama = 0.45 },
                new Alimento { Nome = "Coca-Cola (original)", CaloriasPorGrama = 0.37 },
                new Alimento { Nome = "Guaraná Antarctica Zero", CaloriasPorGrama = 0.02 },
                new Alimento { Nome = "Peito de Frango grelhado", CaloriasPorGrama = 1.65 },
                new Alimento { Nome = "Tilápia grelhada", CaloriasPorGrama = 0.96 },
                new Alimento { Nome = "Salmão grelhado", CaloriasPorGrama = 2.06 }
            };

                    await _database.InsertAllAsync(alimentos);
                    Debug.WriteLine("Lista de alimentos básicos carregada com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao inserir lista de alimentos: {ex.Message}");
            }
        }

        // Obtém a conexão com o banco
        public static SQLiteAsyncConnection GetConnection() => _database;
    }
}
