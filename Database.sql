USE [master]
GO
/****** Object:  Database [LibraryManagement]    Script Date: 07/03/2025 9:26:01 AM ******/
CREATE DATABASE [LibraryManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LibraryManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\LibraryManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LibraryManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\LibraryManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [LibraryManagement] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LibraryManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LibraryManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LibraryManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LibraryManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LibraryManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LibraryManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [LibraryManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LibraryManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LibraryManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LibraryManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LibraryManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LibraryManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LibraryManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LibraryManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LibraryManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LibraryManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LibraryManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LibraryManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LibraryManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LibraryManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LibraryManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LibraryManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LibraryManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LibraryManagement] SET RECOVERY FULL 
GO
ALTER DATABASE [LibraryManagement] SET  MULTI_USER 
GO
ALTER DATABASE [LibraryManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LibraryManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LibraryManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LibraryManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LibraryManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LibraryManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LibraryManagement', N'ON'
GO
ALTER DATABASE [LibraryManagement] SET QUERY_STORE = OFF
GO
USE [LibraryManagement]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 07/03/2025 9:26:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[BookID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Author] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BookID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BorrowingRecords]    Script Date: 07/03/2025 9:26:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BorrowingRecords](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[ReaderID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[BorrowDate] [datetime] NOT NULL,
	[ReturnDate] [datetime] NULL,
	[Status] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Readers]    Script Date: 07/03/2025 9:26:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Readers](
	[ReaderID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ReaderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 07/03/2025 9:26:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Role] [nvarchar](20) NOT NULL,
	[ReaderID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([BookID], [Title], [Author], [Category], [Quantity], [Price]) VALUES (1, N'Clean Code', N'Robert C. Martin', N'Programming', 5, CAST(499.99 AS Decimal(10, 2)))
INSERT [dbo].[Books] ([BookID], [Title], [Author], [Category], [Quantity], [Price]) VALUES (2, N'Design Patterns', N'Erich Gamma', N'Software Engineering', 3, CAST(399.99 AS Decimal(10, 2)))
INSERT [dbo].[Books] ([BookID], [Title], [Author], [Category], [Quantity], [Price]) VALUES (3, N'The Pragmatic Programmer', N'Andy Hunt', N'Software Development', 5, CAST(299.99 AS Decimal(10, 2)))
INSERT [dbo].[Books] ([BookID], [Title], [Author], [Category], [Quantity], [Price]) VALUES (4, N'Introduction to Algorithms', N'Thomas H. Cormen', N'Computer Science', 3, CAST(599.99 AS Decimal(10, 2)))
INSERT [dbo].[Books] ([BookID], [Title], [Author], [Category], [Quantity], [Price]) VALUES (5, N'Database Systems', N'Ramez Elmasri', N'Database', 6, CAST(450.00 AS Decimal(10, 2)))
INSERT [dbo].[Books] ([BookID], [Title], [Author], [Category], [Quantity], [Price]) VALUES (6, N'Artificial Intelligence: A Modern Approach', N'Stuart Russell', N'AI', 5, CAST(550.00 AS Decimal(10, 2)))
INSERT [dbo].[Books] ([BookID], [Title], [Author], [Category], [Quantity], [Price]) VALUES (7, N'Đắc Nhân Tâm', N'Đang quên', N'Đời sống', 99, CAST(150.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[Books] OFF
GO
SET IDENTITY_INSERT [dbo].[BorrowingRecords] ON 

INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (1, 1, 1, CAST(N'2025-02-18T08:33:22.703' AS DateTime), CAST(N'2025-02-28T00:00:00.000' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (2, 2, 2, CAST(N'2025-02-08T08:33:22.703' AS DateTime), CAST(N'2025-02-23T08:33:22.703' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (3, 3, 3, CAST(N'2025-02-12T08:33:22.703' AS DateTime), CAST(N'2025-02-23T08:33:22.703' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (4, 4, 4, CAST(N'2025-02-20T08:33:22.703' AS DateTime), CAST(N'2025-02-23T08:33:22.703' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (5, 1, 5, CAST(N'2025-02-23T08:33:22.703' AS DateTime), CAST(N'2025-02-23T08:33:22.703' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (6, 2, 1, CAST(N'2022-03-21T00:00:00.000' AS DateTime), CAST(N'2025-03-07T00:00:00.000' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (7, 1, 1, CAST(N'2025-03-07T02:07:51.653' AS DateTime), CAST(N'2025-02-23T08:33:22.703' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (8, 2, 1, CAST(N'2025-03-07T03:24:02.287' AS DateTime), CAST(N'2025-07-26T00:00:00.000' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (9, 1, 1, CAST(N'2025-03-07T03:33:59.740' AS DateTime), CAST(N'2025-03-21T03:33:59.740' AS DateTime), N'Returned')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (10, 1, 1, CAST(N'2025-03-07T08:28:16.637' AS DateTime), CAST(N'2025-03-01T08:28:16.637' AS DateTime), N'Borrowed')
INSERT [dbo].[BorrowingRecords] ([RecordID], [ReaderID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (11, 9, 7, CAST(N'2025-03-07T09:16:31.727' AS DateTime), CAST(N'2025-03-29T00:00:00.000' AS DateTime), N'Returned')
SET IDENTITY_INSERT [dbo].[BorrowingRecords] OFF
GO
SET IDENTITY_INSERT [dbo].[Readers] ON 

INSERT [dbo].[Readers] ([ReaderID], [FullName], [Email], [PhoneNumber]) VALUES (1, N'Nguyễn Văn A', N'vana@example.com', N'0123456789')
INSERT [dbo].[Readers] ([ReaderID], [FullName], [Email], [PhoneNumber]) VALUES (2, N'Trần Thị B', N'thib@example.com', N'0987654321')
INSERT [dbo].[Readers] ([ReaderID], [FullName], [Email], [PhoneNumber]) VALUES (3, N'Lê Văn C', N'vanc@example.com', N'0909090909')
INSERT [dbo].[Readers] ([ReaderID], [FullName], [Email], [PhoneNumber]) VALUES (4, N'Phạm Văn D', N'vand@example.com', N'0911223344')
INSERT [dbo].[Readers] ([ReaderID], [FullName], [Email], [PhoneNumber]) VALUES (5, N'Vũ Quang Huy', N'vuhuy2048@gmail.com', N'0981247490')
INSERT [dbo].[Readers] ([ReaderID], [FullName], [Email], [PhoneNumber]) VALUES (9, N'Nguyễn Văn D', N'vanacc@example.com', N'0123416789')
SET IDENTITY_INSERT [dbo].[Readers] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [ReaderID]) VALUES (1, N'admin', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Admin', NULL)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [ReaderID]) VALUES (2, N'reader1', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Reader', 1)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [ReaderID]) VALUES (3, N'reader2', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Reader', 2)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [ReaderID]) VALUES (4, N'vuhuy2048', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Admin', 5)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Role], [ReaderID]) VALUES (8, N'reader3', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Reader', 9)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Readers__85FB4E38C9B67EBA]    Script Date: 07/03/2025 9:26:02 AM ******/
ALTER TABLE [dbo].[Readers] ADD UNIQUE NONCLUSTERED 
(
	[PhoneNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Readers__A9D1053417811DB0]    Script Date: 07/03/2025 9:26:02 AM ******/
ALTER TABLE [dbo].[Readers] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E489C7FB70]    Script Date: 07/03/2025 9:26:02 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BorrowingRecords] ADD  DEFAULT (getdate()) FOR [BorrowDate]
GO
ALTER TABLE [dbo].[BorrowingRecords]  WITH CHECK ADD FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BorrowingRecords]  WITH CHECK ADD FOREIGN KEY([ReaderID])
REFERENCES [dbo].[Readers] ([ReaderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([ReaderID])
REFERENCES [dbo].[Readers] ([ReaderID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD CHECK  (([Price]>=(0)))
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD CHECK  (([Quantity]>=(0)))
GO
ALTER TABLE [dbo].[BorrowingRecords]  WITH CHECK ADD CHECK  (([Status]='Overdue' OR [Status]='Returned' OR [Status]='Borrowed'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='Reader' OR [Role]='Admin'))
GO
USE [master]
GO
ALTER DATABASE [LibraryManagement] SET  READ_WRITE 
GO
