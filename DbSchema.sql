use TheDessertHouse

create table dbo.Categories(
	Id int identity primary key,
	DateAdded datetime not null,
	AddedBy nvarchar(256) not null,
	Title nvarchar(256) not null,
	Path nvarchar(256) not null,
	Importance int not null,
	Description nvarchar(4000) null,
	ImageUrl nvarchar(256) null
	
)

go

create table dbo.Articles(
 Id int identity primary key,
 CategoryId int not null,
 DateAdded datetime not null,
 AddedBy nvarchar(256) not null,
 Title nvarchar(256) not null,
 Path nvarchar(256) not null,
 Abstract nvarchar(4000) null,
 Body ntext not null,
 Country nvarchar(256) null,
 State nvarchar(256) null,
 City nvarchar(256) null,
 ReleaseDate datetime not null,
 ExpireDate datetime null,
 Approved bit not null,
 Listed bit not null,
 CommentsEnabled bit not null,
 OnlyForMembers bit not null,
 ViewCount int not null,
 Votes int not null,
 TotalRating int not null
 Constraint fk_ArticleCategory Foreign Key (CategoryId) references dbo.Categories(Id)
 )
 
 go
 
 create table dbo.Comments(
	Id int identity primary key,
	ArticleId int not null,
	DateAdded datetime not null,
	AddedBy nvarchar(256) not null,
	AddedByEmail nvarchar(256) not null,
	AddedByIP nvarchar(15) not null,
	Body ntext not null
	Constraint fk_CommentArticle Foreign Key (ArticleId) references dbo.Articles(Id)
 )
 
 go