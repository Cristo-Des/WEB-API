using ASP.NET_LOGIN.Extensions;
using ASP.NET_LOGIN.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;


namespace ASP.NET_LOGIN.Controllers
{
    public class AccesoController : Controller
    {
      
        static string cadena = "Server=localhost; Database=practica; Username=root; Password=admin;";

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult CambiarNombre()
        {
            return View();
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Acceso");
        }

        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult Bienvenido()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(Usuario nUsuario)
        {
           // bool registrado;
            //string mensaje;
            if (nUsuario.Contraseña == nUsuario.ConfirmarContra)
            {


                nUsuario.Contraseña = ConvertirSha256(nUsuario.Contraseña);
            }
            else
            {
                ViewData["Mensaje"] = "Las Contraseñas no Coinciden";
                return View();
            }

            using (MySqlConnection cn = new MySqlConnection(cadena))
            {

                string sql = "INSERT INTO Usuario (nombre, correo, clave) VALUES (@valor1, @valor2, @valor3);";
                MySqlCommand cmd = new MySqlCommand(sql, cn);
                // Agregar los parámetros con los valores
                cmd.Parameters.AddWithValue("@valor1", nUsuario.Nombre);
                cmd.Parameters.AddWithValue("@valor2", nUsuario.Correo);
                cmd.Parameters.AddWithValue("@valor3", nUsuario.Contraseña);
                cn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                //registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                //mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                //ViewData["Mensaje"] = mensaje;

                if (rowsAffected > 0)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                else
                {
                    return View();
                }
            }
        }
        [HttpPost]
        public IActionResult Login(Usuario nUsuario)
        {
            nUsuario.Contraseña = ConvertirSha256(nUsuario.Contraseña);
            using (MySqlConnection cn = new MySqlConnection(cadena))
            {

                string sql = "SELECT * FROM Usuario WHERE correo = @valor1 and clave = @valor2 and status = 1;";
                MySqlCommand cmd = new MySqlCommand(sql, cn);
                // Agregar los parámetros con los valores
                cmd.Parameters.AddWithValue("@valor1", nUsuario.Correo);
                cmd.Parameters.AddWithValue("@valor2", nUsuario.Contraseña);
                cn.Open();
                //int rowsAffected = cmd.ExecuteNonQuery();

                //nUsuario.id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
               
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Usuario us = new Usuario();
                    us.id = Convert.ToInt32(reader["id"]);
                    us.Nombre = reader["nombre"]+"";
                    us.Correo = reader["correo"] + "";
                    us.Contraseña = reader["clave"] + "";
                    HttpContext.Session.SetObjectAsJson("unUsuario", us);
                    Usuario user = HttpContext.Session.GetObjectFromJson<Usuario>("unUsuario");
                    TempData["un"] = JsonConvert.SerializeObject(user);
                    
                    return RedirectToAction("Bienvenido", "Acceso");
                }
                else
                {
                    ViewData["Mensaje"] = "Usuario No Encontrado";
                    return View();
                }

            }
            
        }
        [HttpPost]
        public IActionResult EliminarUsuario()
        {
            Usuario user = HttpContext.Session.GetObjectFromJson<Usuario>("unUsuario");
            using (MySqlConnection cn = new MySqlConnection(cadena))
            {

                string sql = "UPDATE Usuario SET status = 0 WHERE id = @id;";
                MySqlCommand cmd = new MySqlCommand(sql, cn);
                // Agregar los parámetros con los valores
                cmd.Parameters.AddWithValue("@id", user.id);               
                cn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                else
                {
                    return View();
                }
            }
        }
        [HttpPost]
        public IActionResult CambiarNombre(Usuario nUsuario)
        {
            Usuario user = HttpContext.Session.GetObjectFromJson<Usuario>("unUsuario");
            using (MySqlConnection cn = new MySqlConnection(cadena))
            {

                string sql = "UPDATE Usuario SET nombre = @nombre   WHERE id = @id and status = 1 ;";
                MySqlCommand cmd = new MySqlCommand(sql, cn);
                // Agregar los parámetros con los valores
                cmd.Parameters.AddWithValue("@id", user.id);
                cmd.Parameters.AddWithValue("@nombre", nUsuario.Nombre);
                cn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                else
                {
                    ViewData["Mensaje"] = "Nombre Hay Nombre";
                    return View();
                }
            }
        }
        public static string ConvertirSha256(string texto)
        {
    
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
