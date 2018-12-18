﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ml.Doki.Helpers;
using Windows.UI.Popups;

namespace ml.Doki.ViewModels
{
    public class ConfigurationViewModel : Observable
    {
        private string _aboutText;
        public string AboutText
        {
            get => _aboutText;
            set => Set(ref _aboutText, value);
        }

        
        private ObservableCollection<string> _availableLocales;
        public ObservableCollection<string> AvailableLocales
        {
            get => _availableLocales;
            set => Set(ref _availableLocales, value);
        }


        private string _selectedCurrencyLocale;
        public string SelectedCurrencyLocale
        {
            get => _selectedCurrencyLocale;
            set => Set(ref _selectedCurrencyLocale, value);
        }


        public ICommand LoadCommand { get; }

        public ICommand SaveCommand { get; }


        public ConfigurationViewModel()
        {
            LoadCommand = new RelayCommand(Load);
            SaveCommand = new RelayCommand(Save);

            LoadCommand.Execute(null);
        }

        public async void Load()
        {
            await Singleton<Settings>.Instance.InitializeAsync();

            AboutText = Singleton<Settings>.Instance.AboutText;
            AvailableLocales = new ObservableCollection<string>(await Singleton<Settings>.Instance.GetAllAvailableCultureNamesAsync());
            SelectedCurrencyLocale = Singleton<Settings>.Instance.ApplicationCultureName;
        }

        public async void Save()
        {
            await Singleton<Settings>.Instance.SetAboutTextAsync(this.AboutText);
            await Singleton<Settings>.Instance.SetApplicationCultureNameAsync(this.SelectedCurrencyLocale);

            await new MessageDialog("Settings were saved and will be applied when you restart the app.").ShowAsync();
        }
    }
}