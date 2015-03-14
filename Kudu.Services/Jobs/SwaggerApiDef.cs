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
        public String swagger {get; set;}
        [JsonProperty(PropertyName = "info")]
        public SwaggerApiDefInfo Info { get; private set; }

        // host string should be part of the object because it will be replaced by the microservice RP
        public String host { get; private set; }
        public List<String> schemes { get; private set; }
        //public object definitions { get; set; }
        public Dictionary<String, PathItem> paths { get; set; }

        public SwaggerApiDef(IEnumerable<JobBase> triggeredJobs)
        {
            swagger = "2.0";
            Info = new SwaggerApiDefInfo();
            host = "placeHolder";
            schemes = new List<String> {"http", "https"};
            paths = new Dictionary<string, PathItem>();
            //definitions = new object();
            foreach (var triggeredJob in triggeredJobs)
            {
                paths.Add("/" + triggeredJob.Name + "/run", PathItem.GetDefaultPathItem(triggeredJob.Name + "_Post"));
            }
        }
    }

    public class SwaggerApiDefInfo
    {
        public String version { get; set; }
        public String title { get; set; }

        public SwaggerApiDefInfo()
        {
            version = "v1";
            title = "WebJobAsMicroService";
        }
    }

    public class PathItem
    {
        public Operation post { get; set; }

        public static PathItem GetDefaultPathItem(string id)
        {
            PathItem item = new PathItem();
            item.post = Operation.GetDefaultOperation(id);
            return item;
        }
    }

    public class Operation
    {
        public bool deprecated { set; get; }
        public String operationId { set; get; }
        public IEnumerable<String> consumes { set; get; }
        public IEnumerable<String> produces { set; get; }
        public IDictionary<string, Response> responses { set; get; }
        //public List<Parameter> parameters { set; get; }
        public static Operation GetDefaultOperation(String id)
        {
            Operation operation = new Operation();
            operation.deprecated = false;
            operation.operationId = id;
            operation.responses = new Dictionary<String,Response>();
            operation.responses.Add("200", new Response { description = "Success" });
            operation.consumes = new List<String>();
            operation.produces = new List<String>();
            //operation.parameters = new List<Parameter>();
            //operation.parameters.Add(Parameter.GetDefaultParameter());
            return operation;
        }
    }

    public class Parameter
    {
        public string name { set; get; }
        public string @in {set; get;}
        public string description { set; get; }
        public bool required { set; get; }
        public string type { set; get; }
        public static Parameter GetDefaultParameter()
        {
            return new Parameter
            {
                name = "arguments",
                @in = "query",
                description = "Web Job Arguments",
                required = false,
                type = "string"
            };
        }

    }
    public class Response
    {
        public string description { get; set; }
    }
}
