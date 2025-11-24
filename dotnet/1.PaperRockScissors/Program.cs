
var appCancellation = new CancellationTokenSource();
Console.CancelKeyPress += delegate {
    Console.WriteLine();
    Console.WriteLine("See ya!");
    appCancellation.Cancel();
};

Console.WriteLine("Hello, welcome to the Rock, Paper and Scissors!");

// It's going to be User VS Bot
// The Bot will count down from 5 to 1 
// The user should be able to type their option on that time.
// If the user does not provide an option, the bot will say that will wait until the user is ready (press enter)
// rinse and repeat

// if the user provided the value (r / p / s)
// the computer will check who won
// the bot will then say if he won or not

// ask if the user wants to play again
// Keep a score from rounds
// Print the score at the end (Ctrl+C)


var gameLoopTask = GameLoopAsync(appCancellation.Token);

await Task.WhenAll(gameLoopTask);

async Task GameLoopAsync(CancellationToken c)
{
    while(c.IsCancellationRequested is false)
    {
        Console.WriteLine("Press any key to play next round.");
        Console.ReadKey(intercept:true);

        Console.WriteLine("Rock (1), Paper (2), Scissors (3) on");
        var cancelGameLoop = CancellationTokenSource.CreateLinkedTokenSource(c);
        var countDownTask = CountDownAsync(seconds:3, cancelGameLoop.Token);

        var entryTask = ReadUserEntryAsync(cancelGameLoop.Token);

        await Task.WhenAny(countDownTask, entryTask);
        cancelGameLoop.Cancel();
        Console.WriteLine("debug: cancelled");

        var timeLeft = await countDownTask;
        var entry = await entryTask;

        Console.WriteLine($"Entry: {entry}, Time Left: {timeLeft}");
    
        await Task.Delay(1000, c);
        Console.WriteLine("-----------------");
        Console.WriteLine();
    }
}

async Task<int> CountDownAsync(int seconds, CancellationToken e)
{
    const int printDotOn = 250;
    var remainingMs = seconds * 1000;
    while(!e.IsCancellationRequested && remainingMs > 0)
    {
        var currentSec= Math.DivRem(remainingMs, 1000, out var remainder);
        if(remainder == 0)
            Console.Write(currentSec);
        else 
            Console.Write(".");

        await Task.Delay(printDotOn);
        remainingMs -= printDotOn;
    }
    return remainingMs;
}

async Task<char?> ReadUserEntryAsync(CancellationToken c)
{
    while (!Console.KeyAvailable) 
    {
        try { await Task.Delay(100, c); }
        catch { return null; }
    }
    return Console.ReadKey(intercept: true).KeyChar;
}