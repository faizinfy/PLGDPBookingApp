namespace PLGDPBookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookingno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "bookingNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "bookingNo");
        }
    }
}
