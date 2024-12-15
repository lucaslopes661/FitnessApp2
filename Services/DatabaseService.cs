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
        }

        // Obtém a conexão com o banco
        public static SQLiteAsyncConnection GetConnection() => _database;
    }
}
