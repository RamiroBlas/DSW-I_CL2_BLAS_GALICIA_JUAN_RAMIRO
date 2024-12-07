using DSW_I_CL2_BLAS_GALICIA_JUAN_RAMIRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace DSW_I_CL2_BLAS_GALICIA_JUAN_RAMIRO.Controllers
{
    public class DiscoSolidoController : Controller
    {

        public readonly IConfiguration _config;

        public DiscoSolidoController(IConfiguration IConfig)
        {
            _config = IConfig;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DiscoSolido()
        {
            return View();
        }

        public IActionResult Calcular(ClassCalcular model)
        {
            string mensaje = "";
            const int capacidadCdMb = 700;
            const int capacidadGbMb = 1024;

            int capacidadTotalMb = model.CapaDiscoGB * capacidadGbMb;
            int cdsNecesarios = (int)Math.Ceiling((double)capacidadTotalMb / capacidadCdMb);

            model.CapaCdMB = capacidadTotalMb;
            model.TotalCds = cdsNecesarios;

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))

            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_guardar_cdsblasgalicia", cn);
                    cn.Open();

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cliente", model.Cliente);
                    cmd.Parameters.AddWithValue("@CapaDisco", model.CapaDiscoGB);
                    cmd.Parameters.AddWithValue("@CapaCds", model.CapaCdMB);
                    cmd.Parameters.AddWithValue("@TotalCds", model.TotalCds);

                    int c = cmd.ExecuteNonQuery();
                    if (c > 0)
                    {
                        mensaje = $"Registro insertado correctamente. Filas afectadas: {c}";
                    }
                    else
                    {
                        mensaje = "No se insertaron filas. Revisa el procedimiento almacenado.";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error: {ex.Message}";
                    throw;
                }

            }
            ViewBag.mensaje = mensaje;

            return View("Resultado", model);
            //return RedirectToAction("ListadoCalculo", "DiscoSolido");
        }

        IEnumerable<ClassCalcular> Calculos()
        {
            List<ClassCalcular> calculos = new List<ClassCalcular>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_listar_cdsblasgalicia", cn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    calculos.Add(new ClassCalcular
                    {
                        Idcd = dr.GetInt32(0),
                        Cliente = dr.GetString(1),
                        CapaDiscoGB = dr.GetInt32(2),
                        CapaCdMB = dr.GetInt32(3),
                        TotalCds = dr.GetInt32(4)
                    });
                }

            }
            return calculos;
        }

        public async Task<IActionResult> ListadoCalculos(int p)
        {
            int nr = 5;

            int tr = Calculos().Count();
            int paginas = nr % tr > 0 ? tr / nr + 1 : tr / nr;
            ViewBag.paginas = paginas;

            return View(await Task.Run(() => Calculos().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ClassCalcular editarCalculo = new ClassCalcular();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_buscar_cdsblasgalicia @Idcd", cn);
                cmd.Parameters.AddWithValue("@Idcd", id);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        editarCalculo.Idcd = dr.GetInt32(0);
                        editarCalculo.Cliente = dr.GetString(1);
                        editarCalculo.CapaDiscoGB = dr.GetInt32(2);
                    }

                }
            }
            return View(editarCalculo);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult Edit_Post(ClassCalcular classCalcular)
        {
            string cli = classCalcular.Cliente;
            string mensaje = "";

            const int capacidadCdMb = 700;
            const int capacidadGbMb = 1024;

            int capacidadTotalMb = classCalcular.CapaDiscoGB * capacidadGbMb;
            int cdsNecesarios = (int)Math.Ceiling((double)capacidadTotalMb / capacidadCdMb);

            classCalcular.CapaCdMB = capacidadTotalMb;
            classCalcular.TotalCds = cdsNecesarios;

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))

            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizar_cdblasgalicia", cn);
                    cn.Open();

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Idcd", classCalcular.Idcd);
                    cmd.Parameters.AddWithValue("@cliente", classCalcular.Cliente);
                    cmd.Parameters.AddWithValue("@CapaDisco", classCalcular.CapaDiscoGB);
                    cmd.Parameters.AddWithValue("@CapaCds", classCalcular.CapaCdMB);
                    cmd.Parameters.AddWithValue("@TotalCds", classCalcular.TotalCds);

                    int c = cmd.ExecuteNonQuery();
                    if (c > 0)
                    {
                        mensaje = $"Registro actualizado correctamente. Filas afectadas: {c}";
                    }
                    else
                    {
                        mensaje = "No se actualizaron filas. Revisa el procedimiento almacenado.";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error: {ex.Message}";
                    throw;
                }

            }
            ViewBag.mensaje = mensaje;

            return View("Resultado", classCalcular);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            ClassCalcular elmininarCalculo = new ClassCalcular();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_buscar_cdsblasgalicia @Idcd", cn);
                cmd.Parameters.AddWithValue("@Idcd", id);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        elmininarCalculo.Idcd = dr.GetInt32(0);
                        elmininarCalculo.Cliente = dr.GetString(1);
                        elmininarCalculo.CapaDiscoGB = dr.GetInt32(2);
                        elmininarCalculo.CapaCdMB = dr.GetInt32(3);
                        elmininarCalculo.TotalCds = dr.GetInt32(4);

					}

                }
            }
            return View(elmininarCalculo);

        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteCalular(int id)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_cdsblasgalicia", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cn.Open();

					cmd.Parameters.AddWithValue("@Idcd", id);

                    int c = cmd.ExecuteNonQuery();
                    if (c > 0)
                    {
                        mensaje = $"Registro eliminado correctamente. Filas afectadas: {c}";
                    }
                    else
                    {
                        mensaje = "No se elimino fila. Revisa el procedimiento almacenado.";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error: {ex.Message}";
                    throw;
                }

            }
            ViewBag.mensaje = mensaje;

            return RedirectToAction("ListadoCalculos");
        }

        public IActionResult Resultado()
        {
            return View();
        }
    }
}
