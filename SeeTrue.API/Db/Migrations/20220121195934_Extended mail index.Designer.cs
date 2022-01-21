﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SeeTrue.API.Db;

#nullable disable

namespace SeeTrue.API.Db.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220121195934_Extended mail index")]
    partial class Extendedmailindex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SeeTrue.Models.AuditLogEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("InstanceId")
                        .HasColumnType("uuid");

                    b.Property<Dictionary<string, string>>("Payload")
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.ToTable("AuditLogEntries");
                });

            modelBuilder.Entity("SeeTrue.Models.Login", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserAgent")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("SeeTrue.Models.Mail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Audience")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("InstanceId")
                        .HasColumnType("uuid");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Template")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InstanceId", "Language", "Type", "Audience")
                        .IsUnique();

                    b.ToTable("Mails");
                });

            modelBuilder.Entity("SeeTrue.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("InstanceID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LoginId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Revoked")
                        .HasColumnType("boolean");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LoginId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("SeeTrue.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AppMetaDataJson")
                        .HasColumnType("jsonb");

                    b.Property<string>("Aud")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ConfirmationSentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ConfirmationToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("EmailChange")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EmailChangeSentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailChangeToken")
                        .HasColumnType("text");

                    b.Property<string>("EncryptedPassword")
                        .HasColumnType("text");

                    b.Property<Guid>("InstanceID")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("InvitedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsSuperAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Language")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastSignInAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RecoverySentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecoveryToken")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserMetaDataJson")
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("Email", "InstanceID", "Aud")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SeeTrue.Models.Login", b =>
                {
                    b.HasOne("SeeTrue.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SeeTrue.Models.RefreshToken", b =>
                {
                    b.HasOne("SeeTrue.Models.Login", "Login")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("LoginId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SeeTrue.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Login");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SeeTrue.Models.Login", b =>
                {
                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("SeeTrue.Models.User", b =>
                {
                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
