using BingoRandomizer.DataClasses;
using BingoRandomizer.Enums;
using BingoRandomizer.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace BingoRandomizer
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush buttonUnchecked = (SolidColorBrush) new BrushConverter().ConvertFrom("#222831");
        SolidColorBrush buttonChecked = (SolidColorBrush)new BrushConverter().ConvertFrom("#00ADB5");
        SolidColorBrush FontColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
        SolidColorBrush BorderColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#393E46");
        Random ran = new Random();
        List<Goal> listWithSelectdGoals = new List<Goal>();
        DispatcherTimer rng = new DispatcherTimer();
        private readonly GoalButton[,] _fields;

        int columnCount, rowCount, seperator, buttonDim, refreshRate, RNGFactor ;
        

        public MainWindow()
        {
            buttonDim = 100;
            seperator = 0;
            columnCount = 5;
            rowCount = 5;
            refreshRate = 15;
            RNGFactor = 20;

            _fields = new GoalButton[columnCount, rowCount];

            InitializeComponent();
            
        }

        void StartNewGame()
        {
            InitialGoals();
            StartRNGTimer();
        }

        private void StartRNGTimer()
        {
            rng.Tick += new EventHandler(rng_Tick);
            rng.Interval = TimeSpan.FromSeconds(refreshRate);
            rng.Start();
        }

        private void rng_Tick(object? sender, EventArgs e)
        {
            var ranNum = ran.Next(101);
            if (ranNum < RNGFactor)
            {
                RandomizeList();
            }
        }

        private void InitializeButtons()
        {
            canvas.Children.Clear();
            int indexer = 0;
            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    var goalState = Convert.ToInt32(listWithSelectdGoals[indexer].Completet);
                    var stateOfGoal = (ButtonState)goalState;
                    var temp = listWithSelectdGoals;
                    _fields[i, j] = new GoalButton(buttonDim, buttonDim, stateOfGoal);

                    _fields[i, j].Click += HandleButtonClick;
                    _fields[i, j].MouseRightButtonUp += HandleMarking;

                    TextBlock buttonContent = new TextBlock();
                    buttonContent.Background = Brushes.Transparent; 
                    buttonContent.TextAlignment = TextAlignment.Center;

                    buttonContent.TextWrapping = TextWrapping.Wrap;
                    buttonContent.Text = listWithSelectdGoals[indexer].Description.ToString();

                    _fields[i, j].Content = buttonContent;
                    _fields[i, j].Foreground = FontColor;
                    _fields[i, j].BorderThickness = new Thickness(2, 2, 2, 2);
                    _fields[i, j].BorderBrush = BorderColor;

                    Canvas.SetLeft(_fields[i, j], i * (buttonDim + seperator));
                    Canvas.SetTop(_fields[i, j], j * (buttonDim + seperator));

                    canvas.Children.Add(_fields[i, j]);

                    indexer++;
                }
            }
        }

        private void HandleMarking(object sender, MouseButtonEventArgs e)
        {
            GoalButton senderButton = (GoalButton)sender;
            string buttonText = ((TextBlock)senderButton.Content).Text;
            var clickedGoal = listWithSelectdGoals.Where<Goal>(x => x.Description.ToString() == buttonText).FirstOrDefault();

            if (clickedGoal.Marked == true)
            {
                clickedGoal.Marked = false;
                clickedGoal.Description = clickedGoal.Description.Split('\n')[0];
                ((TextBlock)senderButton.Content).Text = clickedGoal.Description;
            }
            else
            {
                clickedGoal.Marked = true;
                clickedGoal.Description = clickedGoal.Description + "\n ★";
                ((TextBlock)senderButton.Content).Text = clickedGoal.Description;
            }
        }

        public void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            GoalButton senderButton = (GoalButton)sender;
            string buttonText = ((TextBlock)senderButton.Content).Text;
            var clickedGoal = listWithSelectdGoals.Where<Goal>(x => x.Description.ToString() == buttonText).FirstOrDefault();

            if (clickedGoal.Completet == true) 
            {
                clickedGoal.Completet = false;
            }
            else
            {
                clickedGoal.Completet = true;
            }
        }

        void InitialGoals()
        {
            var goalsFile = File.ReadAllLines("./Resources/ListOFGoals.txt");
            var goalList = new List<string>(goalsFile);
            FillListWithRandomGoals(goalList);
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Visibility = Visibility.Hidden;
            StartNewGame();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            SettingsDialog newDialog = new SettingsDialog( refreshRate,RNGFactor );
            if (newDialog.ShowDialog() == true)
            {
                refreshRate = Convert.ToInt32(newDialog.Rate);
                RNGFactor = Convert.ToInt32(newDialog.Factor);

                rng.Interval = TimeSpan.FromSeconds(refreshRate);
                rng.Start();

                if (newDialog.Reset)
                {
                    listWithSelectdGoals.Clear();
                    rng.Stop();
                    StartButton.Visibility = Visibility.Visible;
                }
            }

        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void FillListWithRandomGoals(List<string> listOfGoals)
        {
            var tempListWithGoals = listOfGoals;
            for (int i = 1; i <= 25; i++)
            {
                var randomIndex = ran.Next(tempListWithGoals.Count - 1);
                listWithSelectdGoals.Add(new Goal { Description = tempListWithGoals[randomIndex], Completet = false, Marked = false });
                tempListWithGoals.RemoveAt(randomIndex);
            }

            listWithSelectdGoals = ValidateGoals(listWithSelectdGoals);
            InitializeButtons();
        }

        List<Goal> ValidateGoals(List<Goal> currentList)
        {
            for(int i = 0; i < 25; i++)
            {
                if (currentList[i].Description.Contains("{"))
                {
                    var tempStringArray = currentList[i].Description.Split(new char[] { '{','}','-' }, StringSplitOptions.None);
                    var goalAmount = ran.Next(Convert.ToInt32(tempStringArray[1]), Convert.ToInt32(tempStringArray[2]));
                    var newGoalDescription = $"{tempStringArray[0]} {goalAmount} {tempStringArray[3]}";
                    currentList[i].Description = newGoalDescription;
                }
            }
            return currentList;
        }

        void RandomizeList()
        {
            int n = listWithSelectdGoals.Count;
            while (n > 1)
            {
                n--;
                int k = ran.Next(n + 1);
                Goal value = listWithSelectdGoals[k];
                listWithSelectdGoals[k] = listWithSelectdGoals[n];
                listWithSelectdGoals[n] = value;
            }
            InitializeButtons();
        }


        
    }
}
