using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.SqlServer.Transformers
{
    internal static class UsageDataTransformers
    {
        public static ResourceQuota? ExtractQuotaOfKind(this List<UserQuotaSqlModel> dbModels, QuotaTemplateSqlModel quotaKind)
        {
            ResourceQuota? dto = null;

            var sql = dbModels.FirstOrDefault(m => m.QuotaTemplateId == quotaKind.Id);
            if(sql!=null)
            {
                dto = sql.ToDto(quotaKind);
            }

            return dto;
        }

        public static ResourceQuota? ToDto(this UserQuotaSqlModel? sql, QuotaTemplateSqlModel quotaKind)
        {
            ResourceQuota? dto = null;

            if (sql != null)
            {
                dto = new ResourceQuota()
                {
                    QuotaId = sql.Id,
                    MeteredResource = quotaKind.ResourceKind.ToEnum<MeteredResourceKind>(),
                    Budget = sql.CurrentBudget,
                    Consumption = sql.ConsumedBudget,
                };
            }
            return dto;
        }

        public static TokenConsumptionSqlModel ToRow(this TokenConsumptionEntry dto)
        {
            TokenConsumptionSqlModel row = new TokenConsumptionSqlModel()
            {
                UserId = dto.UserId,
                InferenceTimeUtc = dto.InferenceTimeUtc,
                FunctionName = dto.FunctionName,
                modelId = dto.modelId,
                PromptTokens = dto.PromptTokens,
                CompletionTokens = dto.CompletionTokens
            };

            return row;
        }

        public static T? ToEnum<T>(this string value) where T:Enum
        {
            T? converted = default(T?);

            if(Enum.TryParse(typeof(T), value, out object? parsed))
            {
                converted = (T?) parsed;
            }

            return converted;
        }
    }
}
