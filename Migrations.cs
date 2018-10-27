using System;
using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Panmedia.EventManager.Models;
using System.Runtime.InteropServices;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.Drivers;

namespace Panmedia.EventManager
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            // Creating table ParticipantRecord
            SchemaBuilder.CreateTable("ParticipantRecord", table => table
                .Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
                .Column("UserName", DbType.String)
                .Column("ParticipantName", DbType.String)
                .Column("ParticipantComment", DbType.String)                
                .Column("MemberEventPartRecord_Id", DbType.Int32)
                .Column("IsOnWaitingList", DbType.Boolean)
            );
            // Creating table MemberEventPartRecord            
            SchemaBuilder.CreateTable("MemberEventPartRecord", table => table
                .ContentPartRecord()                
                .Column("EventAddress", DbType.String)
                .Column("EventDescription", DbType.String)                
                .Column("GoogleStaticMapLink", DbType.String)
                .Column<DateTime>("EventStartUtc")
                .Column<DateTime>("EventStopUtc")
                .Column("MaxAttendees", DbType.Int32)
                .Column("Shown", DbType.Boolean)
                .Column("WaitingList", DbType.Boolean)
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(MemberEventPart).Name, cfg => cfg.Attachable());

            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition(
                "MemberEvent", cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("MemberEventPart")
                    .WithPart("BodyPart")                    
                    .WithPart("IdentityPart")
                    .WithPart("ContainablePart")                    
                    .Creatable());                    
            return 2;
        }

        
        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition(
                "MemberEvent", cfg => cfg
                    .WithPart("TitlePart"));
            return 3;
        }

        public int UpdateFrom3()
        {
            ContentDefinitionManager.AlterTypeDefinition(
                "MemberEvent", cfg => cfg
                    .Listable());
            return 4;
        }

        public int UpdateFrom4()
        {
            ContentDefinitionManager.AlterTypeDefinition(
                "MemberEvent", cfg => cfg
                    .WithPart("LocalizationPart"));
            return 5;
        }
    }
}