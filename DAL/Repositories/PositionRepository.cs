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
    public class PositionRepository : IPositionRepository
    {
        /// <summary>
        /// Method uses SQL Queries
        /// </summary>
        public async Task<IEnumerable<Position>> GetAll()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                string sqlCommand = "select * from Positions";
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    await command.ExecuteNonQueryAsync();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                }
                await connection.CloseAsync();
            }

            List<Position> list = new List<Position>();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Position position = new Position()
                    {
                        Id = Int32.Parse(row["Id"].ToString()),
                        PositionName = row["PositionName"].ToString()
                    };
                    list.Add(position);
                }
            }
            return list;
        }
        public async Task<Position> GetById(int id)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                string sqlCommand = $"select * from Positions where Positions.Id = {id}";
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    await command.ExecuteNonQueryAsync();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                }
                await connection.CloseAsync();
            }

            Position result = new Position()
            {
                Id = Int32.Parse(dataTable.Rows[0]["Id"].ToString()),
                PositionName = dataTable.Rows[0]["PositionName"].ToString()
            };
            return result;
        }

        public async Task AddPosition(Position position)
        {
            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                string sqlCommand = $"insert into Positions (PositionName) values ('{position.PositionName}')";
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        public async Task UpdatePosition(int id, Position position)
        {
            if (id != position.Id)
            {
                throw new Exception("Id != Position Id");
            }
            try
            {
                var check = await GetById(id);
            }
            catch (Exception)
            {
                throw new Exception("No Position with this Id!");
            }
            using (SqlConnection connection = new SqlConnection(StaticItems.StaticItems.ConnectionString))
            {
                await connection.OpenAsync();
                string sqlCommand = $"update Positions set PositionName = @NewPositionName where Positions.Id = @PositionId";
                SqlCommand command = new SqlCommand(sqlCommand, connection);
                command.Parameters.AddWithValue("@NewPositionName", position.PositionName);
                command.Parameters.AddWithValue("@PositionId", position.Id);
                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }
        public async Task DeletePosition(int id)
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
                string sqlCommand = $"delete from Positions where Positions.Id = {id}";
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
    }
}
