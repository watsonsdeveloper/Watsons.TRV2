1.Make sure solution is build successful.

2.Open Package Manager Console, run command below:
(a) MyMaster Database
`Scaffold-DbContext 'Server=10.98.32.121;Database=MyMaster;User ID=Digital;Password=!QAZ2wsx#EDC;TrustServerCertificate=true;' Microsoft.EntityFrameworkCore.SqlServer -Force -Context MyMasterContext -OutputDir Entities -ContextDir Entities -Namespace Watsons.TRV2.DA.MyMaster.Entities -ContextNamespace Watsons.TRV2.DA.MyMaster.Entities -Tables ItemMaster, StoreMaster -Project Watsons.TRV2.DA.MyMaster -StartupProject Watsons.TRV2.DA.MyMaster`

3.Remove OnConfiguring from <project_name>Context.cs

4. NativeAOT project must compile model before use it.
`dotnet ef dbcontext optimize --output-dir MyDbContextModel --context MyDbContext --namespace MyProject.Data`