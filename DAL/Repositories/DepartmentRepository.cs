using DAL.Interfaces;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        /// <summary>
        /// Method uses Stored Procedures
        /// To change behavior create new SP
        /// </summary>
        public async Task<IEnumerable<Department>> GetAll()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("exec dbo.Get_Departments", connection))
                {
                    await command.ExecuteNonQueryAsync();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                }
                await connection.CloseAsync();
            }

            List<Department> list = new List<Department>();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Department department = new Department()
                    {
                        Id = Int32.Parse(row["Id"].ToString()),
                        DepartmentName = row["DepartmentName"].ToString()
                    };
                    list.Add(department);
                }
            }
            return list;
        }
        public async Task<Department> GetById(int id)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand($"exec dbo.Get_Department_By_Id '"+id+"'", connection))
                {
                    await command.ExecuteNonQueryAsync();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                }
                await connection.CloseAsync();
            }

            Department result = new Department()
            {
                Id = Int32.Parse(dataTable.Rows[0]["Id"].ToString()),
                DepartmentName = dataTable.Rows[0]["DepartmentName"].ToString()
            };
            return result;
        }

        public async Task AddDepartment(Department department)
        {
            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand($"exec dbo.Add_Department '" + department.DepartmentName + "'", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        public async Task UpdateDepartment(int id, Department department)
        {
            if(id != department.Id)
            {
                throw new Exception("Id != Department Id");
            }
            try
            {
                var check = await GetById(id);
            }
            catch (Exception)
            {
                throw new Exception("No Department with this Id!");
            }
            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand($"exec dbo.Update_Department '" + id + "','" + department.DepartmentName + "'", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        public async Task DeleteDepartment(int id)
        {
            try
            {
                var check = await GetById(id);
            }
            catch (Exception)
            {
                throw new Exception("Wrong Id!");
            }
            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand($"exec dbo.Delete_Department '" + id + "'", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
    }
}
