using FruitStore.Models;
using FruitStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FruitStore.Services
{
    public class CategoriaService
    {
        public List<Categorias> Categorias { get; set; }

        public CategoriaService()
        {
          
            using (fruteriashopContext context = new fruteriashopContext())
            {
                Repository<Categorias> repos = new Repository<Categorias>(context);
                Categorias = repos.GetAll().Where(x => x.Eliminado == 0).OrderBy(x => x.Nombre).ToList();
            }
        }
    }
}
