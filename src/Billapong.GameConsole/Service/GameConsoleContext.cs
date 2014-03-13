namespace Billapong.GameConsole.Service
{
    using System;
    using System.Diagnostics;
    using System.Timers;
    using Billapong.Core.Client.Helper;
    using Billapong.Core.Client.Tracing;

    /// <summary>
    /// Provides a static context for the gameconsole
    /// </summary>
    public class GameConsoleContext
    {
        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly GameConsoleContext Instance = new GameConsoleContext();

        /// <summary>
        /// The game console service client
        /// </summary>
        private GameConsoleServiceClient gameConsoleServiceClient;

        /// <summary>
        /// The game console callback
        /// </summary>
        private GameConsoleCallback gameConsoleCallback;

        /// <summary>
        /// The current game identifier
        /// </summary>
        private Guid currentGameId;

        /// <summary>
        /// The keep alive timer
        /// </summary>
        private readonly Timer keepAliveTimer;

        /// <summary>
        /// Prevents a default instance of the <see cref="GameConsoleContext"/> class from being created.
        /// </summary>
        private GameConsoleContext()
        {
            this.keepAliveTimer = new Timer();
            this.keepAliveTimer.Interval = 10000;
            this.keepAliveTimer.Elapsed += this.KeepGameAlive;
        }

        /// <summary>
        /// The running game disappeared event handler
        /// </summary>
        public EventHandler RunningGameDisappeared = delegate { };

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The singleton instance.
        /// </value>
        public static GameConsoleContext Current
        {
            get
            {
                return Instance;
            }
        }

        /// <summary>
        /// Gets the game console service client.
        /// </summary>
        /// <value>
        /// The game console service client.
        /// </value>
        public GameConsoleServiceClient GameConsoleServiceClient
        {
            get
            {
                return this.gameConsoleServiceClient ?? (this.gameConsoleServiceClient = new GameConsoleServiceClient(this.GameConsoleCallback));
            }
        }

        /// <summary>
        /// Gets the game console callback.
        /// </summary>
        /// <value>
        /// The game console callback.
        /// </value>
        public GameConsoleCallback GameConsoleCallback
        {
            get
            {
                return this.gameConsoleCallback ?? (this.gameConsoleCallback = new GameConsoleCallback());
            }
        }

        /// <summary>
        /// Starts the keep alive for the given game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        public void StartKeepGameAlive(Guid gameId)
        {
            if (this.keepAliveTimer.Enabled)
            {
                this.keepAliveTimer.Stop();
            }

            this.currentGameId = gameId;
            this.keepAliveTimer.Start();
        }

        /// <summary>
        /// Stops the keep alive for the current game.
        /// </summary>
        public void StopKeepGameAlive()
        {
            this.keepAliveTimer.Stop();
        }


        /// <summary>
        /// Keeps the game alive.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private async void KeepGameAlive(object sender, ElapsedEventArgs e)
        {
            if (!this.currentGameId.Equals(Guid.Empty))
            {
                bool isGameRunning;
                try 
                {  
                    isGameRunning = await this.gameConsoleServiceClient.IsGameRunningAsync(this.currentGameId);
                }
                catch (Exception ex)
                {
                    Tracer.Error(ex.Message, ex);
                    ThreadContext.InvokeOnUiThread(() => this.RunningGameDisappeared(this, null));
                    this.keepAliveTimer.Stop();
                    return;
                }

                if (isGameRunning)
                {
                    Tracer.Debug(string.Format("Sent keepalive for the game with the id {0}. Game is still running.", this.currentGameId));
                }
                else
                {
                    Tracer.Debug(string.Format("Sent keepalive for the game with the id {0}. Game is no longer running.", this.currentGameId));
                    ThreadContext.InvokeOnUiThread(() => this.RunningGameDisappeared(this, null));
                    this.keepAliveTimer.Stop();
                }
            }
        }
    }
}
