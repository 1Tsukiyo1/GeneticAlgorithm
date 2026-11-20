using System;
using System.Text;

namespace GaOperators;

/// <summary>
/// Оператор побитовой мутации: инвертирует каждый ген хромосомы с заданной вероятностью.
/// </summary>
public sealed class BitFlipMutation : IMutation
{
    private readonly double _mutationProbability;

    /// <summary>
    /// Инициализирует оператор побитовой мутации.
    /// </summary>
    /// <param name="mutationProbability">Вероятность мутации каждого отдельного гена в диапазоне [0, 1].</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если <paramref name="mutationProbability"/> меньше 0, больше 1 или является <see cref="double.NaN"/>.
    /// </exception>
    public BitFlipMutation(double mutationProbability)
    {
        if (double.IsNaN(mutationProbability) || mutationProbability < 0.0 || mutationProbability > 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(mutationProbability),
                "Вероятность мутации должна находиться в диапазоне от 0 до 1 включительно.");
        }

        _mutationProbability = mutationProbability;
    }

    /// <summary>
    /// Возвращает заданную вероятность мутации гена.
    /// </summary>
    public double MutationProbability => _mutationProbability;

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="source"/> или <paramref name="rng"/> равен <see langword="null"/>.
    /// </exception>
    public Chromosome Mutate(Chromosome source, Random rng)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(rng);

        bool[] genes = source.ToArray();
        for (int i = 0; i < genes.Length; i++)
        {
            if (rng.NextDouble() < _mutationProbability)
            {
                genes[i] = !genes[i];
            }
        }

        return new Chromosome(genes)
        {
            Fitness = source.Fitness
        };
    }
}

/// <summary>
/// Оператор одноточечного скрещивания двух хромосом одинаковой длины.
/// </summary>
public sealed class SinglePointCrossover : ICrossover
{
    private readonly double _crossoverProbability;

    /// <summary>
    /// Инициализирует оператор одноточечного скрещивания.
    /// </summary>
    /// <param name="crossoverProbability">Вероятность выполнения скрещивания в диапазоне [0, 1].</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если <paramref name="crossoverProbability"/> меньше 0, больше 1 или является <see cref="double.NaN"/>.
    /// </exception>
    public SinglePointCrossover(double crossoverProbability)
    {
        if (double.IsNaN(crossoverProbability) || crossoverProbability < 0.0 || crossoverProbability > 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(crossoverProbability),
                "Вероятность скрещивания должна находиться в диапазоне от 0 до 1 включительно.");
        }

        _crossoverProbability = crossoverProbability;
    }

    /// <summary>
    /// Возвращает заданную вероятность скрещивания.
    /// </summary>
    public double CrossoverProbability => _crossoverProbability;

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="parent1"/>, <paramref name="parent2"/> или <paramref name="rng"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если длины <paramref name="parent1"/> и <paramref name="parent2"/> не совпадают.
    /// </exception>
    public (Chromosome Child1, Chromosome Child2) Cross(Chromosome parent1, Chromosome parent2, Random rng)
    {
        ArgumentNullException.ThrowIfNull(parent1);
        ArgumentNullException.ThrowIfNull(parent2);
        ArgumentNullException.ThrowIfNull(rng);

        if (parent1.Length != parent2.Length)
        {
            throw new ArgumentException("Родительские хромосомы должны иметь одинаковую длину.");
        }

        if (rng.NextDouble() >= _crossoverProbability || parent1.Length < 2)
        {
            return (parent1.Clone(), parent2.Clone());
        }

        int splitPoint = rng.Next(1, parent1.Length);
        int length = parent1.Length;

        bool[] child1Genes = new bool[length];
        bool[] child2Genes = new bool[length];

        for (int i = 0; i < length; i++)
        {
            if (i < splitPoint)
            {
                child1Genes[i] = parent1[i];
                child2Genes[i] = parent2[i];
            }
            else
            {
                child1Genes[i] = parent2[i];
                child2Genes[i] = parent1[i];
            }
        }

        return (new Chromosome(child1Genes), new Chromosome(child2Genes));
    }
}

/// <summary>
/// Оператор равномерного скрещивания: для каждой позиции гена независимо выбирает родителя-источника.
/// </summary>
public sealed class UniformCrossover : ICrossover
{
    private readonly double _crossoverProbability;

