﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Rules;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldRulesMapper : AbstractSitecoreFieldMapper
    {
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
           
        }

        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type ruleFactory = typeof(RuleFactory);
            var method = ruleFactory.GetMethod("GetRules",
                                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                                                null,
                                                new Type[] { typeof(global::Sitecore.Data.Fields.Field) },
                                                null);
            Type[] genericArgs = Configuration.PropertyInfo.PropertyType.GetGenericArguments();
            method = method.MakeGenericMethod(genericArgs);
            object rules = method.Invoke(null, new object[] { field });
            return rules;
        }
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            var type = configuration.PropertyInfo.PropertyType;
            if (!type.IsGenericType)
            {
                return false;
            }
            var baseType = type.GetGenericTypeDefinition();
            if (baseType == typeof(RuleList<>))
            {
                return true;
            }
            return false;
        }
    }
}
