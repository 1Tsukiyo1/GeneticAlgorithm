using System;
using System.Text;

namespace GaOperators;

/// <summary>
/// Представляет бинарную хромосому фиксированной длины с заданным значением приспособленности.
/// </summary>
public sealed class Chromosome
{
    private readonly bool[] _genes;

    /// <summary>
    /// Создаёт новую хромосому на основе массива бинарных генов.
    /// </summary>
    /// <param name="genes">Исходный массив генов (не может быть <see langword="null"/> или пустым).</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="genes"/> равен <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Выбрасывается, если длина <paramref name="genes"/> меньше 1.</exception>
    public Chromosome(bool[] genes)
    {
        ArgumentNullException.ThrowIfNull(genes);
        if (genes.Length < 1)
        {
            throw new ArgumentException("Длина хромосомы должна быть не меньше 1.", nameof(genes));
        }

        _genes = new bool[genes.Length];
        Array.Copy(genes, _genes, genes.Length);
    }

    /// <summary>
    /// Возвращает количество генов в хромосоме.
    /// </summary>
    public int Length => _genes.Length;

    /// <summary>
    /// Возвращает или задаёт значение приспособленности хромосомы, вычисленное вызывающим кодом.
    /// </summary>
    public double Fitness { get; set; }

    /// <summary>
    /// Возвращает значение гена по указанному индексу.
    /// </summary>
    /// <param name="index">Индекс гена в диапазоне от 0 до <see cref="Length"/> - 1.</param>
    /// <returns>Значение гена (<see langword="true"/> или <see langword="false"/>).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если индекс выходит за допустимые пределы.</exception>
    public bool this[int index]
    {
        get
        {
            if (index < 0 || index >= _genes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _genes[index];
        }
    }

    /// <summary>
    /// Создаёт независимую копию массива генов хромосомы.
    /// </summary>
    /// <returns>Новый массив <see cref="bool"/> с копией генов.</returns>
    public bool[] ToArray()
    {
        var copy = new bool[_genes.Length];
        Array.Copy(_genes, copy, _genes.Length);
        return copy;
    }

    /// <summary>
    /// Создаёт глубокую копию хромосомы с независимым массивом генов и тем же значением <see cref="Fitness"/>.
    /// </summary>
    /// <returns>Новый экземпляр <see cref="Chromosome"/>.</returns>
    public Chromosome Clone()
    {
        return new Chromosome(_genes)
        {
            Fitness = Fitness
        };
    }
}
