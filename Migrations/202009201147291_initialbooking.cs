namespace PLGDPBookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialbooking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        mobileno = c.String(),
                        locationname = c.String(),
                        bookingdate = c.DateTime(nullable: false),
                        createddate = c.DateTime(nullable: false, defaultValueSql: "getdate()"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bookings");
        }
    }
}
