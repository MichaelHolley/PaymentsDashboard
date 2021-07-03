﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentsDashboard.Data;

namespace PaymentsDashboard.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("PaymentTag", b =>
                {
                    b.Property<Guid>("PaymentsPaymentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TagsTagId")
                        .HasColumnType("TEXT");

                    b.HasKey("PaymentsPaymentId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("PaymentTag");
                });

            modelBuilder.Entity("PaymentsDashboard.Data.Modells.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("PaymentId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PaymentsDashboard.Data.Modells.ReoccuringPayment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReoccuringType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ReoccuringPayments");
                });

            modelBuilder.Entity("PaymentsDashboard.Data.Modells.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("HexColorCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ReoccuringPaymentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("TagId");

                    b.HasIndex("ReoccuringPaymentId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PaymentTag", b =>
                {
                    b.HasOne("PaymentsDashboard.Data.Modells.Payment", null)
                        .WithMany()
                        .HasForeignKey("PaymentsPaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaymentsDashboard.Data.Modells.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PaymentsDashboard.Data.Modells.Tag", b =>
                {
                    b.HasOne("PaymentsDashboard.Data.Modells.ReoccuringPayment", null)
                        .WithMany("Tags")
                        .HasForeignKey("ReoccuringPaymentId");
                });

            modelBuilder.Entity("PaymentsDashboard.Data.Modells.ReoccuringPayment", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
