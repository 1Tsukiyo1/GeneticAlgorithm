using GaOperators;

var rng = new Random(123);

List<Chromosome> population =
[
    new Chromosome([true, false, true, false, false, true]) { Fitness = 3.0 },
    new Chromosome([true, true, true, false, true, false]) { Fitness = 8.5 },
    new Chromosome([false, false, false, true, false, false]) { Fitness = 1.5 },
    new Chromosome([true, false, false, true, true, true]) { Fitness = 6.0 }
];

Console.WriteLine("Исходная популяция:");
for (int i = 0; i < population.Count; i++)
{
    Console.WriteLine($"  [{i}] {FormatGenes(population[i])} | Fitness = {population[i].Fitness:F1}");
}

ISelection selection = new TournamentSelection(tournamentSize: 2);
ICrossover singlePoint = new SinglePointCrossover(crossoverProbability: 0.9);
ICrossover uniform = new UniformCrossover(crossoverProbability: 0.8);
IMutation mutation = new BitFlipMutation(mutationProbability: 0.2);

int[] selectedIndices = selection.Select(population, count: 2, rng);
Console.WriteLine($"\nВыбранные индексы родителей: [{string.Join(", ", selectedIndices)}]");

Chromosome parent1 = population[selectedIndices[0]];
Chromosome parent2 = population[selectedIndices[1]];

var (spChild1, spChild2) = singlePoint.Cross(parent1, parent2, rng);
Console.WriteLine($"Одноточечное скрещивание -> Child1: {FormatGenes(spChild1)}, Child2: {FormatGenes(spChild2)}");

var (unChild1, unChild2) = uniform.Cross(parent1, parent2, rng);
Console.WriteLine($"Равномерное скрещивание -> Child1: {FormatGenes(unChild1)}, Child2: {FormatGenes(unChild2)}");

Chromosome mutatedChild = mutation.Mutate(spChild1, rng);
Console.WriteLine($"Мутация Child1 -> {FormatGenes(mutatedChild)}");

static string FormatGenes(Chromosome chromosome)
{
    char[] chars = new char[chromosome.Length];
    for (int i = 0; i < chromosome.Length; i++)
    {
        chars[i] = chromosome[i] ? '1' : '0';
    }
    return new string(chars);
}
