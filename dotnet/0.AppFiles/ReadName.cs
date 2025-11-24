bool isValid = false; 
string name = string.Empty;
Console.WriteLine("input your name");
while(!isValid)
{
  name = Console.ReadLine();
  switch(name.Length) 
  {
    case < 3:
      Console.WriteLine("too short");
      continue;
    case > 12:
      Console.WriteLine("too big");
      continue;
    default:
      isValid = true;
      break;
  }
}

Console.WriteLine($"Hello {name}");