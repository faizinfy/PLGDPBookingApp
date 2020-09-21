namespace PLGDPBookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class booking3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "sector", c => c.String());
            AddColumn("dbo.Bookings", "noofparticipant", c => c.Int(nullable: false));
            AddColumn("dbo.Bookings", "isinternet", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "ispasystem", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "islcdprojector", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "islcdprojector");
            DropColumn("dbo.Bookings", "ispasystem");
            DropColumn("dbo.Bookings", "isinternet");
            DropColumn("dbo.Bookings", "noofparticipant");
            DropColumn("dbo.Bookings", "sector");
        }
    }
}
