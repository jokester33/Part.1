using System;
using System.Threading;
using System.IO;
using System.Speech;
using System.Media;

class CyberSecurityChatbot
{
    // Part 2: delegate for topic handlers
    delegate string TopicHandler(string input);

    // Part 2: generic collection of tips per keyword
    static readonly Dictionary<string, List<string>> topicTips = new Dictionary<string, List<string>>()
    {
        { "phishing", new List<string> {
            "Be cautious of unsolicited emails or links asking for personal information.",
            "Verify the sender's address and look for spelling mistakes or urgent language.",
            "If something feels off, contact the organisation directly rather than using embedded links."
        }},
        { "password", new List<string> {
            "Use a mix of uppercase, lowercase, numbers, and symbols in your passwords.",
            "Never reuse the same password across multiple accounts.",
            "Consider using a reputable password manager to keep track of your credentials."
        }},
        { "privacy", new List<string> {
            "Review and limit app permissions on your devices.",
            "Keep your social media profiles private and think twice before sharing personal info.",
            "Enable multi-factor authentication wherever possible."
        }}
    };
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
    // Part 2: map keywords → handler methods
    static readonly Dictionary<string, TopicHandler> handlers = new Dictionary<string, TopicHandler>()
    {
        { "phishing", HandlePhishing },
        { "password", HandlePassword },
        { "privacy",  HandlePrivacy  }
    };

    // Part 2: state & memory
    static string currentTopic = null;
    static string favoriteTopic = "";
    static readonly Random rnd = new Random();

    // UI functionality (function to simulate typing effect)
    static void SimulateTyping(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(30); //Typing effect pauses
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

        //Phishing responses
        if (input.Contains("phishing") || input.Contains("phising")) 
        {
            return "Phishing is when attackers try to trick you into giving out sensitive information. Be cautious of unsolicited emails or links.";
        }

        if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey") || input.Contains("greetings"))
        {
            return "Hello there! How can I assist you with cybersecurity today?";
        }
        else if (input.Contains("How are you") || input.Contains("how are you") || input.Contains("how r u"))

            return "Im just a bunch of code but im running fanstatic!Thanks for asking";

        //Purpose responses
        else if (input.Contains("purpose") || input.Contains("what do you do") || input.Contains("why are you here") || input.Contains("goal") || input.Contains("job"))
        {
            return "I'm here to help you stay safe online by providing valuable cybersecurity tips and answering your questions.";
        }

        //Password responses
        else if (input.Contains("password") || input.Contains("passwords") || input.Contains("safety") || input.Contains("secure"))
        {
            return "Always use strong, unique passwords for each account. A good password should contain a mix of letters, numbers, and symbols.";
        }

        //Safe browsing reponses
        else if (input.Contains("safe browsing") || input.Contains("browsing") || input.Contains("browsing security") || input.Contains("browser"))
        {
            return "Make sure to only visit websites that are 'https' secure. Avoid clicking on suspicious links.";
        }

        //Password
        else if (input.Contains("password manager") || input.Contains("manager"))
        {
            return "A password manager securely stores and organizes your passwords, making it easier to use strong, unique passwords without forgetting them.";
        }

        //Malware
        else if (input.Contains("malware") || input.Contains("virus") || input.Contains("spyware") || input.Contains("trojan"))
        {
            return "Malware is malicious software designed to harm your system. Make sure your antivirus software is up-to-date and avoid downloading files from untrusted sources.";
        }

        //Vpn-related
        else if (input.Contains("vpn") || input.Contains("virtual private network"))
        {
            return "A VPN (Virtual Private Network) encrypts your internet connection and hides your IP address, enhancing your privacy while browsing.";
        }

        //General inquiry response
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


