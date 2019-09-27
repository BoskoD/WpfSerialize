using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;

namespace WpfSerijalizacija
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string putanja = "Osobe.json";

        private List<Osoba> listaOsoba = new List<Osoba>();

        private int id = 0;

        private bool Validacija()
        {
            if (string.IsNullOrWhiteSpace(TextBoxIme.Text))
            {
                MessageBox.Show("Niste uneli ime");
                TextBoxIme.Focus();
                return false;
            }
            if (TextBoxIme.Text.Trim().Length <2)
            {
                MessageBox.Show("Ime mora imati vise od jednog karaktera");
                TextBoxIme.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxPrezime.Text))
            {
                MessageBox.Show("Niste uneli prezime");
                TextBoxPrezime.Focus();
                return false;
            }
            if (TextBoxPrezime.Text.Trim().Length < 2)
            {
                MessageBox.Show("Prezime mora imati vise od jednog karaktera");
                TextBoxPrezime.Focus();
                return false;
            }
            return true;
        }

        private void Resetuj()
        {
            TextBoxId.Clear();
            TextBoxIme.Clear();
            TextBoxPrezime.Clear();
            ListBox1.SelectedIndex = -1;
        }

        private void PrikaziOsobe()
        {
            ListBox1.Items.Clear();

            foreach (Osoba os in listaOsoba)
            {
                ListBox1.Items.Add(os);
            }
        }

        private void Sacuvaj()
        {
            if (listaOsoba.Count >0)
            {
                string jsonString = JsonConvert.SerializeObject(listaOsoba);
                File.WriteAllText(putanja, jsonString);
            }
            else
            {
                File.Delete(putanja);
            }

            MessageBox.Show("Promene sacuvane");
        }

        private void Citaj()
        {
            if (File.Exists(putanja))
            {
                string jsonString = File.ReadAllText(putanja);
                listaOsoba = JsonConvert.DeserializeObject<List<Osoba>>(jsonString);
            }
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Citaj();
            if (listaOsoba.Count >0)
            {
                PrikaziOsobe();
                id = listaOsoba[listaOsoba.Count - 1].OsobaId;
            }
        }

        private void ButtonNovi_Click(object sender, RoutedEventArgs e)
        {
            Resetuj();
            TextBoxIme.Focus();
        }

        private void ButtonUbaci_Click(object sender, RoutedEventArgs e)
        {
            if (Validacija())
            {
                Osoba os = new Osoba {
                    OsobaId = ++id,
                    Ime = TextBoxIme.Text,
                    Prezime = TextBoxPrezime.Text
                };

                listaOsoba.Add(os);
                PrikaziOsobe();
                Sacuvaj();
                ListBox1.SelectedIndex = listaOsoba.Count - 1;

            }
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox1.SelectedIndex >-1)
            {
                Osoba selektovanaOsoba = ListBox1.SelectedItem as Osoba;
                TextBoxId.Text = selektovanaOsoba.OsobaId.ToString();
                TextBoxIme.Text = selektovanaOsoba.Ime;
                TextBoxPrezime.Text = selektovanaOsoba.Prezime;
            }
        }

        private void ButtonPromeni_Click(object sender, RoutedEventArgs e)
        {
            int indeks = ListBox1.SelectedIndex;

            if (indeks > -1)
            {
                if (Validacija())
                {
                    Osoba os = listaOsoba[indeks];
                    os.Ime = TextBoxIme.Text;
                    os.Prezime = TextBoxPrezime.Text;

                    PrikaziOsobe();
                    ListBox1.SelectedIndex = indeks;
                    Sacuvaj();
                }
            }
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            int indeks = ListBox1.SelectedIndex;

            if (indeks > -1)
            {
                Osoba os = listaOsoba[indeks];
                MessageBoxResult rez = MessageBox.Show($"Brisanje osobe: {os.ToString()}", "Brisanje", MessageBoxButton.YesNo);

                if (rez == MessageBoxResult.Yes)
                {
                    listaOsoba.RemoveAt(indeks);
                    PrikaziOsobe();
                    Sacuvaj();
                    Resetuj();
                }

                
            }
        }
    }
}
