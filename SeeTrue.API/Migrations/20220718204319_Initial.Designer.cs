﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SeeTrue.API.DB;

#nullable disable

namespace SeeTrue.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220718204319_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("SeeTrue.Models.AuditLogEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("InstanceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Payload")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AuditLogEntries");
                });

            modelBuilder.Entity("SeeTrue.Models.Login", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserAgent")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("SeeTrue.Models.Mail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Audience")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("InstanceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Template")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("InstanceId", "Language", "Type", "Audience")
                        .IsUnique();

                    b.ToTable("Mails");
                });

            modelBuilder.Entity("SeeTrue.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InstanceID")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LoginId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Revoked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LoginId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("SeeTrue.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AppMetaData")
                        .HasColumnType("TEXT");

                    b.Property<string>("Aud")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ConfirmationSentAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConfirmationToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ConfirmedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailChange")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EmailChangeSentAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailChangeToken")
                        .HasColumnType("TEXT");

                    b.Property<string>("EncryptedPassword")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("InstanceID")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("InvitedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("IsSuperAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Language")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastSignInAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("RecoverySentAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("RecoveryToken")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserMetaData")
                        .HasColumnType("TEXT");

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