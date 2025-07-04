﻿// <auto-generated />
using System;
using Doerly.Module.Communication.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Communication.DataAccess.Migrations
{
    [DbContext(typeof(CommunicationDbContext))]
    [Migration("20250607173538_Communication_Initial")]
    partial class Communication_Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("communication")
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Doerly.Module.Communication.DataAccess.Entities.ConversationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ConversationName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("conversation_name");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<int>("InitiatorId")
                        .HasColumnType("integer")
                        .HasColumnName("initiator_id");

                    b.Property<int?>("LastMessageId")
                        .HasColumnType("integer")
                        .HasColumnName("last_message_id");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_date");

                    b.Property<int>("RecipientId")
                        .HasColumnType("integer")
                        .HasColumnName("recipient_id");

                    b.HasKey("Id")
                        .HasName("pk_conversation");

                    b.HasIndex("InitiatorId", "RecipientId")
                        .IsUnique()
                        .HasDatabaseName("ix_conversation_initiator_id_recipient_id");

                    b.ToTable("conversation", "communication");
                });

            modelBuilder.Entity("Doerly.Module.Communication.DataAccess.Entities.MessageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ConversationId")
                        .HasColumnType("integer")
                        .HasColumnName("conversation_id");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_date");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message_content");

                    b.Property<byte>("MessageType")
                        .HasColumnType("smallint")
                        .HasColumnName("message_type");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer")
                        .HasColumnName("sender_id");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("sent_at");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_message");

                    b.HasIndex("ConversationId")
                        .HasDatabaseName("ix_message_conversation_id");

                    b.ToTable("message", "communication");
                });

            modelBuilder.Entity("Doerly.Module.Communication.DataAccess.Entities.MessageEntity", b =>
                {
                    b.HasOne("Doerly.Module.Communication.DataAccess.Entities.ConversationEntity", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_message_conversation_conversation_id");

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("Doerly.Module.Communication.DataAccess.Entities.ConversationEntity", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
