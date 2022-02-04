using Kse.Algorithms.Samples;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
    Noise = .1f,
    AddTraffic = true,
    TrafficSeed = 1234
});

string[,]? map = generator.Generate();

new MapPrinter().Print(map);

int StartX, StartY, EndX, EndY;
Console.Write("StartX:");StartX = int.Parse(Console.ReadLine());
Console.Write("StartY:");StartY = int.Parse(Console.ReadLine());
Console.Write("EndX:");EndX = int.Parse(Console.ReadLine());
Console.Write("EndY:");EndY = int.Parse(Console.ReadLine());


for (var row = 0; row < map.GetLength(1); row++)
{
    Console.Write($"{row}\t");
    for (var column = 0; column < map.GetLength(0); column++)
    {
        if (StartX == column && StartY == row) 
            Console.Write("A");
        else if (EndX == column && EndY == row) 
            Console.Write("B");
        else 
            Console.Write(map[column, row]);
    }

    Console.WriteLine();
}

void KakHochesh(string[,] map, Point Start, Point End)
{
    if (Start.Column < 0 && Start.Row < 0 && End.Column < 0 && End.Row < 0) return;
    if (Start.Column > map.GetLength(0) && Start.Row > map.GetLength(1) && End.Column > map.GetLength(0) && End.Row > map.GetLength(1)) return;
    if (map[Start.Column, Start.Row] == MapGenerator.Wall) return;
    if (map[End.Column, End.Row] == MapGenerator.Wall) return;
    if (Start.Equals(End)) return;
}

var path = MazeSolver.Dijkstra(map, new Point(StartX, StartY), new Point(EndX, EndY));
for (var row = 0; row < map.GetLength(1); row++)
{
    Console.Write($"{row}\t");
    for (var column = 0; column < map.GetLength(0); column++)
    {
        if (StartX == column && StartY == row) 
            Console.Write("A");
        else if (EndX == column && EndY == row) 
            Console.Write("B");
        else
        {
            bool isPath = false;

            foreach (var p in path) {
                if (p.Position.Equals(new Point(column, row))) {
                    isPath = true;
                }
            }

            if (isPath) Console.Write("*");
            else Console.Write(map[column, row]);
        }
        
            
            
        
    }

    Console.WriteLine();
}

