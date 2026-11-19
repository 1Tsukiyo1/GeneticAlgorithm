using System;
using System.Text;

namespace GaOperators;

/// <summary>
/// Определяет оператора отбора родителей из популяции.
/// </summary>
public interface ISelection
{
    /// <summary>
    /// Выбирает индексы особей из популяции по итогам отбора.
    /// </summary>
    /// <param name="population">Популяция хромосом только для чтения.</param>
    /// <param name="count">Требуемое количество выбранных индексов родителей.</param>
    /// <param name="rng">Генератор случайных чисел.</param>
    /// <returns>Массив индексов выбранных хромосом в исходной популяции.</returns>
    int[] Select(IReadOnlyList<Chromosome> population, int count, Random rng);
}
