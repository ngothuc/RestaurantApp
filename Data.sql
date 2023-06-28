CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

--Food(Món ăn)
--Table(Bàn ăn)
--FoodCategory(Loại thức ăn)
--Bill
--BillInfo

CREATE TABLE TableFood
(
	ID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100),
	status NVARCHAR(100) DEFAULT N'Trống' --Trống || Có người
)
GO

CREATE TABLE Account
(
	UserName NVARCHAR(100) PRIMARY KEY NOT NULL,
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'User',
	PassWord NVARCHAR(100) NOT NULL,
	Type INT NOT NULL DEFAULT 0 -- 1: admin || 0: staff
)
GO 

CREATE TABLE FoodCategory
(
	ID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Food
(
	ID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	IDCategory INT NOT NULL,
	Price FLOAT NOT NULL

	FOREIGN KEY (IDCategory) REFERENCES dbo.FoodCategory(ID)
)
GO

CREATE TABLE Bill
(
	ID INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	IDTable INT NOT NULL,
	Status INT NOT NULL DEFAULT 0, --1 là đã thanh toán || 0 là chưa thanh toán
	FOREIGN KEY (IDTable) REFERENCES dbo.TableFood(ID)
)
GO 

CREATE TABLE BillInfo
(
	ID INT IDENTITY PRIMARY KEY,
	IDBill INT NOT NULL,
	IDFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0

	FOREIGN KEY (IDBill) REFERENCES dbo.Bill(ID),
	FOREIGN KEY (IDFood) REFERENCES dbo.Food(ID)
)
GO

USE QuanLyQuanCafe

INSERT INTO dbo.Account
(
    UserName,
    DisplayName,
    PassWord,
    Type
)
VALUES
(   N'K9', -- UserName - nvarchar(100)
    N'RongK9', -- DisplayName - nvarchar(100)
    N'1', -- PassWord - nvarchar(100)
    1    -- Type - int
)

INSERT INTO dbo.Account
(
    UserName,
    DisplayName,
    PassWord,
    Type
)
VALUES
(   N'staff', -- UserName - nvarchar(100)
    N'staff', -- DisplayName - nvarchar(100)
    N'1', -- PassWord - nvarchar(100)
    0    -- Type - int
)
GO 

CREATE PROC USP_GetAccountByUserName
@username NVARCHAR(100)
AS
BEGIN
	SELECT * FROM dbo.Account
	WHERE UserName = @username
END
GO

EXEC dbo.USP_GetAccountByUserName @username = N'K9' -- nvarchar(100)

SELECT * FROM dbo.Account

SELECT * FROM dbo.Account WHERE UserName = N'K9' AND PassWord = N'1'
GO

CREATE PROC USP_Login
@UserName nvarchar(100), @PassWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @UserName AND PassWord = @PassWord
END
GO

-- Thêm bàn
DECLARE @i INT = 0

WHILE @i <= 10
BEGIN
	INSERT INTO dbo.TableFood (Name) VALUES	(N'Bàn ' + CAST(@i AS NVARCHAR(100)))
	SET @i = @i +1
END

GO

CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.TableFood
GO 

UPDATE dbo.TableFood SET STATUS = N'Có người' WHERE ID = 9 

EXEC dbo.USP_GetTableList

--Thêm Category
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Hải sản')
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Nông sản')
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Lâm sản')
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Đặc sản')
INSERT INTO dbo.FoodCategory (Name) VALUES (N'Nước');


--Thêm món ăn
INSERT INTO Food (Name, IDCategory, Price) VALUES (N'Mực một nắng nướng sa tế', 1, 120000)
INSERT INTO Food (Name, IDCategory, Price) VALUES (N'Nghêu hấp sả', 1, 50000)
INSERT INTO Food (Name, IDCategory, Price) VALUES (N'Dê nướng sữa', 2, 60000)
INSERT INTO Food (Name, IDCategory, Price) VALUES (N'Heo rừng quay', 3, 75000)
INSERT INTO Food (Name, IDCategory, Price) VALUES (N'Cơm chiên Mushi', 4, 50000)
INSERT INTO Food (Name, IDCategory, Price) VALUES (N'7Up', 5, 15000)
INSERT INTO Food (Name, IDCategory, Price) VALUES (N'Cafe', 5, 12000);

--Thêm Bill
INSERT INTO Bill (DateCheckIn, DateCheckOut, IDTable, Status) VALUES (GETDATE(), NULL, 3, 0)
INSERT INTO Bill (DateCheckIn, DateCheckOut, IDTable, Status) VALUES (GETDATE(), NULL, 4, 0)
INSERT INTO Bill (DateCheckIn, DateCheckOut, IDTable, Status) VALUES (GETDATE(), GETDATE(), 5, 1);

--Thêm BillInfo
INSERT INTO BillInfo (IDBill, IDFood, count) VALUES (1,1,2)
INSERT INTO BillInfo (IDBill, IDFood, count) VALUES (1,3,4)
INSERT INTO BillInfo (IDBill, IDFood, count) VALUES (1,5,1)
INSERT INTO BillInfo (IDBill, IDFood, count) VALUES (2,1,2)
INSERT INTO BillInfo (IDBill, IDFood, count) VALUES (2,6,2)
INSERT INTO BillInfo (IDBill, IDFood, count) VALUES (3,5,2);
GO

SELECT f.Name, bi.count, f.Price, f.Price*bi.count AS totalPrice FROM BillInfo AS bi, Bill AS b, Food AS f
WHERE bi.IDBill = b.id AND bi.IDFood = f.ID AND b.IDTable = 3

SELECT * FROM dbo.Bill
SELECT * FROM dbo.BillInfo
SELECT * FROM dbo.Food
SELECT * FROM dbo.FoodCategory
SELECT * FROM TableFood

create proc USP_InsertBill
@idTable int
as
begin
	insert into bill(DateCheckIn, DateCheckOut, IDTable, Status)
	values (GETDATE(), null, @idTable, 0)
end
go

drop proc USP_InsertBillInfo

select * from BillInfo

create proc USP_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	
	declare @isExitBillInfo int
	declare @foodCount int = 1

	select @isExitBillInfo = ID, @foodCount = b.count 
	from BillInfo as b 
	where IDBill = @idBill and IDFood = @idFood

	if (@isExitBillInfo > 0)
	begin
		declare @newCount int = @foodCount + @count
		if (@newCount > 0)
		begin
			update BillInfo set count = @newCount 
			where IDFood = @idFood
		end
		else
		begin
			delete BillInfo 
			where IDBill = @idBill and IDFood = @idFood
		end
	end
	else
	begin
		insert into BillInfo
		values (@idBill, @idFood, @count)
	end
end

exec USP_InsertBillInfo 1,1,2

update Bill set Status = 1 where ID = 1

create TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = idBill FROM Inserted
	
	DECLARE @idTable INT
	
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status = 0

	declare @count int
	select @count = count(*) from BillInfo where IDBill = @idBill

	if (@count > 0)
	UPDATE dbo.TableFood SET status = N'Có người' WHERE id = @idTable
	else
	UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
END
GO

CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = id FROM Inserted	
	
	DECLARE @idTable INT
	
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count int = 0
	
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable = @idTable AND status = 0
	
	IF (@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
END
GO

alter table bill add Discount int;
 update bill set discount = 0;

create proc USP_SwitchTable
@idTable1 int, @idTable2 int
as
begin
	
	declare @idFirstBill int
	declare @idSecondBill int

	declare @isFirstTableEmpty int = 1
	declare @isSecondTableEmpty int = 1

	select @idSecondBill = ID from Bill where IDTable = @idTable2 and status = 0
	select @idFirstBill = ID from Bill where IDTable = @idTable1 and status = 0

	print @idFirstBill
	print @idSecondBill
	print '-----------'


	if (@idFirstBill is null)
	begin
		INSERT INTO Bill (DateCheckIn, DateCheckOut, IDTable, Status) VALUES (GETDATE(), NULL, @idTable1, 0)

		select @idFirstBill = max(id) from Bill where IDTable = @idTable1 and status = 0

	end

	select @isFirstTableEmpty = count(*) from BillInfo where IDBill = @idFirstBill

	if (@idSecondBill is null)
	begin
		INSERT INTO Bill (DateCheckIn, DateCheckOut, IDTable, Status) VALUES (GETDATE(), NULL, @idTable2, 0)

		select @idSecondBill = max(id) from Bill where IDTable = @idTable2 and status = 0

	end

	select @isSecondTableEmpty = count(*) from BillInfo where IDBill = @idSecondBill
	select id into IDBillInfoTable from BillInfo where IDBill = @idSecondBill

	update BillInfo set IDBill = @idSecondBill where IDBill = @idFirstBill

	update BillInfo set IDBill = @idFirstBill where IDBill in (select * from IDBillInfoTable)

	drop table IDBillInfoTable

	if (@isFirstTableEmpty = 0)
		update TableFood set status = N'Trống' where ID = @idTable2

	if (@isSecondTableEmpty = 0)
		update TableFood set status = N'Trống' where ID = @idTable1
end
go

exec USP_SwitchTable @idTable1 = 10, @idTable2 = 19;

alter PROC USP_SwitchTabel
@idTable1 INT, @idTable2 int
AS BEGIN

	DECLARE @idFirstBill int
	DECLARE @idSeconrdBill INT
	
	DECLARE @isFirstTablEmty INT = 1
	DECLARE @isSecondTablEmty INT = 1
	
	
	SELECT @idSeconrdBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
	
	IF (@idFirstBill IS NULL)
	BEGIN
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idTable ,
		          status
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idTable1 , -- idTable - int
		          0  -- status - int
		        )
		        
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
		
	END
	
	SELECT @isFirstTablEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idFirstBill
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	
	IF (@idSeconrdBill IS NULL)
	BEGIN
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idTable ,
		          status
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idTable2 , -- idTable - int
		          0  -- status - int
		        )
		SELECT @idSeconrdBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
		
	END
	
	SELECT @isSecondTablEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idSeconrdBill
	

	SELECT id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSeconrdBill
	
	UPDATE dbo.BillInfo SET idBill = @idSeconrdBill WHERE idBill = @idFirstBill
	
	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)
	
	DROP TABLE IDBillInfoTable
	
	IF (@isFirstTablEmty = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable2
		
	IF (@isSecondTablEmty= 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable1

	update TableFood set status = N'Trống' where id = @idTable2;
END
GO

alter table bill add TotalPrice float default 0
delete BillInfo
delete Bill

select * from bill where DateCheckIn >= '20230619' and DateCheckOut <= '20230619' and status = 1
select * from BillInfo
select * from food
select　* from FoodCategory
select * from TableFood

select t.Name, b.totalPrice, b.DateCheckIn, b.DateCheckOut, b.Discount
from Bill as b, TableFood as t
where b.DateCheckIn >= '20230619' and b.DateCheckOut <= '20230619' and b.status = 1 and b.IDTable = t.ID
go

alter proc USP_GetListBillByDate
@checkIn date, @checkOut date
as
begin
	select t.Name as N'Tên bàn', b.totalPrice as N'Tổng tiền', b.DateCheckIn as N'Ngày vào', b.DateCheckOut as N'Ngày ra', b.Discount as N'Giảm giá'
	from Bill as b, TableFood as t
	where b.DateCheckIn >= @checkIn and b.DateCheckOut <= @checkOut and b.status = 1 and b.IDTable = t.ID
end
go

create proc USP_UpdateAccount
@userName nvarchar(100), @displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0

	select @isRightPass = count(*) from Account where UserName = @userName and PassWord = @password

	if (@isRightPass = 1)
	begin
		if (@newPassword = null or @newPassword = ' ')
		begin
			update Account set DisplayName = @displayName where UserName = @userName
		end
		else
			update Account set DisplayName = @displayName, PassWord = @newPassword where UserName = @userName

	end
end
go
select * from Food

INSERT INTO Food (Name, IDCategory, Price) VALUES (N'Mực một nắng nướng sa tế', 1, 120000);

update food set Name = N'', IDCategory = 5, Price = 0 where ID = 4

create trigger UTG_DeleteBillInfo
on BillInfo for delete
as
begin
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo = id, @idBill = IDBill from deleted

	declare @idTable int
	select @idTable = IDTable from Bill where id = @idBill

	declare @count int = 0

	select @count = count(*) from BillInfo as bi, Bill as b where b.ID = bi.IDBill and b.ID = @idBill and b.Status = 0

	if (@count = 0)
		update TableFood set status =N'Trống' where id = @idTable
end
go

CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END

select * from Food where dbo.fuConvertToUnsign1(Name) like N'%' + dbo.fuConvertToUnsign1(N'mưc') + N'%'

select * from Account
alter table account
add constraint DF_PS
default N'0' for PassWord

alter proc USP_GetListBillByDateAndPage
@checkIn date, @checkOut date, @page int
as
begin
	declare @pageRows int = 10
	declare @selectRows int = @pageRows * @page
	declare @exceptRows int = @pageRows * (@page - 1)


	;with BillShow as (select b.ID, t.Name as N'Tên bàn', b.totalPrice as N'Tổng tiền', b.DateCheckIn as N'Ngày vào', b.DateCheckOut as N'Ngày ra', b.Discount as N'Giảm giá'
	from Bill as b, TableFood as t
	where b.DateCheckIn >= @checkIn and b.DateCheckOut <= @checkOut and b.status = 1 and b.IDTable = t.ID)

	select top (@pageRows) * from BillShow where ID not in ( select top (@exceptRows) ID from BillShow )
	
end	
go

create proc USP_GetNumBillByDate
@checkIn date, @checkOut date
as
begin
	select count(*)
	from Bill as b, TableFood as t
	where b.DateCheckIn >= @checkIn and b.DateCheckOut <= @checkOut and b.status = 1 and b.IDTable = t.ID
end
go

exec USP_GetListBillByDateAndPage @checkIn ='20230628' , @checkOut='20230628' , @page = 1

select * from FoodCategory

insert into FoodCategory values (N'Trà')
update FoodCategory set Name = N'' where id =
delete from food where IDCategory = 12

select * from TableFood

insert into TableFood(Name) values (N'VoDoi')

delete from BillInfo where IDBill in (select ID from Bill where IDTable = 33)

select * from Bill

update TableFood set name = 'Celica' where ID = 1

exec USP_GetListBillByDateAndPage @checkIn='20230628' , @checkOut='20230628' , @page=4

select * from Account

create proc USP_UpdateAccount
@userName nvarchar(100), @displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0

	select @isRightPass = count(*) from Account where UserName = @userName and PassWord = @password

	if (@isRightPass = 1)
	begin
		if (@newPassword = null or @newPassword = ' ')
		begin
			update Account set DisplayName = @displayName where UserName = @userName
		end
		else
			update Account set DisplayName = @displayName, PassWord = @newPassword where UserName = @userName

	end
end
go
select * from Food

INSERT INTO Food (Name, IDCategory, Price) VALUES (N'Mực một nắng nướng sa tế', 1, 120000);

update food set Name = N'', IDCategory = 5, Price = 0 where ID = 4

create proc USP_UpdateAccountPassWord
@userName nvarchar(100), @displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0

	select @isRightPass = count(*) from Account where UserName = @userName and PassWord = @password

	if (@isRightPass = 1)
	begin
		if (@newPassword = null or @newPassword = ' ')
		begin
			update Account set DisplayName = @displayName where UserName = @userName
		end
		else
			update Account set DisplayName = @displayName, PassWord = @newPassword where UserName = @userName

	end
end
go