create table Customer(
	CustomerId int identity (1,1) primary key,
	UserName varchar(50) null,
	Password varchar(25) null
)

insert into Customer (UserName,Password)
values ('Milos','test')
insert into Customer (UserName,Password)
values ('Marko','test1')
insert into Customer (UserName,Password)
values ('Petar','test2')

create table CustomerAccount (
	IdCustomerAccount int identity (1,1) primary key,
	CustomerId int foreign key references Customer(CustomerId),
	Balance decimal (19,2) null
)

insert into CustomerAccount(CustomerId,Balance)
values (1,1000)
insert into CustomerAccount(CustomerId,Balance)
values (2,2000)
insert into CustomerAccount(CustomerId,Balance)
values (3,3000)

update CustomerAccount set Balance=100000 where CustomerId=1


create table StoreAccount(
	IdStoreAccount int identity(1,1) primary key,
	IdStore int ,
	Balance decimal(19,2) not null,
	TransactionId UNIQUEIDENTIFIER default newid() null,
	TransactionPlus decimal(19,2) null,
	TransactionMinus decimal(19,2) null,
	CustomerId int foreign key references Customer(CustomerId) null,
	Refund int null
)
insert into StoreAccount(Balance,TransactionPlus,idstore)
values (10000,10000,1)
insert into StoreAccount(Balance,TransactionMinus,CustomerId,idstore)
values (9800,200,1,1)


create table CurrencyRate(
	IdCurrencyRate int identity (1,1) primary key,
	CurrencyCode int,
	Currency varchar(3),
	CurrencyRateValue decimal (19,2)
)

insert into CurrencyRate (CurrencyCode,Currency,CurrencyRateValue)
values (941,'RSD',1)
insert into CurrencyRate (CurrencyCode,Currency,CurrencyRateValue)
values (978,'EUR',118)
insert into CurrencyRate (CurrencyCode,Currency,CurrencyRateValue)
values (840,'USD',120)



select * from Customer
select * from CurrencyRate
select * from CustomerAccount
select * from StoreAccount



--drop table CurrencyRate 
--drop table CustomerAccount
--drop table StoreAccount 
--drop table Customer 
