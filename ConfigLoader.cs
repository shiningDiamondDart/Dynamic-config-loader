public class ConfigLoader
{
 public static TSettings LoadConfig<TSettings>() where TSettings : class, new()
 {
     //Console.WriteLine(AppContext.BaseDirectory.ToString());
     var configFileName = $"{typeof(TSettings).Name}.json";
     
     //very shitty solution, fix before production
     string configsDirectory = //put Path to configs here
     Console.WriteLine(configsDirectory);

     var config = new ConfigurationBuilder()
         .SetBasePath(configsDirectory)
         .AddJsonFile(configFileName,
             optional: false,
             reloadOnChange: true)
         .Build();

     TSettings settings = new TSettings();

     IDictionary<string, object> settingsDict = new Dictionary<string, object>();
     config.Bind(settingsDict);

     settings = CreateInstanceFromDictionary<TSettings>(settingsDict);

     return settings;
 }
 public static T CreateInstanceFromDictionary<T>(IDictionary<string, object> dictionary) where T : new()
 {
     var obj = new T();
     var type = typeof(T);

     foreach (var kvp in dictionary)
     {
        //gets type property with name = current dict key, filtering by instanced and public properties
         PropertyInfo? property = type.GetProperty(kvp.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
         if (property != null)
         {
             //var convertedValue = Convert.ChangeType(kvp.Value, int.TryParse(kvp.Value.ToString(), out int intVvalue) ? typeof(int) : typeof(string));
             if (kvp.Value.GetType() != property.GetType())
             {
                 MethodInfo? parseMethod = property.PropertyType.GetMethod("Parse", new[] {typeof(string)});

                 if (parseMethod != null)
                 {
                     property.SetValue(obj, parseMethod.Invoke(null, new object[] { kvp.Value }));
                 }
             } else
             {
                 property.SetValue(obj, kvp.Value);
             }                 
             
         }
     }

     return obj;
 }

}