
-- Створення stored procedure

USE EmployeeManagement;
GO
CREATE PROCEDURE Get_Employees
AS
BEGIN
    SELECT E.*
    FROM Employees E
END;
GO

CREATE PROCEDURE Get_Departments
AS
BEGIN
    SELECT D.*
    FROM Departments D
END;
GO

CREATE PROCEDURE Get_Department_By_Id
	@DepartmentId int
AS
BEGIN
    SELECT D.*
    FROM Departments D
	Where D.Id = @DepartmentId
END;
GO

CREATE PROCEDURE Add_Department
	@DepartmentName nvarchar(50)
AS
BEGIN
	INSERT INTO Departments(DepartmentName)
	VALUES (@DepartmentName)
END
GO

CREATE PROCEDURE Update_Department
	@DepartmentId int,
	@DepartmentName nvarchar(50)
AS
BEGIN
	UPDATE Departments set DepartmentName = @DepartmentName 
	where Departments.Id = @DepartmentId
END
GO

CREATE PROCEDURE Delete_Department
	@DepartmentId int
AS
BEGIN
	DELETE FROM Departments
	WHERE Departments.Id = @DepartmentId
END
GO

update Positions set PositionName = 'new_test' where Positions.Id = 4