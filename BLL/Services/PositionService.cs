using BLL.Contracts;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _repository;
        public PositionService(IPositionRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<Position>> GetAll()
        {
            var result = await _repository.GetAll();
            return result;
        }
        public async Task<Position> GetById(int id)
        {
            var result = await _repository.GetById(id);
            return result;
        }

        public async Task AddPosition(Position position)
        {
            await _repository.AddPosition(position);
        }
        public async Task UpdatePosition(int id, Position position)
        {
            await _repository.UpdatePosition(id, position);
        }
        public async Task DeletePosition(int id)
        {
            await _repository.DeletePosition(id);
        }
    }
}
