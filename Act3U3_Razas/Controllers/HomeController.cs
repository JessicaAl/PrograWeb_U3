using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Act3U3_Razas.Models;
using Act3U3_Razas.Models.ViewModels;
using Act3U3_Razas.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Act3U3_Razas.Controllers
{
    public class HomeController : Controller
    {
		public IActionResult Index(string id)
		{
			using sistem14_razasContext context = new sistem14_razasContext();
			RazasRepository repos = new RazasRepository(context);
			Repository<Razas> reporaza = new Repository<Razas>(context);
			IndexViewModel vm = new IndexViewModel
			{
				Razas = id == null ? repos.GetRazas() : repos.GetRazasByLetraInicial(id),
				LetrasIniciales = repos.GetLetrasIniciales()
			};
		
			return View(vm);
		}
		[Route("Raza/{id}")]
		public IActionResult InfoPerro(string id)
		{
			sistem14_razasContext context = new sistem14_razasContext();
			RazasRepository razarepos = new RazasRepository(context);
			InfoPerroViewModel infovm = new InfoPerroViewModel();
			infovm.Raza = razarepos.GetRazaByNombre(id);

			if (infovm.Raza == null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				infovm.OtrasRazas = razarepos.Get4RandomRazasExcept(id);
				return View(infovm);
			}
		}

		public IActionResult RazaPorPais()
		{
			sistem14_razasContext context = new sistem14_razasContext();
			RazasRepository rr = new RazasRepository(context);
			RazaPorPaisViewModel rvm = new RazaPorPaisViewModel();
			rvm.Paises = rr.GetRazasByPais();
			return View(rvm);
		}
	}
}
