if not exists (select * from [sys].[databases] where [name] = 'Thesaurus')
begin
	create database [Thesaurus];
end
go

use [Thesaurus];
if not exists (select * from [sysobjects] where [name] = 'Meanings' and [xtype] = 'U')
begin
	create table [Meanings] (
		[ID] int identity(1,1) not null primary key,
	);
end
go

if not exists (select * from [sysobjects] where [name] = 'Words' and [xtype] = 'U')
begin
	create table [Words] (
		[ID] int identity(1,1) not null primary key,
		[Word] varchar(100) not null,
		[MeaningID] int not null
	);
end
go

if not exists (select * from [information_schema].[referential_constraints] where [constraint_name] = 'FK_Word_Meaning')
begin
	alter table [Words] add constraint [FK_Word_Meaning] foreign key (MeaningID) references [Meanings](ID);
end
go
