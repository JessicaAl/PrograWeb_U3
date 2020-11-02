using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Act2_ZooPlanet.Models.ViewModels;
using Act2_ZooPlanet.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Act2_ZooPlanet.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public class HomeController : Controller
        {
            animalesContext context;

            public IWebHostEnvironment Enviroment { get; set; }

            public HomeController(animalesContext ctx, IWebHostEnvironment env)
            {
                context = ctx;
                Enviroment = env;
            }
            public IActionResult Index()
            {
                EspeciesRepository repos = new EspeciesRepository(context);
                return View(repos.GetAll().OrderBy(x => x.Especie));
            }

            [HttpGet]
            public IActionResult Agregar()
            {
                EspeciesViewModel vm = new EspeciesViewModel();
                ClasesRepository clasesRepository = new ClasesRepository(context);
                vm.Clases = clasesRepository.GetAll();
                return View(vm);
            }

            [HttpPost]
            public IActionResult Agregar(EspeciesViewModel vm)
            {
                try
                {
                    EspeciesRepository repos = new EspeciesRepository(context);
                    repos.Insert(vm.Especie);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ClasesRepository clasesRepository = new ClasesRepository(context);
                    vm.Clases = clasesRepository.GetAll();
                    return View(vm);
                }
            }


            [HttpGet]
            public IActionResult Editar(int id)
            {
                EspeciesViewModel vm = new EspeciesViewModel();
                EspeciesRepository repos = new EspeciesRepository(context);
                ClasesRepository claserepo = new ClasesRepository(context);
                vm.Especie = repos.GetById(id);
                vm.Clases = claserepo.GetAll();
                if (vm.Especie == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                    return View(vm);
            }

            [HttpPost]
            public IActionResult Editar(EspeciesViewModel vm)
            {
                try
                {
                    ClasesRepository claserepo = new ClasesRepository(context);
                    EspeciesRepository repos = new EspeciesRepository(context);
                    var esp = repos.GetById(vm.Especie.Id);
                    vm.Clases = claserepo.GetAll();
                    if (vm.Especie != null)
                    {
                        esp.Especie = vm.Especie.Especie;
                        esp.Habitat = vm.Especie.Habitat;
                        esp.Observaciones = vm.Especie.Observaciones;
                        esp.Tamaño = vm.Especie.Tamaño;
                        esp.Peso = vm.Especie.Peso;
                        esp.IdClase = vm.Especie.IdClase;
                        repos.Update(esp);
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ClasesRepository clasesRepository = new ClasesRepository(context);
                    vm.Clases = clasesRepository.GetAll();
                    return View(vm);
                }
            }

            [HttpGet]
            public IActionResult Eliminar(int id)
            {
                EspeciesRepository repos = new EspeciesRepository(context);
                var esp = repos.GetById(id);

                if (esp != null)
                {
                    return View(esp);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            [HttpPost]
            public IActionResult Eliminar(Especies e)
            {
                EspeciesRepository repos = new EspeciesRepository(context);
                var esp = repos.GetById(e.Id);

                if (esp != null)
                {
                    repos.Delete(esp);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "La especie no existe o ya ha sido eliminada.");
                    return View();
                }
            }

            [HttpGet]
            public IActionResult Imagen(int id)
            {
                EspeciesRepository repos = new EspeciesRepository(context);
                EspeciesViewModel vm = new EspeciesViewModel();
                vm.Especie = repos.GetById(id);
                if (System.IO.File.Exists(Enviroment.WebRootPath + $"/especies/{vm.Especie.Id}.jpg"))
                {
                    vm.Imagen = vm.Especie.Id + ".jpg";
                }
                else
                {
                    vm.Imagen = "nophoto.jpg";
                }
                return View(vm);
            }

            [HttpPost]
            public IActionResult Imagen(EspeciesViewModel vm)
            {
                try
                {
                    if (vm.Archivo == null)
                    {
                        ModelState.AddModelError("", "Seleccione la imagen de la especie.");
                        return View(vm);
                    }
                    else
                    {
                        if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                        {
                            ModelState.AddModelError("", "Seleccione un archivo jpg de menos de 2MB.");
                            return View(vm);
                        }
                    }
                    if (vm.Archivo != null)
                    {
                        FileStream fs = new FileStream(Enviroment.WebRootPath + "/especies/" + vm.Especie.Id + ".jpg", FileMode.Create);
                        vm.Archivo.CopyTo(fs);
                        fs.Close();
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(vm);
                }
            }

        }
    }
}
