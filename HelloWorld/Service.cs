using System.Runtime.ConstrainedExecution;

namespace com.example.demo;

public class Service {

    private static ILoggerFactory factory = null;
    private static ILogger logger=null;

    static Service(){
        factory = LoggerFactory.Create(builder => builder.AddConsole());
        logger = factory.CreateLogger<Service>();
    }

    public void logging1(){
        logger.LogInformation("Hello World 1! Logging is {Description}.", "fun");
    }
    public void logging2(){
        logger.LogInformation("Hello World 2! Logging is {Description}.", "fun");
    }
}