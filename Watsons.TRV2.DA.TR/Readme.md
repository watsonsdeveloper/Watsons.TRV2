1.Make sure solution is build successful.

2.Open Package Manager Console, run command below:
(a) TRV2 Database
`Scaffold-DbContext 'Server=10.98.32.248;Database=TRV2;User ID=sa;Password=!QAZ2wsx#EDC;TrustServerCertificate=true;' Microsoft.EntityFrameworkCore.SqlServer -Force -Context TrContext -OutputDir Entities -ContextDir Entities -Namespace Watsons.TRV2.DA.TR.Entities -ContextNamespace Watsons.TRV2.DA.TR.Entities -Project Watsons.TRV2.DA.TR -StartupProject Watsons.TRV2.DA.TR`
(b) TRV2_UAT Database
`Scaffold-DbContext 'Server=10.98.32.248;Database=TRV2_UAT;User ID=sa;Password=!QAZ2wsx#EDC;TrustServerCertificate=true;' Microsoft.EntityFrameworkCore.SqlServer -Force -Context TrContext -OutputDir Entities -ContextDir Entities -Namespace Watsons.TRV2.DA.TR.Entities -ContextNamespace Watsons.TRV2.DA.TR.Entities -Project Watsons.TRV2.DA.TR -StartupProject Watsons.TRV2.DA.TR`

3.Remove OnConfiguring from <project_name>Context.cs

4. change ``public virtual`` to `internal virtual` prevent other able use context directly.

5. NativeAOT project must compile model before use it.
`dotnet ef dbcontext optimize --output-dir MyDbContextModel --context MyDbContext --namespace MyProject.Data`