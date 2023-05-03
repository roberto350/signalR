using apiChat.Models;

namespace apiChat.Hubs.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(ConexionUser message);

        Task ConexionMessage(int contador);

        Task ConexionMessageDos(int contador);

     }
}
