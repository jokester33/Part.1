using System;
using System.Collections.Generic;
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

    // Part 2: sentiment detection
    static string DetectSentiment(string input)
    {
        if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            return "worried";
        if (input.Contains("curious") || input.Contains("interested"))
            return "curious";
        if (input.Contains("frustrated") || input.Contains("angry"))
            return "frustrated";
        return "neutral";
    }

    static string GetSentimentPrefix(string sentiment)
    {
        return sentiment switch
        {
            "worried" => "It's completely understandable to feel that way. ",
            "curious" => "I'm glad you're curious! ",
            "frustrated" => "I hear you—it can be overwhelming. ",
            _ => ""
        };
    }

    static bool IsMostlyNumbers(string s)
    {
        var stripped = s.Replace(" ", "");
        int digits = 0;
        foreach (var c in stripped) if (char.IsDigit(c)) digits++;
        return stripped.Length > 0 && (double)digits / stripped.Length > 0.6;
    }

    //// Part 1: play greeting audio
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

    // Part 1: typing effect
    static void SimulateTyping(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(30);
        }
        Console.WriteLine();
    }

    // Part 1: ASCII art logo
    static void PrintASCIIArt()
    {
        string asciiArt = @"
 ██████╗██╗   ██╗██████╗ ███████╗██████╗ ██████╗  ██████╗ ████████╗            
██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔═══██╗╚══██╔══╝            
██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██████╔╝██║   ██║   ██║               
██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██╔══██╗██║   ██║   ██║               
╚██████╗   ██║   ██████╔╝███████╗██║  ██║██████╔╝╚██████╔╝   ██║               
 ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝    ╚═╝";
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(asciiArt);
        Console.ResetColor();
    }

    // Part 1: ask for and return user name
    static string GetUserName()
    {
        SimulateTyping("What is your name?");
        string name = Console.ReadLine();
        return string.IsNullOrWhiteSpace(name) ? "Guest" : name;
    }

    // Part 2: each handler sets currentTopic, maybe memory, and returns a random tip
    static string HandlePhishing(string _)
    {
        currentTopic = "phishing";
        return topicTips["phishing"][rnd.Next(topicTips["phishing"].Count)];
    }

    static string HandlePassword(string _)
    {
        currentTopic = "password";
        return topicTips["password"][rnd.Next(topicTips["password"].Count)];
    }

    static string HandlePrivacy(string _)
    {
        currentTopic = "privacy";
        if (string.IsNullOrEmpty(favoriteTopic))
        {
            favoriteTopic = "privacy";
            SimulateTyping("Great! I'll remember that you're interested in privacy.");
        }
        return topicTips["privacy"][rnd.Next(topicTips["privacy"].Count)];
    }

    // Part 2: dynamic response layer
    static string GetDynamicResponse(string input, string userName)
    {
        var lower = input.ToLower();

        if (IsMostlyNumbers(lower))
            return "I think you entered mostly numbers—could you rephrase that in a sentence?";

        var sentiment = DetectSentiment(lower);
        var prefix = GetSentimentPrefix(sentiment);

        // Follow-up: "more"/"another"
        if ((lower.Contains("more") || lower.Contains("another") || lower.Contains("again"))
            && !string.IsNullOrEmpty(currentTopic)
            && handlers.ContainsKey(currentTopic))
        {
            var tip = handlers[currentTopic](lower);
            return prefix + tip;
        }

        // Keyword-based delegate dispatch
        foreach (var kv in handlers)
        {
            if (lower.Contains(kv.Key))
            {
                var tip = kv.Value(lower);
                return prefix + tip;
            }
        }

        // Memory-recall
        if (lower.Contains("remember") && !string.IsNullOrEmpty(favoriteTopic))
        {
            return $"Yes, {userName}, you mentioned you're interested in {favoriteTopic}.";
        }

        // Not handled here → signal to fallback
        return null;
    }

    // Part 1: original fallback responses
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



    // Main loop
    static void Main(string[] args)
    {
        PlayGreetingAudio();
        PrintASCIIArt();

        string userName = GetUserName();
        Console.Clear();

        SimulateTyping($"Hello, {userName}! Welcome to the Cybersecurity Awareness Bot.");

        bool exitChat = false;
        while (!exitChat)
        {
            SimulateTyping("How can I help you today?");
            string userInput = Console.ReadLine() ?? "";

            if (userInput.ToLower().Contains("exit") ||
                userInput.ToLower().Contains("quit") ||
                userInput.ToLower().Contains("bye"))
            {
                exitChat = true;
                SimulateTyping("Goodbye! Stay safe online!");
                continue;
            }

            // Try Part 2 dynamic layer first
            string dyn = GetDynamicResponse(userInput, userName);
            if (!string.IsNullOrEmpty(dyn))
                SimulateTyping(dyn);
            else
                // Fallback to original Part 1 logic
                SimulateTyping(GetResponse(userInput));
        }
    }
}
