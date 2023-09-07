using BingoRandomizer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BingoRandomizer.DataClasses
{
    class GoalButton : Button
    {
        public ButtonState State { get; private set; }
        public RightClickState RightClickState { get; private set; } = RightClickState.Nothing;

        public GoalButton(int width, int height, ButtonState state = ButtonState.Unchecked)
        {
            State = state;
            Width = width;
            Height = height;
            MouseRightButtonUp += HandleRightClick;
            Click += HandleClick;
            HandleBakground();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#222831");
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
        }

        private void HandleClick(object sender, RoutedEventArgs e)
        {
            switch (State)
            {
                case ButtonState.Checked:
                    State = ButtonState.Unchecked;
                    Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#222831");
                    break;
                case ButtonState.Unchecked:
                    State = ButtonState.Checked;
                    Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#00ADB5");
                    break;
            }
        }

        private void HandleRightClick(object sender, MouseButtonEventArgs e)
        {
            switch (RightClickState)
            {
                case RightClickState.Star:
                    RightClickState = RightClickState.Nothing;
                    break;
                case RightClickState.Nothing:
                    RightClickState = RightClickState.Star;
                    break;
            }
        }

        private void HandleBakground()
        {
            switch (State)
            {
                case ButtonState.Checked:
                    Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#00ADB5");
                    break;
                case ButtonState.Unchecked:
                    Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#222831");
                    break;
            }
        }

        
    }
}
