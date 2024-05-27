using System.ComponentModel.DataAnnotations;

namespace ListaDeTarefas.Modelos
{
    public  class Tarefa
    {
        
        [Key]
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateOnly Prazo { get; set; }
        public bool Status { get; set; }
    }
    
}
