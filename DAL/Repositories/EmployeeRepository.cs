using DAL.Interfaces;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<IEnumerable<Employee>> GetAll()
        {
            List<Employee> list = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                string sqlCommand = 
                    @"SELECT
                    E.Id AS EmployeeId,
                    E.FirstName,
                    E.MiddleName,
                    E.LastName,
                    E.[Address],
                    E.PhoneNumber,
                    E.BirthDate,
                    E.HireDate,
                    E.Salary,
                    E.CompanyInfo,
                    E.DepartmentId,
                    E.PositionId,
                    D.DepartmentName,
                    P.PositionName
                FROM
                    Employees E
                    INNER JOIN Departments D ON E.DepartmentId = D.Id
                    INNER JOIN Positions P ON E.PositionId = P.Id;";

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    await command.ExecuteNonQueryAsync();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee()
                            {
                                Id = Int32.Parse(reader["EmployeeId"].ToString()),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"]==null? " " : reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Address = reader["Address"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                BirthDate = reader.GetDateTime("BirthDate"),
                                HireDate = reader.GetDateTime("HireDate"),              
                                Salary = Decimal.Parse(reader["Salary"].ToString()),
                                CompanyInfo = reader["CompanyInfo"].ToString(),
                                DepartmentId = Int32.Parse(reader["DepartmentId"].ToString()),
                                PositionId = Int32.Parse(reader["PositionId"].ToString()),
                                Department = new Department()
                                {
                                    Id = Int32.Parse(reader["DepartmentId"].ToString()),
                                    DepartmentName = reader["DepartmentName"].ToString()
                                },
                                Position = new Position()
                                {
                                    Id = Int32.Parse(reader["PositionId"].ToString()),
                                    PositionName = reader["PositionName"].ToString()
                                }
                            };

                            list.Add(employee);
                        }
                    }

                }
                await connection.CloseAsync();
            }
            return list;
        }
        public async Task<Employee> GetById(int id)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                string sqlCommand =
                    @$"SELECT
                    E.Id,
                    E.FirstName,
                    E.MiddleName,
                    E.LastName,
                    E.[Address],
                    E.PhoneNumber,
                    E.BirthDate,
                    E.HireDate,
                    E.Salary,
                    E.CompanyInfo,
                    E.DepartmentId,
                    E.PositionId,
                    D.DepartmentName,
                    P.PositionName
                FROM
                    Employees E
                    INNER JOIN Departments D ON E.DepartmentId = D.Id
                    INNER JOIN Positions P ON E.PositionId = P.Id
                    WHERE E.Id = {id};";

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    await command.ExecuteNonQueryAsync();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                }
                await connection.CloseAsync();

                Employee result = new Employee()
                {
                    Id = Int32.Parse(dataTable.Rows[0]["Id"].ToString()),
                    FirstName = dataTable.Rows[0]["FirstName"].ToString(),
                    MiddleName = dataTable.Rows[0]["MiddleName"] == null ? " " : dataTable.Rows[0]["MiddleName"].ToString(),
                    LastName = dataTable.Rows[0]["LastName"].ToString(),
                    Address = dataTable.Rows[0]["Address"].ToString(),
                    PhoneNumber = dataTable.Rows[0]["PhoneNumber"].ToString(),
                    BirthDate = (DateTime)dataTable.Rows[0]["BirthDate"],
                    HireDate = (DateTime)dataTable.Rows[0]["HireDate"],
                    Salary = Decimal.Parse(dataTable.Rows[0]["Salary"].ToString()),
                    CompanyInfo = dataTable.Rows[0]["CompanyInfo"].ToString(),
                    DepartmentId = Int32.Parse(dataTable.Rows[0]["DepartmentId"].ToString()),
                    PositionId = Int32.Parse(dataTable.Rows[0]["PositionId"].ToString()),
                    Department = new Department()
                    {
                        Id = Int32.Parse(dataTable.Rows[0]["DepartmentId"].ToString()),
                        DepartmentName = dataTable.Rows[0]["DepartmentName"].ToString()
                    },
                    Position = new Position()
                    {
                        Id = Int32.Parse(dataTable.Rows[0]["PositionId"].ToString()),
                        PositionName = dataTable.Rows[0]["PositionName"].ToString()
                    }
                };
                return result;
            }
        }

        public async Task AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                string sqlCommand = $"insert into Employees " +
                    $"(DepartmentId, PositionId, FirstName, MiddleName, LastName, Address, PhoneNumber, BirthDate, HireDate, Salary, CompanyInfo) " +
                    $"VALUES(@DepartmentId, @PositionId, @FirstName, @MiddleName, @LastName, @Address, @PhoneNumber, @BirthDate, @HireDate, @Salary, @CompanyInfo)";
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
                    command.Parameters.AddWithValue("@PositionId", employee.PositionId);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", employee.MiddleName == null ? " " : employee.MiddleName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Address", employee.Address);
                    command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                    command.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                    command.Parameters.AddWithValue("@HireDate", employee.HireDate);
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@CompanyInfo", employee.CompanyInfo);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        public async Task UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                throw new Exception("Id != Employee Id");
            }
            try
            {
                var check = await GetById(id);
            }
            catch (Exception)
            {
                throw new Exception("No Employee with this Id!");
            }
            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();

                string sqlCommand = $"update Employees set " +
                    $"DepartmentId=@DepartmentId, PositionId=@PositionId, FirstName=@FirstName, MiddleName=@MiddleName, LastName=@LastName, Address=@Address, " +
                    $"PhoneNumber=@PhoneNumber, BirthDate=@BirthDate, HireDate=@HireDate, Salary=@Salary, CompanyInfo=@CompanyInfo " +
                    $"where Employees.Id = {id}";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
                    command.Parameters.AddWithValue("@PositionId", employee.PositionId);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", employee.MiddleName == null ? " " : employee.MiddleName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Address", employee.Address);
                    command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                    command.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                    command.Parameters.AddWithValue("@HireDate", employee.HireDate);
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@CompanyInfo", employee.CompanyInfo);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        public async Task DeleteEmployee(int id)
        {
            try
            {
                var check = await GetById(id);
            }
            catch (Exception)
            {
                throw new Exception("No Employee with this Id!");
            }
            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                string sqlCommand = $"delete from Employees where Employees.Id = {id}";
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GeneralSalaryInfo(DateTime startPeriod, DateTime endPeriod, int? departmentId, int? positionId)
        {
            List<Employee> list = new List<Employee>();

            if ((endPeriod - startPeriod).Days < 0)
            {
                throw new Exception("Wrong start or end period");
            }

            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                string depCheck = $"D.Id = {departmentId}";
                string posCheck = $"P.Id = {positionId}";

                string sqlCommand =
                    @$"SELECT
                    E.Id,
                    E.FirstName,
                    E.MiddleName,
                    E.LastName,
                    E.[Address],
                    E.PhoneNumber,
                    E.BirthDate,
                    E.HireDate,
                    E.Salary,
                    E.CompanyInfo,
                    E.DepartmentId,
                    E.PositionId,
                    D.DepartmentName,
                    P.PositionName
                FROM
                    Employees E 
                    INNER JOIN Departments D ON E.DepartmentId = D.Id
                    INNER JOIN Positions P ON E.PositionId = P.Id
                    where E.HireDate < '{endPeriod}'" + (departmentId.Value>0 ? " AND " + depCheck : " ") +
                    (positionId.Value>0 ? " AND " + posCheck : " ");
                    

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    await command.ExecuteNonQueryAsync();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee()
                            {
                                Id = Int32.Parse(reader["Id"].ToString()),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"] == null ? " " : reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Address = reader["Address"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                BirthDate = reader.GetDateTime("BirthDate"),
                                HireDate = reader.GetDateTime("HireDate"),
                                Salary = Decimal.Parse(reader["Salary"].ToString()) / 30,
                                CompanyInfo = reader["CompanyInfo"].ToString(),
                                DepartmentId = Int32.Parse(reader["DepartmentId"].ToString()),
                                PositionId = Int32.Parse(reader["PositionId"].ToString()),
                                Department = new Department()
                                {
                                    Id = Int32.Parse(reader["DepartmentId"].ToString()),
                                    DepartmentName = reader["DepartmentName"].ToString()
                                },
                                Position = new Position()
                                {
                                    Id = Int32.Parse(reader["PositionId"].ToString()),
                                    PositionName = reader["PositionName"].ToString()
                                }
                            };

                            // Checks how many days did the employee work
                            int workingDays = 0;
                            if(employee.HireDate <= startPeriod)
                            {
                                workingDays = (endPeriod - startPeriod).Days;
                            } else
                            {
                                workingDays = (endPeriod - employee.HireDate).Days;
                            }
                            employee.Salary *= workingDays;

                            list.Add(employee);
                        }
                    }
                }
                await connection.CloseAsync();

            }
            return list;
        }
    }
}
