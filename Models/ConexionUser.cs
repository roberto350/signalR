namespace apiChat.Models
{
    public class ConexionUser
    {
        public string idConexion { get; set; }
        public string origen { get; set; }
        public string usuario { get; set; } 
        public string horaConexion { get; set; } = DateTime.Now.ToString();
        public string horaDesconexion { get; set; } = string.Empty;
        public string tiempoConexion { get; set; } = string.Empty;
        public string estatus { get; set; } = "Conectado";
        
    }
}
