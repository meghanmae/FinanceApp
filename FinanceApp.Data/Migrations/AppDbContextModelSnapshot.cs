﻿// <auto-generated />
using FinanceApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinanceApp.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FinanceApp.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AzureObjectId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApplicationUserId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.Budget", b =>
                {
                    b.Property<int>("BudgetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BudgetId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BudgetId");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.BudgetUser", b =>
                {
                    b.Property<int>("BudgetUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BudgetUserId"));

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.HasKey("BudgetUserId");

                    b.HasIndex("BudgetId");

                    b.HasIndex("ApplicationUserId", "BudgetId")
                        .IsUnique();

                    b.ToTable("BudgetUsers");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.HasIndex("BudgetId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.CustomCalculation", b =>
                {
                    b.Property<int>("CustomCalculationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomCalculationId"));

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomCalculationId");

                    b.HasIndex("BudgetId");

                    b.ToTable("CustomCalculations");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.SubCategory", b =>
                {
                    b.Property<int>("SubCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubCategoryId"));

                    b.Property<decimal>("Allocation")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsStatic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubCategoryId");

                    b.HasIndex("BudgetId");

                    b.HasIndex("CategoryId");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.SubCategoryCustomCalculation", b =>
                {
                    b.Property<int>("SubCategoryCustomCalculationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubCategoryCustomCalculationId"));

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.Property<int>("CustomCalculationId")
                        .HasColumnType("int");

                    b.Property<int>("SubCategoryId")
                        .HasColumnType("int");

                    b.HasKey("SubCategoryCustomCalculationId");

                    b.HasIndex("BudgetId");

                    b.HasIndex("CustomCalculationId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("SubCategoryCustomCalculations");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.Transaction", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TransactionId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubCategoryId")
                        .HasColumnType("int");

                    b.HasKey("TransactionId");

                    b.HasIndex("BudgetId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.BudgetUser", b =>
                {
                    b.HasOne("FinanceApp.Data.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("BudgetUsers")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FinanceApp.Data.Models.Budget", "Budget")
                        .WithMany("BudgetUsers")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.Category", b =>
                {
                    b.HasOne("FinanceApp.Data.Models.Budget", "Budget")
                        .WithMany("Categories")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.CustomCalculation", b =>
                {
                    b.HasOne("FinanceApp.Data.Models.Budget", "Budget")
                        .WithMany()
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.SubCategory", b =>
                {
                    b.HasOne("FinanceApp.Data.Models.Budget", "Budget")
                        .WithMany()
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FinanceApp.Data.Models.Category", "Category")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Budget");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.SubCategoryCustomCalculation", b =>
                {
                    b.HasOne("FinanceApp.Data.Models.Budget", "Budget")
                        .WithMany()
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FinanceApp.Data.Models.CustomCalculation", "CustomCalculation")
                        .WithMany("SubCategoryCustomCalculations")
                        .HasForeignKey("CustomCalculationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FinanceApp.Data.Models.SubCategory", "SubCategory")
                        .WithMany("SubCategoryCustomCalculations")
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Budget");

                    b.Navigation("CustomCalculation");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.Transaction", b =>
                {
                    b.HasOne("FinanceApp.Data.Models.Budget", "Budget")
                        .WithMany()
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FinanceApp.Data.Models.SubCategory", "SubCategory")
                        .WithMany()
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Budget");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("BudgetUsers");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.Budget", b =>
                {
                    b.Navigation("BudgetUsers");

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.Category", b =>
                {
                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.CustomCalculation", b =>
                {
                    b.Navigation("SubCategoryCustomCalculations");
                });

            modelBuilder.Entity("FinanceApp.Data.Models.SubCategory", b =>
                {
                    b.Navigation("SubCategoryCustomCalculations");
                });
#pragma warning restore 612, 618
        }
    }
}
