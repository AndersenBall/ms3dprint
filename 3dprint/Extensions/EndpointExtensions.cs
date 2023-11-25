using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
  
 public static class EndpointExtensions 
 { 
     public static IServiceCollection AddEndpointDefinitions(this IServiceCollection services, params Type[] scanMarkers) 
     { 
         var endpointDefinitions = new List<IEndpoints>(); 
  
         foreach (var marker in scanMarkers) 
             endpointDefinitions.AddRange( 
                 marker.Assembly.ExportedTypes 
                     .Where(x => typeof(IEndpoints).IsAssignableFrom(x) && 
                                 !x.IsAbstract && 
                                 !x.IsInterface) 
                     .Select(Activator.CreateInstance) 
                     .Cast<IEndpoints>()); 
  
         foreach (var endpointDefinition in endpointDefinitions) endpointDefinition.DefineServices(services); 
  
         services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpoints>); 
  
         return services; 
     } 
  
     public static void UseEndpointDefinitions(this WebApplication app) 
     { 
         var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpoints>>(); 
  
         foreach (var endpointDefinition in definitions) endpointDefinition.DefineEndpoints(app); 
     } 
 }
