using FitnessApp2.Models;
using System;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace FitnessApp2;

public partial class ConfigurarMetaPage : ContentPage
{
	public ConfigurarMetaPage()
	{
		InitializeComponent();
	}
    // Evento do botão "Salvar Meta"
    private async void OnSalvarMetaClicked(object sender, EventArgs e)
    {
        // Validação dos campos
        if (string.IsNullOrWhiteSpace(PesoAtualEntry.Text) ||
        string.IsNullOrWhiteSpace(PesoObjetivoEntry.Text) ||
        string.IsNullOrWhiteSpace(AlturaEntry.Text) ||
        string.IsNullOrWhiteSpace(IdadeEntry.Text) ||
        SexoPicker.SelectedItem == null)
        {
            await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
            return;
        }

        // Convertendo os valores
        double pesoAtual = double.Parse(PesoAtualEntry.Text);
        double pesoObjetivo = double.Parse(PesoObjetivoEntry.Text);
        int altura = int.Parse(AlturaEntry.Text);
        int idade = int.Parse(IdadeEntry.Text);
        string sexo = SexoPicker.SelectedItem.ToString();
        DateTime dataFinal = DataFinalPicker.Date;

        if (SexoPicker.SelectedItem == null)
        {
            await DisplayAlert("Erro", "Por favor, selecione o sexo.", "OK");
            return;
        }

        double tmb = CalcularTMB(pesoAtual, altura, idade, sexo);

        // Criar objeto Meta
        var novaMeta = new Meta
        {
            PesoAtual = pesoAtual,
            PesoObjetivo = pesoObjetivo,
            DataFinal = dataFinal,
            TMB = tmb,
            Sexo = sexo,
            Idade = idade,
            Altura = altura
        };

        // Salvar no banco de dados
        var db = DatabaseService.GetConnection();
        await db.InsertOrReplaceAsync(novaMeta);
        Debug.WriteLine("Meta salva no banco com sucesso!");

        await DisplayAlert("Sucesso", "Meta salva com sucesso!", "OK");
        await Navigation.PopAsync();
    }

    // Método para calcular a TMB (simplificado por enquanto)
    private double CalcularTMB(double peso, int altura, int idade, string sexo)
    {
        if (sexo == "Masculino")
        {
            return 88.36 + (13.4 * peso) + (4.8 * altura) - (5.7 * idade);
        }
        else // Feminino
        {
            return 447.6 + (9.2 * peso) + (3.1 * altura) - (4.3 * idade);
        }
    }
}

