using System;
using System.Threading;
using System.IO;
using System.Speech;
using System.Media;

class CyberSecurityChatbot
{
    // Function to play greeting audio
    static void PlayGreetingAudio()
    {
        SoundPlayer player = new SoundPlayer("gretting.wav");
        try
        {
            player.Load();
            player.Play();
        }
        catch (Exception e)
        {
            Console.WriteLine("Audio file could not be played: " + e.Message);
        }
    }

    // UI functionality (function to simulate typing effect)
    static void SimulateTyping(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(50);
        }
        Console.WriteLine();
    }

    // Displaying an ASCIIArt logo for the chatbot
    static void PrintASCIIArt()
    {
        string asciiArt = @"

 
 ██████╗██╗   ██╗██████╗ ███████╗██████╗ ██████╗  ██████╗ ████████╗            
██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔═══██╗╚══██╔══╝            
██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██████╔╝██║   ██║   ██║               
██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██╔══██╗██║   ██║   ██║               
╚██████╗   ██║   ██████╔╝███████╗██║  ██║██████╔╝╚██████╔╝   ██║               
 ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝    ╚═╝                                                                                                                                                                                                                                                                                                             
        ";

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(asciiArt);
        Console.ResetColor();
    }

    // Asking  for the user's name and return it
    static string GetUserName()
    {
        SimulateTyping("What is your name?");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            name = "Guest";
        }
        return name;
    }

    // Giving out  a response based on user input
    static string GetResponse(string input)
    {
        // Convering  User input to lower case
        input = input.ToLower();

        // Responses to varying user input
        if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey") || input.Contains("greetings"))
        {
            return "Hello there! How can I assist you with cybersecurity today?";
        }
        else if (input.Contains("How are you") || input.Contains("how are you") || input.Contains("how r u"))

            return "Im just a bunch of code but im running fanstatic!Thanks for asking";

        else if (input.Contains("purpose") || input.Contains("what do you do") || input.Contains("why are you here") || input.Contains("goal") || input.Contains("job"))
        {
            return "I'm here to help you stay safe online by providing valuable cybersecurity tips and answering your questions.";
        }
        else if (input.Contains("password") || input.Contains("passwords") || input.Contains("safety") || input.Contains("secure"))
        {
            return "Always use strong, unique passwords for each account. A good password should contain a mix of letters, numbers, and symbols.";
        }
        else if (input.Contains("phishing") || input.Contains("phish"))
        {
            return "Phishing is when attackers try to trick you into giving out sensitive information. Be cautious of unsolicited emails or links.";
        }
        else if (input.Contains("safe browsing") || input.Contains("browsing") || input.Contains("browsing security") || input.Contains("browser"))
        {
            return "Make sure to only visit websites that are 'https' secure. Avoid clicking on suspicious links.";
        }
        else if (input.Contains("password manager") || input.Contains("manager"))
        {
            return "A password manager securely stores and organizes your passwords, making it easier to use strong, unique passwords without forgetting them.";
        }
        else if (input.Contains("malware") || input.Contains("virus") || input.Contains("spyware") || input.Contains("trojan"))
        {
            return "Malware is malicious software designed to harm your system. Make sure your antivirus software is up-to-date and avoid downloading files from untrusted sources.";
        }
        else if (input.Contains("vpn") || input.Contains("virtual private network"))
        {
            return "A VPN (Virtual Private Network) encrypts your internet connection and hides your IP address, enhancing your privacy while browsing.";
        }
        else if (input.Contains("what can i ask you") || input.Contains("what do you know") || input.Contains("topics") || input.Contains("help") || input.Contains("info"))
        {
            return "You can ask me about cybersecurity topics, such as password safety, phishing, malware, VPNs, safe browsing, and more!";
        }
        else
        {
            return "I didn't quite understand that. Could you please rephrase? I'm here to answer your cybersecurity questions.";
        }
    }

    // Main Chatbot logic
    static void Main(string[] args)
    {
        Console.Clear();
        PlayGreetingAudio();
        PrintASCIIArt();

        string userName = GetUserName();
        Console.Clear();

        SimulateTyping($"Hello, {userName}! Welcome to the Cybersecurity Awareness Bot.");
        Console.WriteLine("Feel free to ask me anything related to cybersecurity.");

        bool exitChat = false;
        while (!exitChat)
        {
            SimulateTyping("How can I help you today?");
            string userInput = Console.ReadLine().ToLower();

            if (userInput.Contains("exit") || userInput.Contains("quit") || userInput.Contains("bye"))
            {
                exitChat = true;
                SimulateTyping("Goodbye! Stay safe online!");
            }
            else
            {
                string response = GetResponse(userInput);
                SimulateTyping(response);
            }
        }
    }
}
