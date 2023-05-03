using apiChat.DTOs;
using apiChat.Hubs.Clients;
using apiChat.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;

namespace apiChat.Hubs
{
    public class ConexionesHub : Hub
    {
        private static FileStream _fl = new FileStream("ConexionUser.txt", FileMode.Open);
        private static int _contUsOnSite = 0;
        private static int _conUsMonitoreo = 0;
        private readonly IMapper _mapper;

        //--------------------metodos de conexion para OnSite-----------------------
        public async Task ConexionOnSite(DtoConexionUser dtoConUser)
        {
            _contUsOnSite++;
            await BitacoraConexion(true, dtoConUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName: dtoConUser.origen);
        }

        public async Task DesconexionOnSite(DtoConexionUser dtoConUser)
        {
            _contUsOnSite--;
            await BitacoraConexion(false,dtoConUser);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName: dtoConUser.origen);
        }
        //---------------------------------------------------------------------------

        //---------------------- metodo de conexion Monitor--------------------------
        public async Task ConexionMonitor(DtoConexionUser dtoConUser)
        {
            _conUsMonitoreo++;
            await BitacoraConexion(true, dtoConUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName: dtoConUser.origen);
            await OnConnectedAsync();
        }

        public async Task DesconexionMonitor(DtoConexionUser dtoConUser)
        {
            _conUsMonitoreo--;
            await BitacoraConexion(false, dtoConUser);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName: dtoConUser.origen);
            await OnDisconnectedAsync(null);
        }
        //---------------------------------------------------------------------------
        public override async Task OnConnectedAsync()
        {
            await Clients.Group("Monitoreo").SendAsync("UsersConnected", _contUsOnSite);
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Group("Monitoreo").SendAsync("UsersDisconnected", _contUsOnSite);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task BitacoraConexion(bool swtch, DtoConexionUser dtoConUser) 
        {
         
            var   conexion = _mapper.Map<ConexionUser>(dtoConUser);

            if (swtch == true) {

                using (StreamReader sr = new StreamReader(_fl))
                {
                   while(sr.Read() != null) {
                        conexion.idConexion = dtoConUser.idConexion;
                        conexion.origen = dtoConUser.origen;
                        conexion.usuario = dtoConUser.usuario;
                    }

                    using (StreamWriter sw = new StreamWriter(_fl)) {
                        await sw.WriteLineAsync( conexion.idConexion +","+ conexion.origen +","+ conexion.usuario +","+ conexion.horaConexion 
                            +","+ conexion.horaDesconexion +","+ conexion.tiempoConexion + "," + conexion.estatus);
                    }
                }

            }
            else
            {
                using (StreamReader sr = new StreamReader(_fl))
                {
                    while (!sr.EndOfStream) { 
                        var spltStr = sr.ReadLine().Split(",");

                        if (spltStr[0] == dtoConUser.idConexion && spltStr[1] == dtoConUser.origen && spltStr[4] == string.Empty 
                            && spltStr[5] == string.Empty && spltStr[6] == "Conectado") { 
                        
                            conexion.idConexion = Context.ConnectionId;
                            conexion.origen = dtoConUser.origen;
                            conexion.usuario = spltStr[2];
                            conexion.horaConexion = spltStr[3];
                            conexion.horaDesconexion = DateTime.Now.ToString();
                            conexion.tiempoConexion = Convert.ToString(DateTime.Parse(conexion.horaDesconexion) - DateTime.Parse(conexion.horaConexion));
                            conexion.estatus = "Desconectado";
                        }
                    }
                    using (StreamWriter sw = new StreamWriter(_fl))
                    {
                        await sw.WriteLineAsync(conexion.idConexion +","+ conexion.origen +","+ conexion.usuario +","+ conexion.horaConexion +","
                            + conexion.horaDesconexion +","+ conexion.tiempoConexion +","+ conexion.estatus);
                    }
                }
            }
        }

    }
}
