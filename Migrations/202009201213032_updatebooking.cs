namespace PLGDPBookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "startdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bookings", "enddate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bookings", "purpose", c => c.String());
            DropColumn("dbo.Bookings", "bookingdate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "bookingdate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Bookings", "purpose");
            DropColumn("dbo.Bookings", "enddate");
            DropColumn("dbo.Bookings", "startdate");
        }
    }
}
