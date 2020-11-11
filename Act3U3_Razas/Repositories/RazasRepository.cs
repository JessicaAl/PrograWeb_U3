using Act3U3_Razas.Models;
using Act3U3_Razas.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Act3U3_Razas.Repositories
{
    public class RazasRepository : Repository<Razas>
    {
		public RazasRepository(sistem14_razasContext ctx) : base(ctx)
		{

		}

        sistem14_razasContext context = new sistem14_razasContext();

		public IEnumerable<RazaViewModel> GetRazas()
		{
			return context.Razas.Where(x => x.Eliminado == 0).OrderBy(x => x.Nombre)
				.Select(x => new RazaViewModel
				{
					Id = x.Id,
					Nombre = x.Nombre
				});
		}

		public IEnumerable<RazaViewModel> GetRazasByLetraInicial(string letra)
		{
			return GetRazas().Where(x => x.Nombre.StartsWith(letra));
		}


		public IEnumerable<char> GetLetrasIniciales()
		{
			return context.Razas.OrderBy(x => x.Nombre).Select(x => x.Nombre.First());
		}

		public Razas GetRazaByNombre(string nombre)
		{
			nombre = nombre.Replace("-", " ");
			return context.Razas.Include(x => x.Estadisticasraza)
				.Include(x => x.Caracteristicasfisicas)
				.Include(x => x.IdPaisNavigation)
				.FirstOrDefault(x => x.Nombre == nombre);
		}

		public IEnumerable<RazaViewModel> Get4RandomRazasExcept(string nombre)
		{
			nombre = nombre.Replace("-", " ");
			Random r = new Random();
			var al = r.Next();
			return context.Razas.Where(x => x.Nombre != nombre).OrderBy(x => al).Take(4).Select(x => new RazaViewModel { Id = x.Id, Nombre = x.Nombre });
		}

		public Razas GetRazaById(int id)
		{
			return context.Razas.Where(x => x.Eliminado == 0).Include(x => x.Estadisticasraza).Include(x => x.IdPaisNavigation).Include(x => x.Caracteristicasfisicas).FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Paises> GetPaises()
		{
			return context.Paises.OrderBy(x => x.Nombre);
		}

		public IEnumerable<Paises> GetRazasByPais()
		{
			return context.Paises.Include(x => x.Razas).OrderBy(x => x.Nombre);
		}

		public override bool Validate(Razas valor)
		{
			if (string.IsNullOrEmpty(valor.Nombre))
			{
				throw new Exception("No ingresó el nombre de la raza.");
			}

			if (string.IsNullOrEmpty(valor.IdPaisNavigation.Nombre))
			{
				throw new Exception("No ingresó el país de la raza.");
			}

			if (valor.PesoMin <= 0 || valor == null)
			{
				throw new Exception("No ingresó el peso mínimo de la raza.");
			}

			if (valor.PesoMax <= 0 || valor == null)
			{
				throw new Exception("No ingresó el peso máximo de la raza.");
			}

			if (valor.AlturaMin <= 0 || valor == null)
			{
				throw new Exception("No ingresó la altura mínima de la raza.");
			}

			if (valor.AlturaMax <= 0 || valor == null)
			{
				throw new Exception("No ingresó la altura máxima de la raza.");
			}

			if (valor.EsperanzaVida <= 0 || valor == null)
			{
				throw new Exception("No ingresó la esperanza de vida de la raza.");
			}

			if (string.IsNullOrEmpty(valor.Descripcion))
			{
				throw new Exception("No ingresó una descripción de la raza");
			}

			if (string.IsNullOrEmpty(valor.OtrosNombres))
			{
				throw new Exception("No ingresó otros nombres de la raza");
			}

			if (string.IsNullOrEmpty(valor.Caracteristicasfisicas.Patas))
			{
				throw new Exception("No ingresó las características de las patas de la raza.");
			}

			if (string.IsNullOrEmpty(valor.Caracteristicasfisicas.Cola))
			{
				throw new Exception("No ingresó las características de la cola de la raza.");
			}

			if (string.IsNullOrEmpty(valor.Caracteristicasfisicas.Hocico))
			{
				throw new Exception("No ingresó las características del hocico de la raza.");
			}

			if (string.IsNullOrEmpty(valor.Caracteristicasfisicas.Pelo))
			{
				throw new Exception("No ingresó las características del pelo de la raza.");
			}

			if (string.IsNullOrEmpty(valor.Caracteristicasfisicas.Color))
			{
				throw new Exception("No ingresó las características del color de la raza.");
			}

			if (valor.Estadisticasraza.NivelEnergia < 0 || valor.Estadisticasraza.NivelEnergia > 10)
			{
				throw new Exception("Ingrese el nivel de energía entre los valores 0 y 10.");
			}

			if (valor.Estadisticasraza.FacilidadEntrenamiento < 0 || valor.Estadisticasraza.FacilidadEntrenamiento > 10)
			{
				throw new Exception("Ingrese la facilidad en entrenamiento entre los valores 0 y 10.");
			}

			if (valor.Estadisticasraza.AmistadDesconocidos < 0 || valor.Estadisticasraza.AmistadDesconocidos > 10)
			{
				throw new Exception("Ingrese la amistad con desconocidos entre los valores 0 y 10.");
			}

			if (valor.Estadisticasraza.AmistadPerros < 0 || valor.Estadisticasraza.AmistadPerros > 10)
			{
				throw new Exception("Ingrese la amistad con perros entre los valores 0 y 10.");
			}

			if (valor.Estadisticasraza.NecesidadCepillado < 0 || valor.Estadisticasraza.NecesidadCepillado > 10)
			{
				throw new Exception("Ingrese la necesidad de cepillado entre los valores 0 y 10.");
			}

			return true;
		}
	}
}
