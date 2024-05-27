namespace ListaDeTarefas.Dto
{
    public record class TarefaRequest(string Titulo, string descricao, DateOnly Prazo, bool status);
    
}
