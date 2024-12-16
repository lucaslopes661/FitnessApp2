using RestSharp;
using RestSharp.Authenticators;
using FitnessApp2.Models;
using System.Diagnostics;

namespace FitnessApp2.Services
{
    public static class FoodService
    {
        private const string ApiUrl = "https://platform.fatsecret.com/rest/server.api";
        private const string ConsumerKey = "fdf8e9a4044e438aa224c07e5ae50d0e";  // Substitua pela sua Consumer Key
        private const string ConsumerSecret = "a98af9fb7a064311aade0caf1e9e1fcf"; // Substitua pela sua Consumer Secret

        public static async Task<List<Alimento>> BuscarAlimentosAsync(string query)
        {
            var alimentos = new List<Alimento>();

            try
            {
                var options = new RestClientOptions(ApiUrl)
                {
                    Authenticator = OAuth1Authenticator.ForRequestToken(ConsumerKey, ConsumerSecret)
                };

                var client = new RestClient(options);

                var request = new RestRequest();
                request.AddParameter("method", "foods.search");
                request.AddParameter("format", "json");
                request.AddParameter("search_expression", query);

                var response = await client.ExecuteAsync<ApiResponse>(request);

                // Verificar a resposta
                if (response.IsSuccessful && response.Data?.foods?.food != null)
                {
                    foreach (var item in response.Data.foods.food)
                    {
                        double calorias = ExtrairCalorias(item.food_description);

                        alimentos.Add(new Alimento
                        {
                            Nome = item.food_name,
                            CaloriasPorGrama = calorias / 100.0 // Dividir por 100 para calorias por grama
                        });
                    }
                }
                else
                {
                    Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                    Console.WriteLine(response.Content);
                    throw new Exception($"Erro na requisição: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar alimentos: {ex.Message}");
            }

            return alimentos;
        }

        // Função para extrair calorias da descrição
        private static double ExtrairCalorias(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                return 0;

            try
            {
                // Procurar por "Calories: " e capturar o número antes de "kcal"
                var match = System.Text.RegularExpressions.Regex.Match(descricao, @"Calories:\s*(\d+)");
                if (match.Success && double.TryParse(match.Groups[1].Value, out double calorias))
                {
                    return calorias;
                }
            }
            catch
            {
                // Log de erro em caso de falha na extração
                Console.WriteLine($"Erro ao extrair calorias de: {descricao}");
            }

            return 0; // Retorna 0 se não conseguir extrair
        }

        // Classes auxiliares
        public class ApiResponse
        {
            public FoodList foods { get; set; }
        }

        public class FoodList
        {
            public List<FoodItem> food { get; set; }
        }

        public class FoodItem
        {
            public string food_name { get; set; }
            public string food_description { get; set; } // Adicione esta linha
        }
    }
}


