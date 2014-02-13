namespace Billapong.GameConsole.Animation
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;
    using Configuration;
    using Game;

    public static class AnimationHelpers
    {
        /// <summary>
        /// Gets the point animation.
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="targetPosition">The target position.</param>
        /// <returns>The point animation</returns>
        public static PointAnimation GetPointAnimation(Point currentPosition, Point targetPosition)
        {
            var actualDistance = currentPosition.DistanceTo(targetPosition);

            var animation = new PointAnimation
            {
                By = currentPosition,
                To = targetPosition,
                Duration = TimeSpan.FromMilliseconds(GameConfiguration.BaseAnimationDuration / GameConfiguration.MaxAnimationDistance * actualDistance)
            };
            return animation;
        }

    }
}
