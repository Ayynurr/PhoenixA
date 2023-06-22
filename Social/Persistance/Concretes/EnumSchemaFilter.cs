using Application.Abstracts;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ISchemaFilter = Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter;

namespace Persistance.Concretes;

public class EnumSchemaFilter : ISchemaFilter
{


    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();

            var enumValues = Enum.GetValues(context.Type);
            var enumNames = Enum.GetNames(context.Type);
            var enumDictionary = new Dictionary<object, string>();

            for (var i = 0; i < enumValues.Length; i++)
            {
                var enumValue = enumValues.GetValue(i);
                var enumName = enumNames[i];
                enumDictionary.Add(enumValue, enumName);
            }

            foreach (var enumValue in enumDictionary.OrderBy(kv => kv.Value))
            {
                var enumName = enumValue.Value;
                schema.Enum.Add(new OpenApiString(enumName));
            }
        }
    }
}

