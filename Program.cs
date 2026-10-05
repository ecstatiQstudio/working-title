Start:

Console.WriteLine("Enter path to output directory:");

string outputDirectory = Console.ReadLine();

try
{
    Directory.CreateDirectory(outputDirectory);

    new Classes.Horse().Create(outputDirectory);

    Console.WriteLine("Success!");
}
catch (Exception exception)
{
    Console.WriteLine($"{exception.Message}\n{exception.StackTrace}");
}


Console.WriteLine("");