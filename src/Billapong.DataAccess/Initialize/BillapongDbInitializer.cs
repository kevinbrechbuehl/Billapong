namespace Billapong.DataAccess.Initialize
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using Model.Editor;

    /// <summary>
    /// Billapong database initializer.
    /// </summary>
    public class BillapongDbInitializer : DropCreateDatabaseIfModelChanges<BillapongDbContext>
    {
        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(BillapongDbContext context)
        {
            var simpleMap = new Map
            {
                Name = "Simple Map",
                Windows = new List<Window>
                {
                    new Window
                    {
                        X = 0,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole {X = 3, Y = 3},
                            new Hole {X = 6, Y = 7},
                            new Hole {X = 2, Y = 6}
                        }
                    }
                }
            };

            var advancedMap = new Map
            {
                Name = "Advanced Map",
                Windows = new List<Window>
                {
                    new Window
                    {
                        X = 0,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole {X = 1, Y = 8},
                            new Hole {X = 3, Y = 2},
                            new Hole {X = 7, Y = 5}
                        }
                    },
                    new Window
                    {
                        X = 0,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole {X = 1, Y = 4},
                            new Hole {X = 5, Y = 2},
                            new Hole {X = 2, Y = 6},
                            new Hole {X = 9, Y = 3}
                        }
                    },
                    new Window
                    {
                        X = 0,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole {X = 3, Y = 8}
                        }
                    },
                    new Window
                    {
                        X = 1,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole {X = 3, Y = 3},
                            new Hole {X = 6, Y = 7},
                            new Hole {X = 0, Y = 0},
                            new Hole {X = 1, Y = 3},
                            new Hole {X = 4, Y = 7}
                        }
                    }
                }
            };

            context.Maps.Add(simpleMap);
            context.Maps.Add(advancedMap);
            context.SaveChanges();
        }
    }
}
