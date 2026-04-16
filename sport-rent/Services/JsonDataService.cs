namespace sport_rent.Services
{
    public class JsonDataService
    {
        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "equipment.json");

        public List<T> LoadData<T>()
        {
            if (!File.Exists(_path)) return new List<T>();
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }
}