namespace Billapong.GameConsole.Animation
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    public class StoryboardCompletedCommand
    {
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof (object), typeof (StoryboardCompletedCommand), new PropertyMetadata(default(object)));

        public static void SetCommandParameter(DependencyObject element, object value)
        {
            element.SetValue(CommandParameterProperty, value);
        }

        public static object GetCommandParameter(DependencyObject element)
        {
            return element.GetValue(CommandParameterProperty);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(StoryboardCompletedCommand), new PropertyMetadata(OnCommandChanged));

        public static void SetCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject element)
        {
            return (ICommand) element.GetValue(CommandProperty);
        }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timeline = d as Timeline;
            if (timeline != null && !timeline.IsFrozen)
            {
                timeline.Completed += delegate
                {
                    var command = GetCommand(d);
                    var param = GetCommandParameter(d);

                    if (command != null && command.CanExecute(param))
                        GetCommand(d).Execute(GetCommandParameter(d));
                };
            }
        }
    }
}
