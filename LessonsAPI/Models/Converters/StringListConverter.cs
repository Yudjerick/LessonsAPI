using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace LessonsAPI.Models.Converters
{
    public class StringListConverter: ValueConverter<List<string>, string>
    {
        public StringListConverter() :
            base(v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>()) { }
    }
}
