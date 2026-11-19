using System;
using System.Text;

namespace GaOperators;

/// <summary>
/// Определяет оператора мутации хромосомы.
/// </summary>
public interface IMutation
{
    /// <summary>
    /// Создаёт мутированную копию переданной хромосомы.
    /// </summary>
    /// <param name="source">Исходная хромосома.</param>
    /// <param name="rng">Генератор случайных чисел.</param>
    /// <returns> Новый экземпляр <see cref="Chromosome"/> после применения мутации.</returns>
    Chromosome Mutate(Chromosome source, Random rng);
}