namespace Billapong.DataAccess.Initialize
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using Model.Map;

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
                IsPlayable = true,
                Windows = new List<Window>
                {
                    new Window
                    {
                        X = 0,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 3 },
                            new Hole { X = 6, Y = 7 },
                            new Hole { X = 2, Y = 6 }
                        }
                    }
                }
            };

            var advancedMap = new Map
            {
                Name = "Advanced Map",
                IsPlayable = true,
                Windows = new List<Window>
                {
                    new Window
                    {
                        X = 0,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 8 },
                            new Hole { X = 3, Y = 2 },
                            new Hole { X = 7, Y = 5 }
                        }
                    },
                    new Window
                    {
                        X = 0,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 4 },
                            new Hole { X = 5, Y = 2 },
                            new Hole { X = 2, Y = 6 },
                            new Hole { X = 9, Y = 3 }
                        }
                    },
                    new Window
                    {
                        X = 0,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 8 }
                        }
                    },
                    new Window
                    {
                        X = 1,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 3 },
                            new Hole { X = 6, Y = 7 },
                            new Hole { X = 0, Y = 0 },
                            new Hole { X = 1, Y = 3 },
                            new Hole { X = 4, Y = 7 }
                        }
                    }
                }
            };

            var fullMap = new Map
            {
                Name = "Full Map",
                IsPlayable = true,
                Windows = new List<Window>
                {
                    new Window
                    {
                        X = 0,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 8 },
                            new Hole { X = 3, Y = 2 },
                            new Hole { X = 7, Y = 5 }
                        }
                    },
                    new Window
                    {
                        X = 1,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 4 },
                            new Hole { X = 5, Y = 2 },
                            new Hole { X = 2, Y = 6 },
                            new Hole { X = 9, Y = 3 }
                        }
                    },
                    new Window
                    {
                        X = 2,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 8 }
                        }
                    },
                    new Window
                    {
                        X = 3,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 3 },
                            new Hole { X = 6, Y = 7 },
                            new Hole { X = 0, Y = 0 },
                            new Hole { X = 1, Y = 3 },
                            new Hole { X = 4, Y = 7 }
                        }
                    },
                    new Window
                    {
                        X = 0,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 8 },
                            new Hole { X = 3, Y = 2 },
                            new Hole { X = 7, Y = 5 }
                        }
                    },
                    new Window
                    {
                        X = 1,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 4 },
                            new Hole { X = 5, Y = 2 },
                            new Hole { X = 2, Y = 6 },
                            new Hole { X = 9, Y = 3 }
                        }
                    },
                    new Window
                    {
                        X = 2,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 8 }
                        }
                    },
                    new Window
                    {
                        X = 3,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 3 },
                            new Hole { X = 6, Y = 7 },
                            new Hole { X = 0, Y = 0 },
                            new Hole { X = 1, Y = 3 },
                            new Hole { X = 4, Y = 7 }
                        }
                    },
                    new Window
                    {
                        X = 0,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 8 },
                            new Hole { X = 3, Y = 2 },
                            new Hole { X = 7, Y = 5 }
                        }
                    },
                    new Window
                    {
                        X = 1,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 4 },
                            new Hole { X = 5, Y = 2 },
                            new Hole { X = 2, Y = 6 },
                            new Hole { X = 9, Y = 3 }
                        }
                    },
                    new Window
                    {
                        X = 2,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 8 }
                        }
                    },
                    new Window
                    {
                        X = 3,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 3 },
                            new Hole { X = 6, Y = 7 },
                            new Hole { X = 0, Y = 0 },
                            new Hole { X = 1, Y = 3 },
                            new Hole { X = 4, Y = 7 }
                        }
                    }
                }
            };

            var doughnutMap = new Map
            {
                Name = "The Doughnut Map",
                IsPlayable = true,
                Windows = new List<Window>
                {
                    new Window
                    {
                        X = 0,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 8 },
                            new Hole { X = 3, Y = 2 },
                            new Hole { X = 7, Y = 5 }
                        }
                    },
                    new Window
                    {
                        X = 1,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 4 },
                            new Hole { X = 5, Y = 2 },
                            new Hole { X = 2, Y = 6 },
                            new Hole { X = 9, Y = 3 }
                        }
                    },
                    new Window
                    {
                        X = 2,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 8 }
                        }
                    },
                    new Window
                    {
                        X = 3,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 3 },
                            new Hole { X = 6, Y = 7 },
                            new Hole { X = 0, Y = 0 },
                            new Hole { X = 1, Y = 3 },
                            new Hole { X = 4, Y = 7 }
                        }
                    },
                    new Window
                    {
                        X = 0,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 1 },
                            new Hole { X = 3, Y = 2 },
                            new Hole { X = 15, Y = 15 }
                        }
                    },
                    new Window
                    {
                        X = 3,
                        Y = 1,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 3 },
                            new Hole { X = 6, Y = 7 },
                            new Hole { X = 0, Y = 0 },
                            new Hole { X = 1, Y = 3 },
                            new Hole { X = 4, Y = 7 }
                        }
                    },
                    new Window
                    {
                        X = 0,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 8 },
                            new Hole { X = 3, Y = 5 },
                            new Hole { X = 7, Y = 2 }
                        }
                    },
                    new Window
                    {
                        X = 1,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 1, Y = 4 },
                            new Hole { X = 5, Y = 2 },
                            new Hole { X = 2, Y = 1 },
                            new Hole { X = 9, Y = 3 }
                        }
                    },
                    new Window
                    {
                        X = 2,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 8 }
                        }
                    },
                    new Window
                    {
                        X = 3,
                        Y = 2,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 3 },
                            new Hole { X = 6, Y = 7 },
                            new Hole { X = 0, Y = 0 },
                            new Hole { X = 1, Y = 3 },
                            new Hole { X = 4, Y = 7 },
                            new Hole { X = 8, Y = 9 },
                            new Hole { X = 12, Y = 12 }
                        }
                    }
                }
            };

            var unplayableMap = new Map
            {
                Name = "Unplayable Map",
                IsPlayable = false,
                Windows = new List<Window>
                {
                    new Window
                    {
                        X = 0,
                        Y = 0,
                        Holes = new List<Hole>
                        {
                            new Hole { X = 3, Y = 5 },
                            new Hole { X = 10, Y = 7 },
                            new Hole { X = 1, Y = 14 }
                        }
                    }
                }
            };

            context.Maps.Add(simpleMap);
            context.Maps.Add(advancedMap);
            context.Maps.Add(fullMap);
            context.Maps.Add(doughnutMap);
            context.Maps.Add(unplayableMap);
            context.SaveChanges();
        }
    }
}
