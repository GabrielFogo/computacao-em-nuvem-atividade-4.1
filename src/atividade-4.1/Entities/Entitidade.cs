namespace atividade_4._1.Entities;

public abstract class Entitidade
{
    public Guid Id { get; private set; }

    public Entitidade()
    {
        Id = Guid.CreateVersion7();
    }
}
