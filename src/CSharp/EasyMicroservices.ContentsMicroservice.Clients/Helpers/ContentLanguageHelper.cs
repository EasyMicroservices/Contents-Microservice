using Contents.GeneratedServices;
using EasyMicroservices.ContentsMicroservice.Clients.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Clients.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentLanguageHelper
    {
        private readonly ContentClient _contentClient;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentClient"></param>
        public ContentLanguageHelper(ContentClient contentClient)
        {
            _contentClient = contentClient;
        }

        string GetUniqueIdentity(object contract)
        {
            var type = contract.GetType();
            var uniqueIdentity = type.GetProperty("UniqueIdentity", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (uniqueIdentity == null)
                throw new Exception($"Property UniqueIdentity not found in type {type.FullName} to ResolveContentLanguage!");
            return uniqueIdentity.GetValue(contract) as string;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public Task ResolveContentLanguage(object contract, string language)
        {
            return ResolveContentLanguage(contract, language, new HashSet<object>());
        }

        async Task ResolveContentLanguage(object contract, string language, HashSet<object> mappedItems)
        {
            if (contract.Equals(default) || mappedItems.Contains(contract))
                return;
            var type = contract.GetType();
            mappedItems.Add(contract);
            foreach (var property in contract.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (property.GetCustomAttribute<ContentLanguageAttribute>() != null)
                {
                    var cotentResult = await _contentClient.GetByLanguageAsync(new GetByLanguageRequestContract()
                    {
                        Key = GetUniqueIdentity(contract) + "-" + property.Name,
                        Language = language
                    });
                    if (cotentResult.IsSuccess)
                        property.SetValue(contract, cotentResult.Result.Data);
                }
                else if (IsClass(property.PropertyType) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var items = property.GetValue(contract);
                    if (items == null)
                        continue;
                    foreach (var item in (IEnumerable)items)
                    {
                        await ResolveContentLanguage(item, language, mappedItems);
                    }
                }
                else if (IsClass(property.PropertyType))
                {
                    var value = property.GetValue(contract);
                    if (value != null)
                        await ResolveContentLanguage(value, language, mappedItems);
                }
            }
        }

        bool IsClass(Type type)
        {
            return type.GetTypeInfo().IsClass && typeof(string) != type && typeof(char[]) != type;
        }
    }
}
