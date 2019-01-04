
CREATE DATABASE QuanLyTaiKhoan
GO

USE QuanLyTaiKhoan
GO



CREATE TABLE Account
(
	
	UserName NVARCHAR(100) PRIMARY KEY,	
	--DisplayName NVARCHAR(100) NOT NULL DEFAULT N'No Name',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	--Type INT NOT NULL  DEFAULT 0 -- 1: admin && 0: staff
	Level int not null default 1
)
GO




CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
END
GO
