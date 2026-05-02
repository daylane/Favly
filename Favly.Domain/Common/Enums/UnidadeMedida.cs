namespace Favly.Domain.Common.Enums
{
    /// <summary>
    /// Unidades de medida base. Não existem sub-unidades (ex: grama, mililitro) —
    /// o usuário entra com decimais: 0,500 kg em vez de 500 g.
    /// </summary>
    public enum UnidadeMedida
    {
        Unidade   = 1,  // UN  — inteiros apenas
        Kilograma = 2,  // KG  — permite decimal (0,500 kg)
        Litro     = 4,  // LT  — permite decimal (0,500 L)
        Caixa     = 6,  // CX  — inteiros apenas
        Pacote    = 7,  // PCT — inteiros apenas
        Duzia     = 8,  // DZ  — inteiros apenas
        Cartela   = 9,  // CT  — inteiros apenas
    }
}
