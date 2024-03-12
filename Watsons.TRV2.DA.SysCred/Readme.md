1.Make sure solution is build successful.

2.Open Package Manager Console, run command below:
(a) CashManage Database
`Scaffold-DbContext 'Server=10.98.32.185;Database=SysCred;User ID=sa;Password=!QAZ2wsx#EDC;TrustServerCertificate=true;' Microsoft.EntityFrameworkCore.SqlServer -Force -Context SysCredContext -OutputDir Entities -ContextDir Entities -Namespace Watsons.TRV2.DA.SysCred.Entities -ContextNamespace Watsons.TRV2.DA.SysCred.Entities -Tables MfaApplication, MfaUser, MfaUserLogin, UserApplicationMapping, OTPTracking -Project Watsons.TRV2.DA.SysCred -StartupProject Watsons.TRV2.DA.SysCred`

3.Remove OnConfiguring from <project_name>Context.cs

4. NativeAOT project must compile model before use it.
`dotnet ef dbcontext optimize --output-dir MyDbContextModel --context MyDbContext --namespace MyProject.Data`