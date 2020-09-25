namespace PLGDPBookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "status", c => c.Int(nullable: false));
            AddColumn("dbo.Bookings", "remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "remarks");
            DropColumn("dbo.Bookings", "status");
        }
    }
}
