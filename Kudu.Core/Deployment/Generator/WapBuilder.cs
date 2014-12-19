using System;
using System.Text;
using Kudu.Contracts.Settings;
using Kudu.Core.Infrastructure;

namespace Kudu.Core.Deployment.Generator
{
    public class WapBuilder : GeneratorSiteBuilder
    {
        private readonly string _projectPath;
        private readonly string _solutionPath;
        private readonly bool _betaMsBuild;

        public WapBuilder(IEnvironment environment, IDeploymentSettingsManager settings, IBuildPropertyProvider propertyProvider, string sourcePath, string projectPath, string solutionPath, bool betaMsBuild)
            : base(environment, settings, propertyProvider, sourcePath)
        {
            _projectPath = projectPath;
            _solutionPath = solutionPath;
            _betaMsBuild = betaMsBuild;
        }

        protected override string ScriptGeneratorCommandArguments
        {
            get
            {
                var commandArguments = new StringBuilder();
                commandArguments.AppendFormat("--aspWAP \"{0}\"", _projectPath);

                if (!String.IsNullOrEmpty(_solutionPath))
                {
                    commandArguments.AppendFormat(" --solutionFile \"{0}\"", _solutionPath);
                }
                else
                {
                    commandArguments.AppendFormat(" --no-solution", _solutionPath);
                }

                if (this._betaMsBuild)
                {
                    commandArguments.AppendFormat(" --msbuildPath \"{0}\"", PathUtility.ResolveMSBuildPath(useBeta: true));
                }

                return commandArguments.ToString();
            }
        }

        public override string ProjectType
        {
            get { return "ASP.NET WAP"; }
        }
    }
}
