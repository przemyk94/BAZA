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
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace ProjektPOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Tabela(Id integer primary key autoincrement, IMIE integer, NAZWISKO integer" + "IMIE varchar(100), NAZWISKO varchar(100)
        
        //SQLiteDataReader czytnik; 

        //private static string _zapytanieSQL = "create table if not exists CREATE TABLE [Tabela] ([Id] INTEGER NOT NULL, [IMIE] TEXT NOT NULL, [NAZWISKO] TEXT NOT NULL, CONSTRAINT[PK_Tabela] PRIMARY KEY([Id]))";
        //SQLiteCommand Komenda = new SQLiteCommand(_zapytanieSQL, _polaczenie);
        #region Zmienne
        SQLiteConnection _polaczenie = new SQLiteConnection("Data Source=baza.db");
        SQLiteCommand komenda;
        SQLiteDataReader czytnik;
        string zapytanieSQL = "";
        List<Postac> listaPostaci = new List<Postac>();
        Postac wybranaPostac = null;


        //string zapytanieSQL = string.Format("");
        //zapytanieSQL = string.Format( "UPDATE Tabela SET IMIE = '{0}', NAZWISKO= '{2}' WHERE Id = {1}", imieBox.Text, wybranaPostac.Id, nazwiskoBox.Text);
        //komenda = new SQLiteCommand(_zapytanieSQL, _polaczenie);
        //komenda.ExecuteNonQuery();
        //            MessageBox.Show("Postać zaktualizowana.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
        #endregion Zmienne

        #region Konstruktory
        public MainWindow()
        {
            InicjalizacjaDanych();
            InitializeComponent();
        }

        #endregion Konstruktory

        #region Metody

        #region Ogolne

        void InicjalizacjaDanych()
        {
            
            try
            {
                if (_polaczenie.State == ConnectionState.Closed)
                { _polaczenie.Open(); }
                zapytanieSQL = "select * from Tabela";
                komenda = new SQLiteCommand(zapytanieSQL, _polaczenie);
                czytnik = komenda.ExecuteReader();
                //int licznik = 1;


                //listBox.Items.Add(string.Format( "{0} - {1} - {2}", czytnik["Id"].ToString(), czytnik["IMIE"].ToString(), czytnik["NAZWISKO"].ToString()));
                if (czytnik.HasRows)
                {
                    while (czytnik.Read())
                    {
                        listaPostaci.Add(new Postac(int.Parse(czytnik["Id"].ToString()), czytnik["IMIE"].ToString(), czytnik["NAZWISKO"].ToString()));
                        //listBox.Items.Add(string.Format("{0} - {1} - {2}", licznik++, czytnik["IMIE"].ToString(), czytnik["NAZWISKO"]));
                        listBox.Items.Add(string.Format("{0} - {1} - {2}", int.Parse(czytnik["Id"].ToString()), czytnik["IMIE"].ToString(), czytnik["NAZWISKO"].ToString()));
                    }
                    czytnik.Close();
                }

            }
            catch (Exception ex)
            {
                //string byk = string.Format("Błąd podczas pobierania danych: \n {0}", ex.Message);
                //MessageBox.Show(byk, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _polaczenie.Close();
                if (czytnik != null)
                {
                    czytnik.Dispose();
                    czytnik = null;
                }
            }
        }

        void WyczyscListe()
        {
            listBox.SelectedIndex = -1;
        }

        private void imieBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        #endregion Ogolne
        private void button_Click(object sender, RoutedEventArgs e)
        {
            listaPostaci.Clear();
            listBox.Items.Clear();
            InicjalizacjaDanych();

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IndeksWybranejPostaci = listBox.SelectedIndex;

                if (wybranaPostac != null)
                {
                    if (_polaczenie.State == ConnectionState.Closed)
                    { _polaczenie.Open(); }
                    ////////////////////////////////////////////////////////////////////
                    string _zapytanieSQL = string.Format("");
                    _zapytanieSQL = string.Format( "UPDATE Tabela SET IMIE = '{0}', NAZWISKO= '{2}' WHERE Id = {1}", imieBox.Text, wybranaPostac.Id, nazwiskoBox.Text);
                    komenda = new SQLiteCommand(_zapytanieSQL, _polaczenie);
                    komenda.ExecuteNonQuery();
                    MessageBox.Show("Postać zaktualizowana.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);


                    listaPostaci.Clear();
                    listBox.Items.Clear();
                    InicjalizacjaDanych();
                    listBox.SelectedIndex = IndeksWybranejPostaci;
                }
                else
                    MessageBox.Show("Wybierz postać, którą chcesz zaktualizować.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                string byk = string.Format("Problem : \n {0}", ex.Message);
                MessageBox.Show(byk, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
  

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listBox.SelectedIndex == -1)
                {
                    imieBox.Text = "";
                    nazwiskoBox.Text = "";
                    wybranaPostac = null;

                }
                else
                {

                    string cos = listBox.SelectedItem.ToString();
                    string[] tab = cos.Split('-');
                       
                    
                    Postac p = listaPostaci.FirstOrDefault(x => x.IMIE.Equals(tab[1].Trim()));
                    wybranaPostac = p;
                    if (p != null)
                    {
                        imieBox.Text = p.IMIE;
                        nazwiskoBox.Text = p.NAZWISKO;
                    }
                }
            }
            catch (Exception ex)
            {
                string byk = string.Format("Problem podczas wybierania postaci: \n", ex.Message);
                MessageBox.Show(byk, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imieBox.Text != null || nazwiskoBox.Text != null)
                { 
                    if (_polaczenie.State == ConnectionState.Closed)
                    { _polaczenie.Open(); }
                    ////////////////////////////////////////////////////////////////////
                    string _zapytanieSQL = string.Format("");
                _zapytanieSQL = string.Format("INSERT INTO Tabela (IMIE, NAZWISKO, Id)"+"VALUES('{0}', '{1}', {2})", imieBox.Text, nazwiskoBox.Text, listaPostaci.MaxId()+1);
                    komenda = new SQLiteCommand(_zapytanieSQL, _polaczenie);
                    komenda.ExecuteNonQuery();
                    MessageBox.Show("Postać Dodana.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);


                    listaPostaci.Clear();
                    listBox.Items.Clear();
                    InicjalizacjaDanych();
                    
                }
                else
                    MessageBox.Show("Musisz coś wpisać.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                string byk = string.Format("Problem : \n {0}", ex.Message);
                MessageBox.Show(byk, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                

                if (wybranaPostac != null)
                {
                    if (_polaczenie.State == ConnectionState.Closed)
                    { _polaczenie.Open(); }

                   
                    ////////////////////////////////////////////////////////////////////
                    string _zapytanieSQL = string.Format("");
                    _zapytanieSQL = string.Format("DELETE FROM Tabela WHERE Id = {0}", wybranaPostac.Id);
                    komenda = new SQLiteCommand(_zapytanieSQL, _polaczenie);
                    komenda.ExecuteNonQuery();
                    MessageBox.Show("Postać usunięta.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);


                    listaPostaci.Clear();
                    listBox.Items.Clear();
                    InicjalizacjaDanych();
               
                }
                else
                    MessageBox.Show("Wybierz postać, którą chcesz usunąć.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                string byk = string.Format("Problem : \n {0}", ex.Message);
                MessageBox.Show(byk, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        #endregion Metody

       
    }
}
