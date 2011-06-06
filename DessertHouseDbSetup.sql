
    drop table if exists Articles

    drop table if exists Categories

    drop table if exists "Comments"

    drop table if exists Departments

    drop table if exists Forums

    drop table if exists ForumPosts

    drop table if exists "ForumPostVote"

    drop table if exists "Newsletter"

    drop table if exists OrderItems

    drop table if exists Orders

    drop table if exists "Poll"

    drop table if exists "PollOptions"

    drop table if exists Products

    drop table if exists ShippingMethods

    create table Articles (
        Id  integer,
       DateAdded DATETIME,
       AddedBy TEXT not null,
       Title TEXT not null,
       Path TEXT not null,
       Abstract TEXT,
       Body TEXT not null,
       Country TEXT,
       State TEXT,
       City TEXT,
       ReleaseDate DATETIME not null,
       ExpireDate DATETIME,
       Approved INTEGER not null,
       Listed INTEGER not null,
       CommentsEnabled INTEGER not null,
       OnlyForMembers INTEGER not null,
       ViewCount INTEGER not null,
       Votes INTEGER not null,
       TotalRating INTEGER not null,
       CategoryId INTEGER,
       primary key (Id)
    )

    create table Categories (
        Id  integer,
       DateAdded DATETIME not null,
       AddedBy TEXT not null,
       Title TEXT not null,
       Path TEXT not null,
       Importance INTEGER not null,
       Description TEXT,
       ImageUrl TEXT,
       primary key (Id)
    )

    create table "Comments" (
        Id  integer,
       DateAdded DATETIME not null,
       AddedBy TEXT not null,
       AddedByEmail TEXT not null,
       AddedByIP TEXT not null,
       Body TEXT not null,
       ArticleId INTEGER,
       primary key (Id)
    )

    create table Departments (
        Id  integer,
       AddedBy TEXT not null,
       DateAdded DATETIME not null,
       Description TEXT,
       ImageUrl TEXT,
       Importance INTEGER not null,
       Title TEXT not null,
       primary key (Id)
    )

    create table Forums (
        Id  integer,
       DateAdded DATETIME not null,
       AddedBy TEXT not null,
       Description TEXT not null,
       Importance INTEGER not null,
       Moderated INTEGER not null,
       Path TEXT not null,
       Title TEXT not null,
       primary key (Id)
    )

    create table ForumPosts (
        Id  integer,
       DateAdded DATETIME not null,
       AddedBy TEXT not null,
       AddedByIP TEXT not null,
       Title TEXT not null,
       Path TEXT not null,
       Body TEXT not null,
       Approved INTEGER not null,
       Closed INTEGER not null,
       VoteCount INTEGER,
       ViewCount INTEGER,
       ReplyCount INTEGER,
       LastPostBy TEXT,
       LastPostDate DATETIME,
       ParentPostId INTEGER,
       ForumId INTEGER,
       primary key (Id)
    )

    create table "ForumPostVote" (
        Id  integer,
       AddedBy TEXT not null,
       AddedByIP TEXT not null,
       DateAdded DATETIME not null,
       Direction INTEGER not null,
       PostId INTEGER,
       primary key (Id)
    )

    create table "Newsletter" (
        Id  integer,
       DateAdded DATETIME not null,
       DateSent DATETIME,
       HtmlBody TEXT not null,
       PlainTextBody TEXT not null,
       Subject TEXT not null,
       AddedBy TEXT not null,
       Status TEXT,
       primary key (Id)
    )

    create table OrderItems (
        Id  integer,
       AddedBy TEXT not null,
       DateAdded DATETIME not null,
       ProductId INTEGER not null,
       Quantity INTEGER not null,
       SKU TEXT not null,
       Title TEXT not null,
       UnitPrice NUMERIC not null,
       OrderId INTEGER,
       primary key (Id)
    )

    create table Orders (
        Id  integer,
       AddedBy TEXT not null,
       CustomerEmail TEXT not null,
       DateAdded DATETIME not null,
       ShippedDate DATETIME,
       Shipping NUMERIC not null,
       ShippingCity TEXT not null,
       ShippingFirstName TEXT not null,
       ShippingLastName TEXT not null,
       ShippingMethod TEXT not null,
       ShippingState TEXT not null,
       ShippingStreet TEXT not null,
       ShippingZipCode TEXT not null,
       Status TEXT not null,
       SubTotal NUMERIC not null,
       TrackingId TEXT,
       TransactionId TEXT,
       primary key (Id)
    )

    create table "Poll" (
        Id  integer,
       AddedBy TEXT not null,
       DateAdded DATETIME not null,
       DateArchived DATETIME,
       IsArchived INTEGER not null,
       IsCurrent INTEGER not null,
       Path TEXT not null,
       PollQuestion TEXT not null,
       primary key (Id)
    )

    create table "PollOptions" (
        Id  integer,
       DateAdded DATETIME not null,
       OptionText TEXT not null,
       Votes INTEGER not null,
       PollId INTEGER,
       primary key (Id)
    )

    create table Products (
        Id  integer,
       AddedBy TEXT not null,
       DateAdded DATETIME not null,
       Description TEXT,
       DiscountPercentage NUMERIC,
       SKU TEXT not null,
       UnitPrice NUMERIC not null,
       UnitsInStock INTEGER not null,
       SmallImageUrl TEXT,
       FullImageUrl TEXT,
       Title TEXT not null,
       DepartmentId INTEGER,
       primary key (Id)
    )

    create table ShippingMethods (
        Id  integer,
       AddedBy TEXT not null,
       DateAdded DATETIME not null,
       Title TEXT not null,
       Price NUMERIC not null,
       primary key (Id)
    )
