﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VIOBANK.PostgresPersistence;

#nullable disable

namespace VIOBANK.PostgresPersistence.Migrations
{
    [DbContext(typeof(VIOBANKDbContext))]
    partial class VIOBANKDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VIOBANK.Domain.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountId"));

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(28)
                        .HasColumnType("character varying(28)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("AccountId");

                    b.HasIndex("AccountNumber")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CardId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<string>("Bank")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("CardPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Cvc")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("HolderName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CardId");

                    b.HasIndex("AccountId");

                    b.HasIndex("CardNumber")
                        .IsUnique();

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ContactId"));

                    b.Property<string>("ContactCard")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ContactPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ContactId");

                    b.HasIndex("ContactCard")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Deposit", b =>
                {
                    b.Property<int>("DepositId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DepositId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int>("CardId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("DurationMonths")
                        .HasColumnType("integer");

                    b.Property<decimal>("InitialAmount")
                        .HasColumnType("numeric");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.HasKey("DepositId");

                    b.HasIndex("CardId");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.DepositTransaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("DepositId")
                        .HasColumnType("integer");

                    b.HasKey("TransactionId");

                    b.HasIndex("DepositId");

                    b.ToTable("DepositTransactions");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.MobileTopup", b =>
                {
                    b.Property<int>("TopupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TopupId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("TopupId");

                    b.HasIndex("UserId");

                    b.ToTable("MobileTopups");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Settings", b =>
                {
                    b.Property<int>("SettingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SettingId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<bool>("NotificationsEmail")
                        .HasColumnType("boolean");

                    b.Property<bool>("NotificationsSms")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("SettingId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CurrencyFrom")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("CurrencyTo")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FromCardId")
                        .HasColumnType("integer");

                    b.Property<int>("ToCardId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TransactionId");

                    b.HasIndex("FromCardId");

                    b.HasIndex("ToCardId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("IdCard")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Registration")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TaxNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("IdCard")
                        .IsUnique();

                    b.HasIndex("TaxNumber")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.WithdrawnDeposit", b =>
                {
                    b.Property<int>("WithdrawnDepositId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WithdrawnDepositId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<decimal>("InterestEarned")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("WithdrawnAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("WithdrawnDepositId");

                    b.HasIndex("UserId");

                    b.ToTable("WithdrawnDeposits");
                });

            modelBuilder.Entity("VIOBANK.PostgresPersistence.Entities.BlacklistedToken", b =>
                {
                    b.Property<string>("Token")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Token");

                    b.HasIndex("ExpiryDate");

                    b.ToTable("BlacklistedTokens", (string)null);
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Account", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Card", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.Account", "Account")
                        .WithMany("Cards")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Contact", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.User", "User")
                        .WithMany("Contacts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Deposit", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.Card", "Card")
                        .WithMany("Deposits")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.DepositTransaction", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.Deposit", "Deposit")
                        .WithMany("DepositTransactions")
                        .HasForeignKey("DepositId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Deposit");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.MobileTopup", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.User", "User")
                        .WithMany("MobileTopups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Settings", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.User", "User")
                        .WithOne("Settings")
                        .HasForeignKey("VIOBANK.Domain.Models.Settings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Transaction", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.Card", "FromCard")
                        .WithMany("TransactionsFrom")
                        .HasForeignKey("FromCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VIOBANK.Domain.Models.Card", "ToCard")
                        .WithMany("TransactionsTo")
                        .HasForeignKey("ToCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromCard");

                    b.Navigation("ToCard");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.User", b =>
                {
                    b.OwnsOne("VIOBANK.Domain.Models.Employment", "Employment", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("integer");

                            b1.Property<string>("Income")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<string>("Type")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Employment")
                        .IsRequired();
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.WithdrawnDeposit", b =>
                {
                    b.HasOne("VIOBANK.Domain.Models.User", "User")
                        .WithMany("WithdrawnDeposits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Account", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Card", b =>
                {
                    b.Navigation("Deposits");

                    b.Navigation("TransactionsFrom");

                    b.Navigation("TransactionsTo");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.Deposit", b =>
                {
                    b.Navigation("DepositTransactions");
                });

            modelBuilder.Entity("VIOBANK.Domain.Models.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Contacts");

                    b.Navigation("MobileTopups");

                    b.Navigation("Settings")
                        .IsRequired();

                    b.Navigation("WithdrawnDeposits");
                });
#pragma warning restore 612, 618
        }
    }
}
