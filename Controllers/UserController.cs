using apiChat.Contexto;
using apiChat.Hubs;
using apiChat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace apiChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
        {

        public static string rdm = "";

        [HttpPost("Conexion/Conectar")]
        public async Task<ActionResult> ConexionUsers()
        {
            var random = new Random();
            FileStream fl = new FileStream("ConexionUser.txt", FileMode.Open);
            
           

            using (StreamReader rd = new StreamReader(fl))
            {
                if (rd.Read() != null) {
                    /*var conexiones = new List<ConexionUser>();
                    while (!rd.EndOfStream)
                    {

                        var con = new ConexionUser();
                        var conexionCut = rd.ReadLine().Split(",");

                        con.idConexion = conexionCut[0];
                        con.origen = conexionCut[1];
                        con.usuario = conexionCut[2];
                        con.horaConexion = conexionCut[3];
                        con.horaDesconexion = conexionCut[4];
                        con.tiempoConexion = conexionCut[5];
                        con.estatus = conexionCut[6];
                        conexiones.Add(con);
                    }*/

                    var conexion = new ConexionUser();
                    conexion.idConexion = "raac" + random.Next();
                    conexion.origen = "OnSite";
                    conexion.usuario = "Robert";
                    conexion.horaConexion = DateTime.Now.ToString();
                    conexion.horaDesconexion = "";
                    conexion.tiempoConexion = "";
                    conexion.estatus = "Conectado";

                    rdm = conexion.idConexion;

                    using (StreamWriter sw = new StreamWriter(fl))
                    {
                        sw.WriteLine(conexion.idConexion + "," + conexion.origen + "," + conexion.usuario + "," + conexion.horaConexion + ","
                            + conexion.horaDesconexion + "," + conexion.tiempoConexion + "," + conexion.estatus);
                    }
                }
            }
            return NoContent();
        }
        [HttpPost("Conexion/Desconectar")]
        public async Task<ActionResult> DesconexionUser()
        {
            
            FileStream fl = new FileStream("ConexionUser.txt", FileMode.Open);

            var conexion = new ConexionUser();

            using (StreamReader rd = new StreamReader(fl))
            {
                while (!rd.EndOfStream)
                {
                    var splStr = rd.ReadLine().Split(",");

                    if (splStr[0] == rdm && splStr[4] == "" && splStr[5] == "" && splStr[6] == "Conectado")
                    {
                        conexion.idConexion = splStr[0];
                        conexion.origen = splStr[1];
                        conexion.usuario = splStr[2];
                        conexion.horaConexion = splStr[3];
                        conexion.horaDesconexion = DateTime.Now.ToString();
                        conexion.tiempoConexion = Convert.ToString(DateTime.Parse(conexion.horaDesconexion) - DateTime.Parse(conexion.horaConexion));
                        conexion.estatus = "Desconectado";
                    }
                }

                using(StreamWriter sw = new StreamWriter(fl))
                {
                            sw.WriteLine(conexion.idConexion + "," + conexion.origen + "," + conexion.usuario + "," + conexion.horaConexion + "," 
                                + conexion.horaDesconexion + "," + conexion.tiempoConexion+","+ conexion.estatus);
                        
                    }
                }
                 
            return NoContent();
        }


    }
}
