using Microsoft.Win32;
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
using System.IO;
using System.ComponentModel;

namespace PoznamkovyBlok
{
    public partial class MainWindow : Window
    {
        //Privátní proměnná, "má" v sobě jméno souboru
        private string fileName = "";

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Zobrazí první okno s dotazem a prozatím skryje druhé
            panStart.Visibility = Visibility.Visible;
            grMain.Visibility = Visibility.Hidden;
        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //Schová první okno
            panStart.Visibility = Visibility.Hidden;

            //Zobrazí hlavní okno
            grMain.Visibility = Visibility.Visible;

            if (rbNight.IsChecked == true)
            {
                //Nastaví noční mód (změna barvy objektů), každý je zvlášť, protože jsem nevěděl jak to zkrátit :D
                menu.Background = Brushes.Black;
                NoteTextBox.Background = Brushes.Black;
                NoteTextBox.Foreground = Brushes.White;
                menuItem.Background = Brushes.Black;
                menuItem2.Background = Brushes.Black;
                menuItem.Foreground = Brushes.White;
                menuItem2.Foreground = Brushes.White;
                btnOpen.Background = Brushes.Black;
                btnSave.Background = Brushes.Black;
                btnOpen.Foreground = Brushes.White;
                btnSave.Foreground = Brushes.White;
                btnInfo.Foreground = Brushes.White;
                btnInfo.Background = Brushes.Black;
                foreach (var button in spName.Children.OfType<Button>())
                {
                    button.Background = Brushes.Black;
                    button.Foreground = Brushes.White;
                }
            }
        }




        //Po kliknutí na tlačítko si pomocí funkce (SaveFileDialog) můžeme vybrat soubor, do kterého se uloží obsah TextBoxu
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Textové dokumenty (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, NoteTextBox.Text);
            }
        }
        //Po kliknutí na tlačítko si pomocí podobné funkce (OpenFileDialog) vybereme soubor, který chceme otevřít a obsah se zobrazí opět v TextBoxu
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Textové soubory (*.txt)|*.txt|Všechny soubory (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                fileName = openFileDialog.FileName;
                string fileContent = string.Empty;
        //Zachycení výjimky, kód co může vyvolat výjimku
                try
                {
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    NoteTextBox.Text = fileContent;
                }
        //Pokud se vyvolá výjimka v první části (try), provede se tento kód místo ukončení programu chybou
                catch (Exception ex)
                {
                    MessageBox.Show("Nastala chyba při čtení souboru: " + ex.Message);
                }
            }
        }
        //Zkopíruje označený text
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NoteTextBox.SelectedText))
                {
                    Clipboard.SetText(NoteTextBox.SelectedText);
                }
            }
            //Když se naskytne chyba spustí se tato část
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("Nepodařilo se otevřít schránku: " + ex.Message);
            }
        }
        //Funguje jako klasické "enter", nový řádek - (Tento příkaz jsem nevěděl jak napsat, takže jsem ho musel zkopčit xd)
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    textBox.Text += Environment.NewLine;
                    textBox.CaretIndex = textBox.Text.Length;
                }
                e.Handled = true;
            }
        }

        //Po kliknutí se spustí metoda pro vymazání obsahu TextBoxu
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NoteTextBox.Clear();
        }
        //Po kliknutí se zobrazí MessageBox s informacemi
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Jakub Cífka, Adam Poláček, Václav Michálek, Karel Lang\nEduchem\n{DateTime.Now.Year}");
        }
        //Po kliknutí se písmena v TextBoxu zmenší
        private void LowerButton_Click(object sender, RoutedEventArgs e)
        {
            NoteTextBox.Text = NoteTextBox.Text.ToLower();
        }
        //Po kliknutí se písmena v TextBoxu zvětší
        private void UpperButton_Click(object sender, RoutedEventArgs e)
        {
            NoteTextBox.Text = NoteTextBox.Text.ToUpper();
        }
        //Po kliknutí se v MessageBoxu zobrazí počet znaků v TextBoxu
        private void CountButton_Click(object sender, RoutedEventArgs e)
        {
            int pocetZnaku = NoteTextBox.Text.Length;
            MessageBox.Show("Počet znaků: " + pocetZnaku.ToString());
        }
    }
}

