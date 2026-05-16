using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class StocView : Window
    {
        private PreparatBLL preparatBLL = new PreparatBLL();
        private ObservableCollection<Preparat> listaPreparate = new();
        private List<Preparat> preparateOriginale = new();

        public StocView()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };

            IncarcaDate();
        }

        private void IncarcaDate()
        {
            try
            {
                listaPreparate = preparatBLL.GetAllPreparate();
                
                // Pastram copiile originale pentru a stii ce s-a modificat
                preparateOriginale = listaPreparate.Select(p => new Preparat
                {
                    IdPreparat = p.IdPreparat,
                    CantitateTotala = p.CantitateTotala
                }).ToList();

                preparateList.ItemsSource = listaPreparate;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea produselor: {ex.Message}", 
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Inchide_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int modificate = 0;
                foreach (var preparat in listaPreparate)
                {
                    var original = preparateOriginale.FirstOrDefault(p => p.IdPreparat == preparat.IdPreparat);
                    
                    if (original != null && original.CantitateTotala != preparat.CantitateTotala)
                    {
                        if (preparat.CantitateTotala < 0)
                        {
                            MessageBox.Show($"Cantitatea pentru '{preparat.Denumire}' nu poate fi negativa.", 
                                "Eroare validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        preparatBLL.UpdateStoc(preparat.IdPreparat, preparat.CantitateTotala);
                        modificate++;
                    }
                }

                if (modificate > 0)
                {
                    MessageBox.Show($"Stocul a fost actualizat cu succes pentru {modificate} produse!", 
                        "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nu a fost modificata nicio cantitate.", 
                        "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A aparut o eroare la salvare: {ex.Message}", 
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
