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
using System.Windows.Shapes;

namespace Baseball
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        public int player_count;
        public string[] player_names;


        private void One_player_button_Click(object sender, RoutedEventArgs e)
        {
            player_count = 1;
            show(1);
        }

        private void Two_player_button_Click(object sender, RoutedEventArgs e)
        {
            player_count = 2;
            show(2);
        }

        private void Three_player_button_Click(object sender, RoutedEventArgs e)
        {
            player_count = 3;
            show(3);
        }

        private void Four_player_button_Click(object sender, RoutedEventArgs e)
        {
            player_count = 4;
            show(4);
        }

        private void show(int n)
        {
            TextBox[] elements = { p1_name_textbox, p2_name_textbox, p3_name_textbox, p4_name_textbox };
            for (int i = 0; i < 4; i++)
            {
                elements[i].Visibility = Visibility.Hidden;
            }
            for (int i = 0; i < n; i++)
            {
                elements[i].Visibility = Visibility.Visible;
            }
            start_button.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] elements = { p1_name_textbox, p2_name_textbox, p3_name_textbox, p4_name_textbox };
            player_names = new string[player_count];
            for (int i = 0; i < player_count; i++)
            {
                string text = elements[i].Text;
                if (text == "")
                {
                    text = $"Player {i}";
                }
                player_names[i] = text.TrimEnd();
            }
            StartWindow1.Close();
        }
    }
}
