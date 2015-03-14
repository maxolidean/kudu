using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kudu.Contracts.Jobs;
using Newtonsoft.Json;

namespace Kudu.Services.Jobs
{
    public class SwaggerApiDef
    {
        [JsonProperty(PropertyName = "swagger")]
        public String Swagger { get; set; }
        [JsonProperty(PropertyName = "info")]
        public SwaggerApiDefInfo Info { get; private set; }
        [JsonProperty(PropertyName = "host")]
        public String Host { get; private set; }
        [JsonProperty(PropertyName = "schemes")]
        public List<String> Schemes { get; private set; }
        //public object definitions { get; set; }
        [JsonProperty(PropertyName = "paths")]
        public Dictionary<String, PathItem> Paths { get; set; }
        public SwaggerApiDef(IEnumerable<JobBase> triggeredJobs)
        {
            Swagger = "2.0";
            Info = new SwaggerApiDefInfo();
            Host = "placeHolder";
            Schemes = new List<String> { "http", "https" };
            Paths = new Dictionary<string, PathItem>();
            //definitions = new object();
            foreach (var triggeredJob in triggeredJobs)
            {
                Paths.Add("/" + triggeredJob.Name + "/run", PathItem.GetDefaultPathItem(triggeredJob.Name + "_Post"));
            }
        }
    }

    public class SwaggerApiDefInfo
    {
        [JsonProperty(PropertyName = "version")]
        public String Version { get; set; }
        [JsonProperty(PropertyName = "title")]
        public String Title { get; set; }

        public SwaggerApiDefInfo()
        {
            Version = "v1";
            Title = "WebJobAsMicroService";
        }
    }

    public class PathItem
    {
        [JsonProperty(PropertyName = "post")]
        public Operation Post { get; set; }

        public static PathItem GetDefaultPathItem(string id)
        {
            PathItem item = new PathItem();
            item.Post = Operation.GetDefaultOperation(id);
            return item;
        }
    }

    public class Operation
    {
        [JsonProperty(PropertyName = "deprecated")]
        public bool Deprecated { set; get; }
        [JsonProperty(PropertyName = "operationId")]
        public String OperationId { set; get; }
        [JsonProperty(PropertyName = "consumes")]
        public IEnumerable<String> Consumes { set; get; }
        [JsonProperty(PropertyName = "produces")]
        public IEnumerable<String> Produces { set; get; }
        [JsonProperty(PropertyName = "responses")]
        public IDictionary<string, Response> Responses { set; get; }
        //public List<Parameter> parameters { set; get; }
        public static Operation GetDefaultOperation(String id)
        {
            Operation operation = new Operation();
            operation.Deprecated = false;
            operation.OperationId = id;
            operation.Responses = new Dictionary<String, Response>();
            operation.Responses.Add("200", new Response { Description = "Success" });
            operation.Consumes = new List<String>();
            operation.Produces = new List<String>();
            //operation.parameters = new List<Parameter>();
            //operation.parameters.Add(Parameter.GetDefaultParameter());
            return operation;
        }
    }

    public class Parameter
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { set; get; }
        [JsonProperty(PropertyName = "in")]
        public string Input { set; get; }
        [JsonProperty(PropertyName = "description")]
        public string Description { set; get; }
        [JsonProperty(PropertyName = "required")]
        public bool Required { set; get; }
        [JsonProperty(PropertyName = "type")]
        public string Type { set; get; }
        public static Parameter GetDefaultParameter()
        {
            return new Parameter
            {
                Name = "arguments",
                Input = "query",
                Description = "Web Job Arguments",
                Required = false,
                Type = "string"
            };
        }

    }
    public class Response
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
