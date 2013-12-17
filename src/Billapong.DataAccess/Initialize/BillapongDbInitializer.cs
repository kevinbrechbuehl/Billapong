namespace Billapong.DataAccess.Initialize
{
    using System.Data.Entity;

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
            base.Seed(context);
        }
    }
}
