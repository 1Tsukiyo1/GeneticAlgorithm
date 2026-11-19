using System;
using System.Text;

namespace GaOperators;

/// <summary>
/// Определяет оператора скрещивания двух родительских хромосом.
/// </summary>
public interface ICrossover
{
    /// <summary>
    /// Создаёт пару потомков на основе двух родительских хромосом.
    /// </summary>
    /// <param name="parent1">Первая родительская хромосома.</param>
    /// <param name="parent2">Вторая родительская хромосома.</param>
    /// <param name="rng">Генератор случайных чисел.</param>
    /// <returns>Кортеж из двух новых хромосом-потомков (<c>Child1</c> и <c>Child2</c>).</returns>
    (Chromosome Child1, Chromosome Child2) Cross(Chromosome parent1, Chromosome parent2, Random rng);
}