    /// <summary>
    /// Инициализирует оператор равномерного скрещивания.
    /// </summary>
    /// <param name="crossoverProbability">Вероятность выполнения скрещивания в диапазоне [0, 1].</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если <paramref name="crossoverProbability"/> меньше 0, больше 1 или является <see cref="double.NaN"/>.
    /// </exception>
    public UniformCrossover(double crossoverProbability)
    {
        if (double.IsNaN(crossoverProbability) || crossoverProbability < 0.0 || crossoverProbability > 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(crossoverProbability),
                "Вероятность скрещивания должна находиться в диапазоне от 0 до 1 включительно.");
        }

        _crossoverProbability = crossoverProbability;
    }

    /// <summary>
    /// Возвращает заданную вероятность скрещивания.
    /// </summary>
    public double CrossoverProbability => _crossoverProbability;

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="parent1"/>, <paramref name="parent2"/> или <paramref name="rng"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если длины <paramref name="parent1"/> и <paramref name="parent2"/> не совпадают.
    /// </exception>
    public (Chromosome Child1, Chromosome Child2) Cross(Chromosome parent1, Chromosome parent2, Random rng)
    {
        ArgumentNullException.ThrowIfNull(parent1);
        ArgumentNullException.ThrowIfNull(parent2);
        ArgumentNullException.ThrowIfNull(rng);

        if (parent1.Length != parent2.Length)
        {
            throw new ArgumentException("Родительские хромосомы должны иметь одинаковую длину.");
        }

        if (rng.NextDouble() >= _crossoverProbability)
        {
            return (parent1.Clone(), parent2.Clone());
        }

        int length = parent1.Length;
        bool[] child1Genes = new bool[length];
        bool[] child2Genes = new bool[length];

        for (int i = 0; i < length; i++)
        {
            if (rng.Next(0, 2) == 0)
            {
                child1Genes[i] = parent1[i];
                child2Genes[i] = parent2[i];
            }
            else
            {
                child1Genes[i] = parent2[i];
                child2Genes[i] = parent1[i];
            }
        }

        return (new Chromosome(child1Genes), new Chromosome(child2Genes));
    }
}

/// <summary>
/// Оператор турнирного отбора: проводит серию независимых турниров среди случайно выбранных особей.
/// </summary>
public sealed class TournamentSelection : ISelection
{
    private readonly int _tournamentSize;

    /// <summary>
    /// Инициализирует оператор турнирного отбора.
    /// </summary>
    /// <param name="tournamentSize">Количество различных участников в одном турнире (не меньше 2).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если <paramref name="tournamentSize"/> меньше 2.
    /// </exception>
    public TournamentSelection(int tournamentSize)
    {
        if (tournamentSize < 2)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tournamentSize),
                "Размер турнира должен быть не меньше 2.");
        }

        _tournamentSize = tournamentSize;
    }

    /// <summary>
    /// Возвращает размер одного турнира.
    /// </summary>
    public int TournamentSize => _tournamentSize;

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="population"/> или <paramref name="rng"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если <paramref name="count"/> меньше 0.
    /// </exception>
    public int[] Select(IReadOnlyList<Chromosome> population, int count, Random rng)
    {
        ArgumentNullException.ThrowIfNull(population);
        ArgumentNullException.ThrowIfNull(rng);

        if (population.Count == 0)
        {
            throw new ArgumentException("Популяция не должна быть пустой.", nameof(population));
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Количество отборов не может быть отрицательным.");
        }

        if (_tournamentSize > population.Count)
        {
            throw new ArgumentException(
                "Размер турнира не может превышать размер популяции.",
                nameof(population));
        }

        int[] winners = new int[count];
        int[] candidatePool = new int[population.Count];

        for (int t = 0; t < count; t++)
        {
            for (int i = 0; i < candidatePool.Length; i++)
            {
                candidatePool[i] = i;
            }

            for (int i = 0; i < _tournamentSize; i++)
            {
                int swapIndex = rng.Next(i, candidatePool.Length);
                (candidatePool[i], candidatePool[swapIndex]) = (candidatePool[swapIndex], candidatePool[i]);
            }

            int bestIndex = candidatePool[0];
            double bestFitness = population[bestIndex].Fitness;

            for (int i = 1; i < _tournamentSize; i++)
            {
                int idx = candidatePool[i];
                double fitness = population[idx].Fitness;
                if (fitness > bestFitness)
                {
                    bestFitness = fitness;
                    bestIndex = idx;
                }
            }

            winners[t] = bestIndex;
        }

        return winners;
    }
}
