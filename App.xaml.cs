﻿namespace FitnessApp2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override async void OnStart()
        {
            base.OnStart();
            await DatabaseService.InitializeAsync();
        }
    }
}
