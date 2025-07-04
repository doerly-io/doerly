﻿// <auto-generated />
using System;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Profile.DataAccess.Migrations
{
    [DbContext(typeof(ProfileDbContext))]
    partial class ProfileDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("profile")
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.Competence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("category_name");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    b.Property<float?>("Rating")
                        .HasPrecision(3, 2)
                        .HasColumnType("real")
                        .HasColumnName("rating");

                    b.HasKey("Id")
                        .HasName("pk_competence");

                    b.HasIndex("Rating")
                        .HasDatabaseName("ix_competence_rating");

                    b.HasIndex("ProfileId", "CategoryId")
                        .IsUnique()
                        .HasDatabaseName("ix_competence_profile_id_category_id");

                    b.ToTable("competence", "profile", t =>
                        {
                            t.HasCheckConstraint("ck_competence_rating_range", "\"rating\" >= 1 AND \"rating\" <= 5");
                        });
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.FeedbackEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<string>("Comment")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)")
                        .HasColumnName("comment");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_date");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer")
                        .HasColumnName("order_id");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<int>("RevieweeUserId")
                        .HasColumnType("integer")
                        .HasColumnName("reviewee_user_id");

                    b.Property<int>("ReviewerUserId")
                        .HasColumnType("integer")
                        .HasColumnName("reviewer_user_id");

                    b.HasKey("Id")
                        .HasName("pk_feedback");

                    b.HasIndex("LastModifiedDate")
                        .HasDatabaseName("ix_feedback_last_modified_date");

                    b.HasIndex("RevieweeUserId")
                        .HasDatabaseName("ix_feedback_reviewee_user_id");

                    b.HasIndex("ReviewerUserId")
                        .HasDatabaseName("ix_feedback_reviewer_user_id");

                    b.ToTable("feedback", "profile", t =>
                        {
                            t.HasCheckConstraint("ck_feedback_rating_range", "\"rating\" >= 1 AND \"rating\" <= 5");
                        });
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("code");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_language");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_language_code");

                    b.ToTable("language", "profile");
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.LanguageProficiency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer")
                        .HasColumnName("language_id");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_date");

                    b.Property<int>("Level")
                        .HasColumnType("integer")
                        .HasColumnName("level");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    b.HasKey("Id")
                        .HasName("pk_language_proficiencies");

                    b.HasIndex("LanguageId")
                        .HasDatabaseName("ix_language_proficiencies_language_id");

                    b.HasIndex("ProfileId", "LanguageId")
                        .IsUnique()
                        .HasDatabaseName("ix_language_proficiencies_profile_id_language_id");

                    b.ToTable("language_proficiencies", "profile");
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Bio")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("bio");

                    b.Property<int?>("CityId")
                        .HasColumnType("integer")
                        .HasColumnName("city_id");

                    b.Property<string>("CvPath")
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("cv_path");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("first_name");

                    b.Property<string>("ImagePath")
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("image_path");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_date");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_name");

                    b.Property<float?>("Rating")
                        .HasPrecision(3, 2)
                        .HasColumnType("real")
                        .HasColumnName("rating");

                    b.Property<int>("Sex")
                        .HasColumnType("integer")
                        .HasColumnName("sex");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_profile");

                    b.HasAlternateKey("UserId")
                        .HasName("ak_profile_user_id");

                    b.HasIndex("Rating")
                        .HasDatabaseName("ix_profile_rating");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_profile_user_id");

                    b.ToTable("profile", "profile", t =>
                        {
                            t.HasCheckConstraint("ck_profile_rating_range", "\"rating\" >= 1 AND \"rating\" <= 5");
                        });
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.Competence", b =>
                {
                    b.HasOne("Doerly.Module.Profile.DataAccess.Models.Profile", "Profile")
                        .WithMany("Competences")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_competence_profile_profile_id");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.FeedbackEntity", b =>
                {
                    b.HasOne("Doerly.Module.Profile.DataAccess.Models.Profile", "RevieweeProfile")
                        .WithMany("FeedbackReceived")
                        .HasForeignKey("RevieweeUserId")
                        .HasPrincipalKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_feedback_profile_reviewee_user_id");

                    b.HasOne("Doerly.Module.Profile.DataAccess.Models.Profile", "ReviewerProfile")
                        .WithMany("FeedbackGiven")
                        .HasForeignKey("ReviewerUserId")
                        .HasPrincipalKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_feedback_profile_reviewer_user_id");

                    b.Navigation("RevieweeProfile");

                    b.Navigation("ReviewerProfile");
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.LanguageProficiency", b =>
                {
                    b.HasOne("Doerly.Module.Profile.DataAccess.Models.Language", "Language")
                        .WithMany("LanguageProficiencies")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_language_proficiencies_language_language_id");

                    b.HasOne("Doerly.Module.Profile.DataAccess.Models.Profile", "Profile")
                        .WithMany("LanguageProficiencies")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_language_proficiencies_profile_profile_id");

                    b.Navigation("Language");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.Language", b =>
                {
                    b.Navigation("LanguageProficiencies");
                });

            modelBuilder.Entity("Doerly.Module.Profile.DataAccess.Models.Profile", b =>
                {
                    b.Navigation("Competences");

                    b.Navigation("FeedbackGiven");

                    b.Navigation("FeedbackReceived");

                    b.Navigation("LanguageProficiencies");
                });
#pragma warning restore 612, 618
        }
    }
}
