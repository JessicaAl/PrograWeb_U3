using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Act2_ZooPlanet.Repositories
{
    public class ClasesRepository: Repository<Clase>
    {
    public ClasesRepository(animalesContext context) : base(context)
    {

    }
    public override IEnumerable<Clase> GetAll()
    {
        return Context.Clase.OrderBy(x => x.Nombre);
    }

    
   }
}
