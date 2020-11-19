using System.Collections.Generic;

public class OpenWeatherResponse
{
    public string Name { get; set; }
    public string Timezone { get; set; }

    public IEnumerable<WeatherDescription> Weather { get; set; }

    public Main Main { get; set; }
    public Wind Wind { get; set; }
}

public class WeatherDescription
{
    public string Main { get; set; }
    public string Description { get; set; }
}

public class Main
{
    public string Temp { get; set; }
    public string Feels_Like { get; set; }
    public string Humidity { get; set; }
}

public class Wind
{
    public string Speed { get; set; }
    public string Deg { get; set; }
}