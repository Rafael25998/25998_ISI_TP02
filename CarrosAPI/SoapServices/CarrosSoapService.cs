using CarrosAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarrosAPI.SoapServices
{
    public class CarrosSoapService : ICarrosSoapService
    {
        private readonly CarrosDbContext _context;

        public CarrosSoapService(CarrosDbContext context)
        {
            _context = context;
        }

        public List<ModeloCarro> GetModelosPorMarca(string marca)
        {
            // Filtra os modelos pela marca
            return _context.ModelosCarros.Where(m => m.Marca == marca).ToList();
        }
    }
}
