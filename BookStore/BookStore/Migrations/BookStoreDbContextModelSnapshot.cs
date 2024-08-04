﻿// <auto-generated />
using System;
using BookStore.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookStore.Migrations
{
    [DbContext(typeof(BookStoreDbContext))]
    partial class BookStoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookStore.Entities.Admin", b =>
                {
                    b.Property<int>("admin_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("admin_id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("admin_id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("BookStore.Entities.Author", b =>
                {
                    b.Property<int>("author_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("author_id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("author_image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("biography")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.HasKey("author_id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("BookStore.Entities.Books", b =>
                {
                    b.Property<int>("book_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("book_id"));

                    b.Property<int>("AuthorAuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("GenreGenreId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("book_image")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("offerOfferId")
                        .HasColumnType("int");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(5,2)");

                    b.Property<DateTime>("publication_date")
                        .HasColumnType("date");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.HasKey("book_id");

                    b.HasIndex("AuthorAuthorId");

                    b.HasIndex("GenreGenreId");

                    b.HasIndex("offerOfferId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BookStore.Entities.Enquiry", b =>
                {
                    b.Property<int>("enq_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("enq_id"));

                    b.Property<byte>("isRead")
                        .HasColumnType("tinyint");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user_email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("enq_id");

                    b.ToTable("Enquiries");
                });

            modelBuilder.Entity("BookStore.Entities.Genre", b =>
                {
                    b.Property<int>("genre_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("genre_id"));

                    b.Property<string>("genre_name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar");

                    b.HasKey("genre_id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("BookStore.Entities.Offer", b =>
                {
                    b.Property<int>("offer_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("offer_id"));

                    b.Property<decimal>("discount")
                        .HasColumnType("decimal(5,2)");

                    b.Property<DateTime>("endDate")
                        .HasColumnType("date");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("startDate")
                        .HasColumnType("date");

                    b.HasKey("offer_id");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("BookStore.Entities.Books", b =>
                {
                    b.HasOne("BookStore.Entities.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookStore.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreGenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookStore.Entities.Offer", "Offer")
                        .WithMany()
                        .HasForeignKey("offerOfferId");

                    b.Navigation("Author");

                    b.Navigation("Genre");

                    b.Navigation("Offer");
                });
#pragma warning restore 612, 618
        }
    }
}
