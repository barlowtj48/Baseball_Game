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
using System.Windows.Markup;

namespace Baseball
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Baseball_Game game;
        private Dictionary<string, int> stats;
        private Dictionary<string, Ellipse[]> stat_elements = new Dictionary<string, Ellipse[]>();
        private Ellipse[] bases;
        private StackPanel[] scoreboards;
        private string[] names;
        private Dictionary<string, Actions> actions = new Dictionary<string, Actions>();


        private LinkedList<Actions> GameRecord = new LinkedList<Actions>();
        

        public MainWindow()
        {
            InitializeComponent();
            All_Buttons_StackPanel.IsEnabled = false;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartWindow s = new StartWindow();
            s.ShowDialog();

            names = s.player_names;
            if (names == null)
                System.Environment.Exit(0);
            if(game == null)
            {
                InitVars();
            }
            New_Game();
            var window = Window.GetWindow(this);
            window.KeyDown += KeyPressed;
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            Actions action;
            switch (e.Key)
            {
                case Key.D1:
                    action = Actions.Single;
                    break;
                case Key.D2:
                    action = Actions.Double;
                    break;
                case Key.D3:
                    action = Actions.Triple;
                    break;
                case Key.Q:
                    action = Actions.Strike;
                    break;
                case Key.W:
                    action = Actions.Foul;
                    break;
                case Key.E:
                    action = Actions.Ball;
                    break;
                case Key.A:
                    action = Actions.Stolen_Base;
                    break;
                case Key.S:
                    action = Actions.Sacrifice;
                    break;
                case Key.D:
                    action = Actions.Double_Play;
                    break;
                case Key.Z:
                    action = Actions.Out;
                    break;
                case Key.X:
                    action = Actions.Home_Run;
                    break;
                case Key.C:
                    action = Actions.Grand_Slam;
                    break;
                default:
                    return;
            }
            stats = game.Do(action);
            Register_Action(action);
            Update_Stats();
        }

        private void InitVars()
        {
            Ellipse[] strikes = { strike_0_ellipse, strike_1_ellipse };
            Ellipse[] balls = { ball_0_ellipse, ball_1_ellipse, ball_2_ellipse };
            Ellipse[] outs = { out_0_ellipse, out_1_ellipse };
            stat_elements.Add("strikes", strikes);
            stat_elements.Add("balls", balls);
            stat_elements.Add("outs", outs);

            bases = new Ellipse[] { first_base_ellipse, second_base_ellipse, third_base_ellipse };
            scoreboards = new StackPanel[] { p1_scoreboard, p2_scoreboard, p3_scoreboard, p4_scoreboard };

            actions.Add("Single", Actions.Single);
            actions.Add("Double", Actions.Double);
            actions.Add("Triple", Actions.Triple);
            actions.Add("Strike", Actions.Strike);
            actions.Add("Foul", Actions.Foul);
            actions.Add("Ball", Actions.Ball);
            actions.Add("Stolen_Base", Actions.Stolen_Base);
            actions.Add("Sacrifice", Actions.Sacrifice);
            actions.Add("Double_Play", Actions.Double_Play);
            actions.Add("Out", Actions.Out);
            actions.Add("Home_Run", Actions.Home_Run);
            actions.Add("Grand_Slam", Actions.Grand_Slam);
        }

        private void New_Game()
        {
            game = new Baseball_Game(names);
            All_Buttons_StackPanel.IsEnabled = true;
            Undo_button.IsEnabled = false;

            int count = 0;
            foreach (var scoreboard in scoreboards)
            {
                if (count >= names.Length)
                {
                    scoreboard.Visibility = Visibility.Hidden;
                    continue;
                }

                foreach (Label child in scoreboard.Children)
                {
                    if (child.Name.Contains("i"))
                        child.Content = "";
                    else if (child.Name.Contains("name"))
                        child.Content = names[count];
                }
                count++;
            }
            current_turn_label.Content = $"{names.First()}'s Turn";
            foreach (var element in stat_elements)
            {
                for (int i = 0; i < element.Value.Length; i++)
                {
                    element.Value[i].Fill = Brushes.White;
                }
            }
            for (int i = 0; i < bases.Length; i++)
            {
                bases[i].Fill = Brushes.White;
            }
        }

        private void Update_Stats()
        {
            foreach(var element in stat_elements)
            {
                for (int i = 0; i < element.Value.Length; i++)
                {
                    if(i+1 <= stats[element.Key])
                    {
                        element.Value[i].Fill = Brushes.Red;
                    }
                    else
                    {
                        element.Value[i].Fill = Brushes.White;
                    }
                }
            }

            string field = stats["field"].ToString();

            for (int i = 0; i < bases.Length; i++)
            {
                if(field[i+1] == '1')
                {
                    bases[i].Fill = Brushes.Red;
                }
                else
                {
                    bases[i].Fill = Brushes.White;
                }
            }
            if (stats["inning"] == 9)
            {
                current_turn_label.Content = "Game Over";
                inning_label.Content = $"{game.Get_Winner()} wins";
                All_Buttons_StackPanel.IsEnabled = false;
                return;
            }
            current_turn_label.Content = $"{names[stats["current_player"]]}'s Turn";
            inning_label.Content = $"Inning {stats["inning"]+1}";
            foreach (Label label in scoreboards[stats["current_player"]].Children)
            {
                if (label.Name.Contains($"i{stats["inning"] + 1}"))
                {
                    label.Content = stats["inning_score"];
                    continue;
                }
                if (label.Name.Contains("total"))
                {
                    label.Content = stats["score"];
                }
            }

            if(GameRecord.Count > 0)
            {
                Undo_button.IsEnabled = true;
            }
            else
            {
                Undo_button.IsEnabled = false;
            }
        }

        private void Register_Action(Actions action)
        {
            GameRecord.AddLast(action);
        }

        private bool Undo()
        {
            GameRecord.RemoveLast();
            game = new Baseball_Game(names);
            LinkedListNode<Actions> node = GameRecord.First;
            if(node == null)
            {
                New_Game();
                return false;
            }
            while (node != null)
            {
                stats = game.Do(node.Value);
                node = node.Next;
            }
            return true;
        }

        private void Undo_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Undo())
            {
                Update_Stats();
            }
        }

        private void Action_Button_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            string contents = Convert.ToString(clicked.Content).Replace(" ", "_");
            stats = game.Do(actions[contents]);
            Register_Action(actions[contents]);
            Update_Stats();
        }
    }
}
