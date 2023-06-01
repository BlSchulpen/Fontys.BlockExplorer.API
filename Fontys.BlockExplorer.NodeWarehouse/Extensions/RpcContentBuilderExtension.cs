using System.Text;

namespace Fontys.BlockExplorer.NodeWarehouse.Extensions
{
    public static class RpcContentBuilderExtension
    {
        public static string RpcContent(Dictionary<string, string> argumentNameToValueMap, List<string> contentParams)
        {
            var contentBuilder = new StringBuilder("{");
            AddArguments(contentBuilder, argumentNameToValueMap);
            AddParams(contentBuilder, contentParams);
            contentBuilder.Append("}");
            return contentBuilder.ToString();
        }

        private static void AddArguments(StringBuilder contentBuilder, Dictionary<string, string> argumentNameToValueMap)
        {
            foreach (var kvp in argumentNameToValueMap)
            {
                var addition = string.Format("\"{Argument}\":\"{Value}\",", kvp.Key, kvp.Value);
                contentBuilder.Append(addition);
            }
        }

        private static void AddParams(StringBuilder contentBuilder, List<string> contentParams)
        {
            if (contentParams.Count == 0)
            {
                contentBuilder.Length--;
                return;
            }

            contentBuilder.Append("\"params:\"[");
            foreach (var param in contentParams)
            {
                var addition = string.Format("\"{Param}\",", param);
                contentBuilder.Append(addition);
            }
            contentBuilder.Length--;
            contentBuilder.Append("]");
        }
    }
}