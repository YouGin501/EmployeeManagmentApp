USE master;
GO
-- Створення бази даних
CREATE DATABASE EmployeeManagement;
GO
USE EmployeeManagement;
GO
CREATE TABLE Departments
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(50) NOT NULL
);
GO
CREATE TABLE Positions
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PositionName NVARCHAR(50) NOT NULL
);
GO
CREATE TABLE Employees
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(30) NOT NULL,
    MiddleName NVARCHAR(30),
    LastName NVARCHAR(30) NOT NULL,
    [Address] NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    BirthDate DATE NOT NULL,
    HireDate DATE NOT NULL,
    Salary DECIMAL(10, 2) NOT NULL,
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id),
    PositionId INT NOT NULL FOREIGN KEY REFERENCES Positions(Id),
    CompanyInfo NVARCHAR(MAX)
);
GO

-- Внесення даних
INSERT INTO Departments (DepartmentName)
VALUES
    ('IT'),
    ('HR'),
    ('Finance');
go
INSERT INTO Positions (PositionName)
VALUES
    ('Software Engineer'),
    ('HR Manager'),
    ('Financial Analyst');
go
INSERT INTO Employees (DepartmentId, PositionId, FirstName, MiddleName, LastName, Address, PhoneNumber, BirthDate, HireDate, Salary, CompanyInfo)
VALUES
    (1, 1, 'John','Johnes', 'Doe', '123 Main St', '555-1234', '1990-01-01', '2020-01-15', 75000.00, 'ABC Inc.'),
    (2, 2, 'Jane','Janes', 'Smith', '456 Oak St', '555-5678', '1985-05-15', '2019-05-10', 60000.00, 'XYZ Corp.'),
    (1, 3, 'Bob','', 'Johnes', '789 Pine St', '555-8765', '1992-08-20', '2021-02-28', 80000.00, '123 Company'),
	(3, 1, 'Alice','Jayson', 'Johnson', '111 Elm St', '555-1111', '1988-03-10', '2022-03-01', 90000.00, 'ABC Inc.'),
    (2, 3, 'Tom','Tomason', 'Williams', '222 Maple St', '555-2222', '1995-07-25', '2021-09-15', 70000.00, 'XYZ Corp.'),
    (1, 2, 'Emily', 'May', 'Davis', '333 Birch St', '555-3333', '1980-11-05', '2020-05-20', 85000.00, '123 Company'),
    (3, 1, 'Michael','Mikeson', 'Brown', '444 Cedar St', '555-4444', '1993-12-15', '2022-01-10', 95000.00, 'ABC Inc.');


-- for debug
-- DO NOT USE
--go
--DELETE FROM Employees;
--DBCC CHECKIDENT ('Employees', RESEED, 0);
--go
--DELETE FROM Departments
--DBCC CHECKIDENT ('Departments', RESEED, 0);
--go
--DELETE FROM Positions
--DBCC CHECKIDENT ('Positions', RESEED, 0);
