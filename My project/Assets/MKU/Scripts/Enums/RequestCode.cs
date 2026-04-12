namespace MKU.Scripts.Enums {
    public enum RequestCode
    {
        None = 0,        // Valor padrão ou sem solicitação específica
        User = 1,        // Solicitações relacionadas ao usuário (login, registro, etc.)
        Lobby = 2,       // Solicitações relacionadas ao lobby ou sala de espera
        Interface = 3,   // Solicitações para atualizar ou interagir com a interface do usuário
        Room = 4,        // Solicitações para salas de jogo ou de chat
        Server = 5,      // Solicitações relacionadas ao servidor
        Game = 6,        // Solicitações relacionadas ao jogo (iniciar, pausar, encerrar, etc.)
        Chat = 7,        // Solicitações relacionadas ao chat entre usuários
        MarketPlace = 8  // Solicitações relacionadas ao mercado virtual (compra/venda de itens)
    }
}