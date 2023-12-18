using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IPositionService
    {
        Task<IEnumerable<Position>> GetAll();
        Task<Position> GetById(int id);

        Task AddPosition(Position position);
        Task UpdatePosition(int id, Position position);
        Task DeletePosition(int id);
    }
}
