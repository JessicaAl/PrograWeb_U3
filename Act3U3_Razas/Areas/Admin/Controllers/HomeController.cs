using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Act3U3_Razas.Models;
using Act3U3_Razas.Models.ViewModels;
using Act3U3_Razas.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Act3U3_Razas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        sistem14_razasContext context;
        public IWebHostEnvironment Enviroment { get; set; }

        public HomeController(sistem14_razasContext ctx, IWebHostEnvironment env)
        {
            context = ctx;
            Enviroment = env;
        }

        public IActionResult Index()
        {
            Repository<Razas> repos = new Repository<Razas>(context);
            return View(repos.GetAll().Where(x => x.Eliminado == 0).OrderBy(x => x.Nombre));
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            Repository<Paises> repos = new Repository<Paises>(context);
            InfoPerroViewModel vm = new InfoPerroViewModel();
            vm.Paises = repos.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(InfoPerroViewModel vm)
        {
            try
            {
                if (vm.Archivo == null)
                {
                    ModelState.AddModelError("", "Seleccione una imagen para la raza.");

                    return View(vm);
                }

                else
                {
                    if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un archivo jpg de menos de 2MB.");

                        return View(vm);
                    }

                }

                Repository<Razas> repos = new Repository<Razas>(context);
                repos.Insert(vm.Raza);
                FileStream fs = new FileStream(Enviroment.WebRootPath + "/imgs_perros/" + vm.Raza.Id + "_0.jpg", FileMode.Create);
                vm.Archivo.CopyTo(fs);
                fs.Close();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }


        }

        [HttpGet]
        public IActionResult Editar(uint id)
        {
            Repository<Razas> reposRazas = new Repository<Razas>(context);
            Repository<Paises> repos = new Repository<Paises>(context);
            Repository<Caracteristicasfisicas> reposCaracteristicas = new Repository<Caracteristicasfisicas>(context);
            Repository<Estadisticasraza> reposEstadisticas = new Repository<Estadisticasraza>(context);
            InfoPerroViewModel vm = new InfoPerroViewModel();
            vm.Raza = reposRazas.GetById(id);
            vm.Paises = repos.GetAll();
            vm.Raza.Caracteristicasfisicas = reposCaracteristicas.GetById(id);
            vm.Raza.Estadisticasraza = reposEstadisticas.GetById(id);


            if (vm.Raza == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (System.IO.File.Exists(Enviroment.WebRootPath + "/imgs_perros/" + vm.Raza.Id + "_0.jpg"))
            {
                vm.Imagen = vm.Raza.Id + "_0.jpg";
            }
            else
            {
                vm.Imagen = "NoPhoto.jpg";
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(InfoPerroViewModel infovm)
        {
            try
            {
                if (infovm.Archivo != null)
                {
                    if (infovm.Archivo.ContentType != "image/jpeg" || infovm.Archivo.Length > 1024 * 1024 * 2)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un archivo jpg de menos de 2MB.");
                        Repository<Paises> repospaises = new Repository<Paises>(context);
                        infovm.Paises = repospaises.GetAll();
                        return View(infovm);
                    }
                }

                Repository<Razas> repos = new Repository<Razas>(context);
                var raz = repos.GetById(infovm.Raza.Id);
                Repository<Caracteristicasfisicas> reposCaracteristicas = new Repository<Caracteristicasfisicas>(context);
                var c = reposCaracteristicas.GetById(infovm.Raza.Caracteristicasfisicas.Id);
                Repository<Estadisticasraza> reposEstadisticas = new Repository<Estadisticasraza>(context);
                var est = reposEstadisticas.GetById(infovm.Raza.Estadisticasraza.Id);

                if (raz != null && c != null && est != null)
                {
                    raz.Nombre = infovm.Raza.Nombre;
                    raz.IdPais = infovm.Raza.IdPais;
                    raz.OtrosNombres = infovm.Raza.OtrosNombres;
                    raz.PesoMin = infovm.Raza.PesoMin;
                    raz.PesoMax = infovm.Raza.PesoMax;
                    raz.AlturaMin = infovm.Raza.AlturaMin;
                    raz.AlturaMax = infovm.Raza.AlturaMax;
                    raz.EsperanzaVida = infovm.Raza.EsperanzaVida;
                    raz.Descripcion = infovm.Raza.Descripcion;

                    c.Patas = infovm.Raza.Caracteristicasfisicas.Patas;
                    c.Cola = infovm.Raza.Caracteristicasfisicas.Cola;
                    c.Hocico = infovm.Raza.Caracteristicasfisicas.Hocico;
                    c.Pelo = infovm.Raza.Caracteristicasfisicas.Pelo;
                    c.Color = infovm.Raza.Caracteristicasfisicas.Color;

                    est.NivelEnergia = infovm.Raza.Estadisticasraza.NivelEnergia;
                    est.FacilidadEntrenamiento = infovm.Raza.Estadisticasraza.FacilidadEntrenamiento;
                    est.EjercicioObligatorio = infovm.Raza.Estadisticasraza.EjercicioObligatorio;
                    est.AmistadDesconocidos = infovm.Raza.Estadisticasraza.AmistadDesconocidos;
                    est.AmistadPerros = infovm.Raza.Estadisticasraza.AmistadPerros;
                    est.NecesidadCepillado = infovm.Raza.Estadisticasraza.NecesidadCepillado;

                    repos.Update(raz);
                    reposCaracteristicas.Update(c);
                    reposEstadisticas.Update(est);

                    if (infovm.Archivo != null)
                    {
                        FileStream fs = new FileStream(Enviroment.WebRootPath + "/imgs_perros/" + infovm.Raza.Id + "_0.jpg", FileMode.Create);
                        infovm.Archivo.CopyTo(fs);
                        fs.Close();
                    }
                }
                return RedirectToAction("Index", "Home");


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                Repository<Paises> repos = new Repository<Paises>(context);

                infovm.Paises = repos.GetAll();

                return View(infovm);
            }

        }


        [HttpPost]
        public IActionResult Eliminar(Razas r)
        {

            try
            {
                using (sistem14_razasContext context = new sistem14_razasContext())
                {
                    Repository<Razas> repos = new Repository<Razas>(context);
                    var razas = repos.GetById(r.Id);
                    razas.Eliminado = 1;
                    repos.Update(razas);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(r);
            }
        }
    }
}
