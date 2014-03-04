namespace Billapong.GameConsole.Animation
{
    using System.Collections.Generic;
    using System.Windows.Media.Animation;
    using Models;

    /// <summary>
    /// Represents a single animation of the game ball
    /// </summary>
    public class BallAnimationTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BallAnimationTask"/> class.
        /// </summary>
        public BallAnimationTask()
        {
            this.Steps = new List<PointAnimation>();
        }

        /// <summary>
        /// Gets or sets the window.
        /// </summary>
        /// <value>
        /// The window.
        /// </value>
        public Window Window { get; set; }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        public List<PointAnimation> Steps { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this animation is the last one.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the animation is the last one; otherwise, <c>false</c>.
        /// </value>
        public bool IsLastAnimation { get; set; }
    }
}
