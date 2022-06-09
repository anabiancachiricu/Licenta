namespace MedOffice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false),
                        DepartmentDescription = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        City = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Latitude = c.Single(nullable: false),
                        Longitude = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        Department_DepartmentId = c.Int(),
                        Location_LocationId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DoctorId)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Department_DepartmentId)
                .Index(t => t.Location_LocationId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserPhoto = c.Binary(),
                        Description = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Investigations",
                c => new
                    {
                        InvestigationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Single(nullable: false),
                        Department_DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvestigationId)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentId, cascadeDelete: true)
                .Index(t => t.Department_DepartmentId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.LocationDepartments",
                c => new
                    {
                        Location_LocationId = c.Int(nullable: false),
                        Department_DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Location_LocationId, t.Department_DepartmentId })
                .ForeignKey("dbo.Locations", t => t.Location_LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentId, cascadeDelete: true)
                .Index(t => t.Location_LocationId)
                .Index(t => t.Department_DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Investigations", "Department_DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Doctors", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Doctors", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Doctors", "Department_DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.LocationDepartments", "Department_DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.LocationDepartments", "Location_LocationId", "dbo.Locations");
            DropIndex("dbo.LocationDepartments", new[] { "Department_DepartmentId" });
            DropIndex("dbo.LocationDepartments", new[] { "Location_LocationId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Investigations", new[] { "Department_DepartmentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Doctors", new[] { "User_Id" });
            DropIndex("dbo.Doctors", new[] { "Location_LocationId" });
            DropIndex("dbo.Doctors", new[] { "Department_DepartmentId" });
            DropTable("dbo.LocationDepartments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Investigations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Doctors");
            DropTable("dbo.Locations");
            DropTable("dbo.Departments");
        }
    }
}
